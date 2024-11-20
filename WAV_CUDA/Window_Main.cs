using ManagedCuda.BasicTypes;
using NAudio.Wave;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WAV_CUDA
{
	public partial class Window_Main : Form
	{
		// Attributes
		public CudaHandling CuH = new();
		public AudioHandling AuH = new();


		// Constructor
		public Window_Main()
		{
			InitializeComponent();

			// Dispose CuH & ReloadDevices()
			CuH.Dispose();
			ReloadDevices();

			// Call GUI-Updaters
			UpdatePointers();
			UpdateTracks();
			UpdateWaveViewers();
			UpdateInitLabel();
			ToggleButtons("auto");
		}


		// Methods
		public void ReloadDevices()
		{
			// Clear listBox_devices
			listBox_devices.Items.Clear();

			// Get all devices
			var devices = CuH.GetDeviceNames();

			// Fill listBox_devices with device names
			foreach (var device in devices)
			{
				listBox_devices.Items.Add(device);
			}
		}

		public void UpdatePointers()
		{
			// Clear listBox_pointers
			listBox_pointers.Items.Clear();

			// Fill listBox_pointers with DataPointers
			foreach (var pointer in CuH.DataPointers)
			{
				listBox_pointers.Items.Add(new CUdeviceptr(pointer.Pointer));
			}
		}

		public void UpdateTracks()
		{
			// Clear listBox_tracks
			listBox_files.Items.Clear();

			// Fill listBox_tracks with Names
			foreach (var name in AuH.Names)
			{
				listBox_files.Items.Add(name);
			}
		}

		public void UpdateWaveViewers()
		{
			// If none selected from listBox_files: Clear waveViewer_track & return
			if (listBox_files.SelectedIndex == -1)
			{
				waveViewer_track.SamplesPerPixel = 100;
				waveViewer_track.WaveStream = null;
			}
			else
			{
				// Get selected track data from listBox_files
				byte[] data = AuH.GetTrack(name: listBox_files.SelectedItem!.ToString() ?? "");

				// Add Wave Header
				data = AuH.AddWaveHeader(data);

				// Set waveViewer_track to selected track in listBox_files
				waveViewer_track.SamplesPerPixel = 256;
				waveViewer_track.WaveStream = new WaveFileReader(new MemoryStream(data));
			}


			// If none selected from listBox_pointers: Clear waveViewer_pointer & return
			if (listBox_pointers.SelectedIndex == -1)
			{
				waveViewer_pointer.SamplesPerPixel = 100;
				waveViewer_pointer.WaveStream = null;
			}
			else
			{
				// Get selected pointer data from listBox_pointers
				byte[] data = CuH.GetDataFromPointer((CUdeviceptr) listBox_pointers.SelectedItem!);

				// Add Wave Header
				data = AuH.AddWaveHeader(data);

				// Set waveViewer_pointer to selected pointer in listBox_pointers
				waveViewer_pointer.SamplesPerPixel = 256;

				// Check if data is a valid WAVE file -> Set WaveStream
				if (data.Length > 12 && Encoding.ASCII.GetString(data, 0, 4) == "RIFF")
				{
					waveViewer_pointer.WaveStream = new WaveFileReader(new MemoryStream(data));
				}
				else
				{
					// Handle invalid WAVE data
					waveViewer_pointer.WaveStream = null;
					MessageBox.Show("Invalid WAVE file data.");
				}
			}

			// Update buttons
			ToggleButtons("auto");
		}

		public void UpdateInitLabel()
		{
			// If CuH has Device: Set label_status.Text to "Initialized: " + CuH.Device.Name green & return
			if (CuH.Device != null)
			{
				// If GetDeviceName() is not "": Set label_status.Text to "Initialized: " + CuH.Device.Name green & return
				if (!string.IsNullOrEmpty(CuH.GetDeviceName()))
				{
					label_status.Text = "Initialized: " + CuH.GetDeviceName();
					label_status.ForeColor = Color.Green;
					label_status.Font = new Font(label_status.Font, FontStyle.Regular);
					return;
				}
			}

			// If CuH has no Device: Set label_status.Text to "Init" red & return
			label_status.Text = "Initialize CUDA-Device ...";
			label_status.ForeColor = Color.FromName("HotTrack");
			label_status.Font = new Font(label_status.Font, FontStyle.Underline);
		}

		public void ToggleButtons(string mode = "auto")
		{
			// If mode is "auto": Enable/Disable buttons based on their current state
			if (mode == "auto")
			{
				// If CuH.Device == null OR Name is "" OR listBox_files.Length == 0: Disable button_clearFiles & button_moveAll
				if (listBox_files.Items.Count == 0 || CuH.Device == null || string.IsNullOrEmpty(CuH.GetDeviceName()))
				{
					button_clearFiles.Enabled = false;
					button_moveAll.Enabled = false;
				}
				else
				{
					button_clearFiles.Enabled = true;
					button_moveAll.Enabled = true;
				}

				// If listBox_pointers.Length == 0: Disable button_clearPointers
				if (listBox_pointers.Items.Count == 0)
				{
					button_clearPointers.Enabled = false;
				}
				else
				{
					button_clearPointers.Enabled = true;
				}

				// If CuH has Device: Enable button_resetContext
				if (CuH.Device != null)
				{
					button_resetContext.Enabled = true;
				}
				else
				{
					button_resetContext.Enabled = false;
				}

				// If CuH has no Device OR Name is "" OR no File selected: Disable button_move & button_moveAll
				if (CuH.Device == null || string.IsNullOrEmpty(CuH.GetDeviceName()) || listBox_files.SelectedIndex == -1 || listBox_files.Items.Count == 0)
				{
					button_move.Enabled = false;
				}
				else
				{
					button_move.Enabled = true;
					button_moveAll.Enabled = true;
				}

				// If no pointer selected: Disable button_export
				if (waveViewer_pointer.WaveStream == null)
				{
					button_export.Enabled = false;
				}
				else
				{
					button_export.Enabled = true;
				}

				// If no pointer selected: Disable button_stretch & numericUpDown_bpm
				if (waveViewer_pointer.WaveStream == null)
				{
					button_stretch.Enabled = false;
					numericUpDown_bpm.Enabled = false;
					numericUpDown_bpm.Value = 180;
				}
				else
				{
					button_stretch.Enabled = true;
					numericUpDown_bpm.Enabled = true;
				}

				// If both waveViewers have no Stream: Disable button_scanBpm
				if (waveViewer_track.WaveStream == null && waveViewer_pointer.WaveStream == null)
				{
					button_scanBpm.Enabled = false;
				}
				else
				{
					button_scanBpm.Enabled = true;
				}
			}

			// If mode is "all": Enable all buttons
			if (mode == "all")
			{
				button_clearFiles.Enabled = true;
				button_clearPointers.Enabled = true;
				button_move.Enabled = true;
				button_moveAll.Enabled = true;
				button_resetContext.Enabled = true;
			}

			// If mode is "none": Disable all buttons
			if (mode == "none")
			{
				button_clearFiles.Enabled = false;
				button_clearPointers.Enabled = false;
				button_move.Enabled = false;
				button_moveAll.Enabled = false;
				button_resetContext.Enabled = false;
				button_scanBpm.Enabled = false;
				button_stretch.Enabled = false;
				button_export.Enabled = false;
			}
		}


		// Event Handlers
		private void label_status_Click(object sender, EventArgs e)
		{
			// Disable buttons
			ToggleButtons("none");

			// If CuH has Device: Dispose CuH & set label_status.Text to "Init" red & ReloadDevices() & return
			if (CuH.Device != null || !string.IsNullOrEmpty(CuH.GetDeviceName()))
			{
				CuH.Dispose();
				UpdateInitLabel();
				ReloadDevices();
				return;
			}

			// If listBox_devices.Length == 0: MsgBox & return
			if (listBox_devices.Items.Count == 0)
			{
				MessageBox.Show("No devices found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// If none selected from listBox_devices: Initialize strongest device
			if (listBox_devices.SelectedIndex == -1)
			{
				// Initialize strongest device (-1)
				CuH.Initialize(-1);

				// Update label_status
				UpdateInitLabel();

				// Put " * " infront of the strongest device
				listBox_devices.Items[CuH.GetDeviceId()] = " * " + listBox_devices.Items[CuH.GetDeviceId()];
			}

			// If selected from listBox_devices: Initialize device by name
			var selectedItem = listBox_devices.SelectedItem?.ToString();
			if (selectedItem != null)
			{
				CuH.Initialize(name: selectedItem);

				// Update label_status
				UpdateInitLabel();

				// Put " * " infront of the selected device
				listBox_devices.Items[CuH.GetDeviceId()] = " * " + listBox_devices.Items[CuH.GetDeviceId()];
			}

			// Enable buttons
			ToggleButtons("auto");
		}

		private void listBox_pointers_SelectedIndexChanged(object sender, EventArgs e)
		{
			// ToggleButtons
			ToggleButtons("auto");

			// If none selected from listBox_pointers: return
			if (listBox_pointers.SelectedIndex == -1)
			{
				UpdateWaveViewers();
				return;
			}

			// UpdateWaveViewers()
			UpdateWaveViewers();

			// Return
			return;
		}

		private void listBox_files_SelectedIndexChanged(object sender, EventArgs e)
		{
			// ToggleButtons
			ToggleButtons("auto");

			// If none selected from listBox_files: return
			if (listBox_files.SelectedIndex == -1)
			{
				UpdateWaveViewers();
				return;
			}

			// UpdateWaveViewers()
			UpdateWaveViewers();

			// Return
			return;
		}

		private void button_import_Click(object sender, EventArgs e)
		{
			// Disable buttons
			ToggleButtons("none");

			// OFD (Filter: "Wave Files|*.wav|MP3 Files|*.mp3", Multiselect: true, Title: "Import Tracks", CheckFileExists: true, InitialDirectory: MyMusic)
			using var ofd = new OpenFileDialog
			{
				Filter = "Wave Files|*.wav|MP3 Files|*.mp3",
				Multiselect = true,
				Title = "Import Tracks",
				CheckFileExists = true,
				InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)
			};

			// If OFD OK: Add Tracks & Names & Update Pointers & Tracks
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				foreach (var path in ofd.FileNames)
				{
					AuH.AddTrack(path);
				}

				UpdatePointers();
				UpdateTracks();
			}

			// Enable buttons
			ToggleButtons("auto");

			// Select first item in listBox_files
			listBox_files.SelectedIndex = 0;

			// Return
			return;
		}

		private void button_move_Click(object sender, EventArgs e)
		{
			// Disable buttons
			ToggleButtons("none");

			// If listBox_files.Length == 0: MsgBox & return
			if (listBox_files.Items.Count == 0)
			{
				MessageBox.Show("No tracks found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// If none selected from listBox_tracks: MsgBox & return
			if (listBox_files.SelectedIndex == -1)
			{
				MessageBox.Show("Please select a track first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// Get the selected track name
			var selectedTrackName = listBox_files.SelectedItem?.ToString();
			if (string.IsNullOrEmpty(selectedTrackName))
			{
				MessageBox.Show("Selected track name is null or empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// Otherwise: Move Track & Name
			byte[] trackData = AuH.GetTrack(name: selectedTrackName);
			if (trackData == null)
			{
				trackData = AuH.GetTrack(index: listBox_files.SelectedIndex);
			}

			CuH.MoveDataToDevice(trackData);

			// Update Pointers & Tracks
			UpdatePointers();
			UpdateTracks();

			// Enable buttons
			ToggleButtons("auto");

			// Select last item in listBox_pointers
			listBox_pointers.SelectedIndex = listBox_pointers.Items.Count - 1;

			// Return
			return;
		}

		private void button_moveAll_Click(object sender, EventArgs e)
		{
			// Disable buttons
			ToggleButtons("none");

			// If listBox_files.Length == 0: MsgBox & return
			if (listBox_files.Items.Count == 0)
			{
				MessageBox.Show("No tracks found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// Move all Tracks
			foreach (var name in AuH.Names)
			{
				var trackData = AuH.GetTrack(name: name);
				CuH.MoveDataToDevice(trackData);
			}

			// Call GUI-Updaters
			UpdatePointers();
			UpdateTracks();
			UpdateWaveViewers();

			// Enable buttons
			ToggleButtons("auto");

			// Select first item in listBox_files & first item in listBox_pointers
			listBox_files.SelectedIndex = 0;
			listBox_pointers.SelectedIndex = 0;

			// Return
			return;
		}

		private void button_clearPointers_Click(object sender, EventArgs e)
		{
			// Clear DataPointers & Update Pointers
			int count = CuH.ClearPointers();
			UpdatePointers();
			UpdateWaveViewers();

			// Show MsgBox with count of cleared DataPointers
			MessageBox.Show("Cleared " + count + " DataPointers!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

			// Return
			return;
		}

		private void button_clearFiles_Click(object sender, EventArgs e)
		{
			// Clear Tracks & Names & count
			int count = AuH.ClearTracks();

			// Update Tracks & WaveViewers
			UpdateTracks();
			UpdateWaveViewers();

			// Show MsgBox with count of cleared Tracks
			MessageBox.Show("Cleared " + count + " Tracks!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void button_resetContext_Click(object sender, EventArgs e)
		{
			// Clear & Dispose CuH & set CuH to new()
			CuH.ClearPointers();
			CuH.Dispose();
			var selectedItem = listBox_devices.SelectedItem?.ToString();
			if (selectedItem != null)
			{
				CuH = new(CuH.GetDeviceId(name: selectedItem));
			}
			else
			{
				CuH = new();
			}

			// CuH.FreeMemory()
			CuH.FreeUnusedMemory();

			// Update GUI
			ReloadDevices();
			UpdatePointers();
			UpdateTracks();
			UpdateWaveViewers();
			UpdateInitLabel();
		}

		

		private void button_export_Click(object sender, EventArgs e)
		{
			// Disable buttons
			ToggleButtons("none");

			// SFD (Filter: "Wave Files|*.wav", Title: "Export Track", DefaultExt: "wav", InitialDirectory: MyMusic)
			using var sfd = new SaveFileDialog
			{
				Filter = "Wave Files|*.wav",
				Title = "Export Track",
				DefaultExt = "wav",
				InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)
			};

			// If SFD OK: Export Track
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				// Get selected pointer data from listBox_pointers
				byte[] data = CuH.GetDataFromPointer((CUdeviceptr) listBox_pointers.SelectedItem!);

				// Add Wave Header if datas first 4 bytes are not "RIFF"
				if (data.Length > 12 && Encoding.ASCII.GetString(data, 0, 4) != "RIFF")
				{
					data = AuH.AddWaveHeader(data);
				}

				// Write data to file
				File.WriteAllBytes(sfd.FileName, data);
			}

			// Enable buttons
			ToggleButtons("auto");
		}

		private void button_stretch_Click(object sender, EventArgs e)
		{

		}

		private void button_scanBpm_Click(object sender, EventArgs e)
		{
			// Disable buttons
			ToggleButtons("none");

			// If both waveViewers have no Stream: MsgBox & return
			if (waveViewer_track.WaveStream == null && waveViewer_pointer.WaveStream == null)
			{
				MessageBox.Show("No waveform loaded!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// New byte[] data
			byte[]? data = null;
			// If waveViewer_track has stream: Scan BPM & set label_fileBpm.Text
			if (waveViewer_track.WaveStream != null)
			{
				// Get data from waveViewer_track
				data = AuH.GetTrack("", -1, waveViewer_track.WaveStream);

				// Abort if data is null
				if (data == null)
				{
					MessageBox.Show("Invalid data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				// Get sampleRate, channels & bitsPerSample from AudioHandling
				int sampleRate = AuH.GetSampleRateFromHeader(data);
				int channels = AuH.GetChannelsFromHeader(data);
				int bitsPerSample = AuH.GetBitsPerSampleFromHeader(data);

				// Set label_track.Text to "File: " + sampleRate + "Hz, " + channels + " Channels, " + bitsPerSample + " Bits"
				label_file.Text = "Pointer: " + sampleRate + "Hz, " + channels + " Channels, " + bitsPerSample + " Bits";

				// Get BPM from AudioHandling
				float bpm = AuH.ScanBpm(data, (float) numericUpDown_threshold.Value, (float) numericUpDown_cutoff.Value, 0);

				// Set label_pointerBpm.Text to "BPM: " + bpm
				label_fileBpm.Text = "BPM: " + bpm;
			}

			// If waveViewer_pointer has stream: Scan BPM & set label_pointerBpm.Text
			if (waveViewer_pointer.WaveStream != null)
			{
				// Get data from waveViewer_pointer
				data = CuH.GetDataFromPointer((CUdeviceptr) listBox_pointers.SelectedItem!);

				// Abort if data is null
				if (data == null)
				{
					MessageBox.Show("Invalid data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				// Get sampleRate, channels & bitsPerSample from AudioHandling
				int sampleRate = AuH.GetSampleRateFromHeader(data);
				int channels = AuH.GetChannelsFromHeader(data);
				int bitsPerSample = AuH.GetBitsPerSampleFromHeader(data);

				// Set label_track.Text to "File: " + sampleRate + "Hz, " + channels + " Channels, " + bitsPerSample + " Bits"
				label_pointer.Text = "Pointer: " + sampleRate + "Hz, " + channels + " Channels, " + bitsPerSample + " Bits";

				// Get BPM from AudioHandling
				float bpm = AuH.ScanBpm(data, (float) numericUpDown_threshold.Value, (float) numericUpDown_cutoff.Value, 0);

				// Set label_pointerBpm.Text to "BPM: " + bpm
				label_pointerBpm.Text = "BPM: " + bpm;
			}

			// Enable buttons
			ToggleButtons("auto");
		}
	}
}
