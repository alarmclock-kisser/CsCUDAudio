using NAudio.Wave;
using System.IO;
using System.Text;

namespace WAV_CUDA
{
	public class AudioHandling
	{
		// Attributes
		public List<byte[]> Tracks = [];
		public List<string> Names = [];
		public WaveStream? Stream;
		public List<byte[]> Current = [];
		private WaveOutEvent? waveOut;

		// Constructor
		public AudioHandling()
		{
			// Empty
		}

		// Player methods
		public void Play(WaveStream stream)
		{
			// Stop current stream
			Stop();

			// Set new stream
			Stream = stream;

			// Set new waveOut
			waveOut = new WaveOutEvent();
			waveOut.DeviceNumber = 0;
			waveOut.Init(stream);
			waveOut.Play();

			// Dispose stream on playback end
			waveOut.PlaybackStopped += (sender, e) =>
			{
				Stream = null;
			};
		}


		public void Stop()
		{
			waveOut?.Stop();
			waveOut = null;

			Stream = null;
		}

		// Methods
		public byte[] AddTrack(string path)
		{
			// Empty byte array
			byte[] data = Array.Empty<byte>();

			// If paths extension is ".wav": Read WAV-File as byte[]
			if (Path.GetExtension(path).ToLower() == ".wav")
			{
				using var reader = new WaveFileReader(path);
				using var ms = new MemoryStream();
				reader.CopyTo(ms);
				data = ms.ToArray();
			}

			// If paths extension is ".mp3": Convert & Read MP3-File as byte[]
			else if (Path.GetExtension(path).ToLower() == ".mp3")
			{
				using var reader = new Mp3FileReader(path);
				using var pcmStream = WaveFormatConversionStream.CreatePcmStream(reader);
				using var ms = new MemoryStream();
				WaveFileWriter.WriteWavFileToStream(ms, pcmStream);
				data = ms.ToArray();
			}

			// Add name to Names
			Names.Add(Path.GetFileName(path));

			// Add & return data
			Tracks.Add(data);
			return data;
		}

		public void RemoveTrack(string name = "", int index = -1)
		{
			// If index is -1 & name is not "": Set index to index of name
			if (index == -1 && name != "")
			{
				index = Names.IndexOf(name);
			}

			// Remove Track & Name at index
			Tracks.RemoveAt(index);
			Names.RemoveAt(index);
		}

		public int ClearTracks()
		{
			// New counter
			int count = Tracks.Count;

			// Clear Tracks & Names
			Tracks.Clear();
			Names.Clear();

			// Return the count of cleared tracks
			return count;
		}

		public byte[] GetTrack(string name = "", int index = -1, WaveStream? stream = null)
		{
			// If index is -1 & name is not "": Set index to index of name
			if (index == -1 && name != "")
			{
				index = Names.IndexOf(name);
			}

			// If stream is not null: Read stream as byte[]
			if (stream != null)
			{
				using var ms = new MemoryStream();
				stream.CopyTo(ms);
				return ms.ToArray();
			}

			// Return Track at index
			return Tracks[index];
		}

		public byte[] AddWaveHeader(byte[] data, int sampleRate = 44100, int channels = 2, int bitsPerSample = 16)
		{
			int dataChunkSize = data.Length;
			int subChunk1Size = 16; // Größe des 'fmt ' Subchunks
			int subChunk2Size = dataChunkSize; // Größe des 'data' Subchunks
			int chunkSize = 4 + (8 + subChunk1Size) + (8 + subChunk2Size); // Gesamtgröße des RIFF-Chunks

			using var ms = new MemoryStream();
			using (var writer = new BinaryWriter(ms))
			{
				// RIFF Header
				writer.Write(new char[] { 'R', 'I', 'F', 'F' });
				writer.Write(chunkSize); // Größe des gesamten Files abzüglich "RIFF" und die Chunk-Größe selbst
				writer.Write(new char[] { 'W', 'A', 'V', 'E' });

				// fmt Subchunk
				writer.Write(new char[] { 'f', 'm', 't', ' ' });
				writer.Write(subChunk1Size); // Länge des fmt-Blocks
				writer.Write((short) 1); // Audioformat (1 = PCM)
				writer.Write((short) channels); // Anzahl der Kanäle
				writer.Write(sampleRate); // Sample-Rate
				writer.Write(sampleRate * channels * bitsPerSample / 8); // Byte-Rate
				writer.Write((short) (channels * bitsPerSample / 8)); // Block Align
				writer.Write((short) bitsPerSample); // Bits per Sample

				// data Subchunk
				writer.Write(new char[] { 'd', 'a', 't', 'a' });
				writer.Write(subChunk2Size); // Größe der Daten
				writer.Write(data); // Audiodaten
			}

			return ms.ToArray();
		}

		public float ScanBpm(byte[] data, float threshold = 0.5f, float cutoff = 100.0f, int sampleRate = 44100, int bitsPerSample = 16)
		{
			// Schritt 0: Attribute sampleRate & bps aus Header lesen + totalSamples berechnen
			if (sampleRate == 0)
			{
				sampleRate = GetSampleRateFromHeader(data);
			}
			if (bitsPerSample == 0)
			{
				bitsPerSample = GetBitsPerSampleFromHeader(data);
			}
			int bytesPerSample = bitsPerSample / 8;
			int totalSamples = data.Length / bytesPerSample;

			// Amplituden berechnen
			float[] amplitudes = new float[totalSamples];

			// Schritt 1: Amplituden berechnen
			for (int i = 0; i < totalSamples; i++)
			{
				// Wandeln von Byte-Array in Int16 oder Int32, abhängig von bitsPerSample
				if (bitsPerSample == 16)
				{
					amplitudes[i] = BitConverter.ToInt16(data, i * bytesPerSample) / 32768f; // Normalisierung auf -1 bis 1
				}
				else if (bitsPerSample == 8)
				{
					amplitudes[i] = (data[i] - 128) / 128f; // 8-bit PCM ist unsigned
				}
				else
				{
					throw new NotSupportedException("Nur 8-Bit und 16-Bit PCM werden unterstützt.");
				}
			}

			// Schritt 2: Filter anwenden (z. B. Hochpass-Filter, um niederfrequente Geräusche zu entfernen)
			float[] filteredAmplitudes = ApplyHighPassFilter(amplitudes, sampleRate, cutoff);

			// Schritt 3: Peaks erkennen
			List<float> peakTimes = [];
			for (int i = 1; i < filteredAmplitudes.Length - 1; i++)
			{
				// Einfache Spitzenwert-Erkennung
				if (filteredAmplitudes[i] > threshold &&
					filteredAmplitudes[i] > filteredAmplitudes[i - 1] &&
					filteredAmplitudes[i] > filteredAmplitudes[i + 1])
				{
					float timeInSeconds = i / (float) sampleRate;
					peakTimes.Add(timeInSeconds);
				}
			}

			// Schritt 4: BPM berechnen
			if (peakTimes.Count > 1)
			{
				List<float> intervals = [];
				for (int i = 1; i < peakTimes.Count; i++)
				{
					intervals.Add(peakTimes[i] - peakTimes[i - 1]);
				}

				float averageInterval = intervals.Average();
				float bpm = 60f / averageInterval; // BPM = 60 Sekunden / durchschnittliches Intervall
				return bpm;
			}

			return 0.0f; // Keine Peaks gefunden
		}

		private float[] ApplyHighPassFilter(float[] signal, int sampleRate = 0, float cutoffFrequency = 150.0f)
		{
			// Return signal if empty
			if (signal.Length == 0)
			{
				return signal;
			}

			// Set sample rate
			if (sampleRate == 0)
			{
				sampleRate = GetSampleRateFromHeader(null, signal);
			}

			// Hochpass-Filter: y[n] = alpha * (y[n-1] + x[n] - x[n-1])
			float rc = 1.0f / (2 * (float) Math.PI * cutoffFrequency);
			float dt = 1.0f / sampleRate;
			float alpha = dt / (rc + dt);

			float[] filteredSignal = new float[signal.Length];
			filteredSignal[0] = signal[0];
			for (int i = 1; i < signal.Length; i++)
			{
				filteredSignal[i] = alpha * (filteredSignal[i - 1] + signal[i] - signal[i - 1]);
			}

			return filteredSignal;
		}

		public int GetSampleRateFromHeader(byte[]? bytes = null, float[]? floats = null)
		{
			// New int
			int sampleRate = 44100;

			// If bytes is not null: Get sample rate from byte[]s header
			if (bytes != null)
			{
				// If first 4 bytes are "RIFF"
				if (bytes.Length >= 4 && Encoding.ASCII.GetString(bytes, 0, 4) == "RIFF")
				{
					sampleRate = BitConverter.ToInt32(bytes, 24);
				}
			}

			// If floats is not null: Get sample rate from float[]s header
			else if (floats != null)
			{
				// If first row is "RIFF"
				if (floats.Length >= 6 && floats[0] == 82 && floats[1] == 73 && floats[2] == 70 && floats[3] == 70)
				{
					sampleRate = (int) floats[5];
				}
			}

			// Verify sample rate
			if (sampleRate < 8000 || sampleRate > 192000)
			{
				sampleRate = 44100;
			}

			// Return sample rate
			return sampleRate;
		}

		public int GetBitsPerSampleFromHeader(byte[]? bytes = null, float[]? floats = null)
		{
			// New int
			int bps = 16;

			// If bytes is not null: Get bits per second rate from byte[]s header
			if (bytes != null)
			{
				// If first 4 bytes are "RIFF"
				if (bytes.Length >= 4 && Encoding.ASCII.GetString(bytes, 0, 4) == "RIFF")
				{
					bps = BitConverter.ToInt16(bytes, 34);
				}
			}

			if (floats != null)
			{
				// If first row is "RIFF"
				if (floats.Length >= 6 && floats[0] == 82 && floats[1] == 73 && floats[2] == 70 && floats[3] == 70)
				{
					bps = (int) floats[4];
				}
			}

			// Return bps
			return bps;
		}

		public int GetChannelsFromHeader(byte[]? bytes = null, float[]? floats = null)
		{
			// New int
			int channels = 2;

			// If bytes is not null: Get channels rate from byte[]s header
			if (bytes != null)
			{
				// If first 4 bytes are "RIFF"
				if (bytes.Length >= 4 && Encoding.ASCII.GetString(bytes, 0, 4) == "RIFF")
				{
					channels = BitConverter.ToInt16(bytes, 22);
				}
			}

			// If floats is not null: Get channels rate from float[]s header
			else if (floats != null)
			{
				// If first row is "RIFF"
				if (floats.Length >= 6 && floats[0] == 82 && floats[1] == 73 && floats[2] == 70 && floats[3] == 70)
				{
					channels = (int) floats[3];
				}
			}

			// Return channels
			return channels;
		}
	}

}

