using ManagedCuda;
using ManagedCuda.BasicTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WAV_CUDA
{
	public class CudaHandling
	{
		// Attributes
		public PrimaryContext? Context;
		public CUdevice? Device;
		public List<CUdeviceptr> DataPointers = [];
		public Dictionary<CUdeviceptr, int> DataLengths = [];

		// Constructor
		public CudaHandling(int device = -1, string name = "")
		{
			// If device is -1 & name is "", Initialize strongest device
			Initialize();

			// If device is -1 & name is not "", Initialize device by name
			Initialize(name: name);

			// If device is not -1 & name == "", Initialize device by index
			Initialize(id: device);

			// Else: Context = null
			Context = null;
		}

		// Methods
		public void Initialize(int id = -1, string name = "")
		{
			// If id is -1 & name is "", Initialize strongest device
			if (id == -1 && string.IsNullOrEmpty(name))
			{
				id = CudaContext.GetMaxGflopsDeviceId();
			}

			// If id is -1 & name is not "", Initialize device by name
			if (id == -1 && !string.IsNullOrEmpty(name))
			{
				id = GetDeviceId(name: name);
			}

			// If id is not -1 & name == "", Initialize device by index
			if (id != -1 && string.IsNullOrEmpty(name))
			{
				Context = new PrimaryContext(id);
				Device = GetDevice(id: id);
			}

			// Else: Context = null
			if (Context == null)
			{
				Context = new PrimaryContext(id);
				Device = GetDevice(id: id);
			}
		}

		public void Dispose()
		{
			// Dispose & free all DataPointers & Context
			if (DataPointers != null)
			{
				foreach (var pointer in DataPointers)
				{
					Context?.FreeMemory(pointer);
				}
				DataPointers.Clear();
			}

			Context?.Dispose();
			Context = null;
			Device = null;
		}

		public List<string> GetDeviceNames()
		{
			// Get all devices & return their names
			List<string> deviceNames = [];
			int deviceCount = PrimaryContext.GetDeviceCount();

			for (int i = 0; i < deviceCount; i++)
			{
				deviceNames.Add(PrimaryContext.GetDeviceName(i));
			}

			// Failed: return empty list
			return deviceNames;
		}

		public CUdevice? GetDevice(int id = -1, string name = "")
		{
			// If id is -1 & name is "", return current device
			if (id == -1 && string.IsNullOrEmpty(name))
			{
				return Context?.Device;
			}

			// If id is -1 & name is not "", return device by name
			if (id == -1 && !string.IsNullOrEmpty(name))
			{
				int deviceId = GetDeviceId(name: name);
				if (deviceId != -1)
				{
					return PrimaryContext.GetCUdevice(deviceId);
				}
			}

			// If id is not -1 & name == "", return device by index
			if (id != -1 && string.IsNullOrEmpty(name))
			{
				return PrimaryContext.GetCUdevice(id);
			}

			// Else: return null
			return null;
		}

		public int GetDeviceId(CUdevice? device = null, string name = "")
		{
			// If device is null & name is "", return current device id
			if (device == null && string.IsNullOrEmpty(name))
			{
				return Context?.DeviceId ?? -1;
			}

			// If device is null & name is not "", return device id by name
			if (device == null && !string.IsNullOrEmpty(name))
			{
				for (int i = 0; i < PrimaryContext.GetDeviceCount(); i++)
				{
					if (PrimaryContext.GetDeviceName(i) == name)
					{
						return i;
					}
				}
			}

			// If device is not null & name == "", return device id by device
			if (device != null && string.IsNullOrEmpty(name))
			{
				for (int i = 0; i < PrimaryContext.GetDeviceCount(); i++)
				{
					if (PrimaryContext.GetCUdevice(i).Pointer == device.Value.Pointer)
					{
						return i;
					}
				}
			}

			// Else: -1
			return -1;
		}

		public string GetDeviceName(CUdevice? device = null, int id = -1)
		{
			// If device is null & id is -1, return current device name
			if (device == null && id == -1)
			{
				return Context != null ? PrimaryContext.GetDeviceName(Context.DeviceId) : "";
			}

			// If device is null & id is not -1, return device name by id
			if (device == null && id != -1)
			{
				return PrimaryContext.GetDeviceName(id);
			}

			// If device is not null & id == -1, return device name by device
			if (device != null && id == -1)
			{
				return PrimaryContext.GetDeviceName(device.Value.Pointer);
			}

			// Else: ""
			return "";
		}

		public CUdeviceptr MoveDataToDevice(byte[]? data = null)
		{
			// Abort if data is null or length is 0
			if (data == null)
			{
				throw new ArgumentNullException("Data must not be null.");
			}

			if (data.Length <= 0)
			{
				throw new ArgumentException("Data length must be greater than zero.");
			}

			// Set current context & allocate memory & copy data to device
			Context?.SetCurrent();
			CUdeviceptr pointer = Context?.AllocateMemory(data.Length) ?? default;
			Context?.CopyToDevice(pointer, data);

			// Add pointer to DataPointers if not default or IntPtr.Zero
			if (pointer.Pointer != IntPtr.Zero && pointer != default)
			{
				DataPointers.Add(pointer);
				DataLengths[pointer] = data.Length; // Store the length of the data
			}

			// Return pointer
			return pointer;
		}

		public byte[] GetDataFromPointer(CUdeviceptr pointer, int length = 0)
		{
			// Abort if pointer is invalid or length is 0
			if (pointer.Pointer == IntPtr.Zero)
			{
				throw new ArgumentException("Invalid CUDA pointer.");
			}

			// If length is not provided, get it from the dictionary
			if (length <= 0 && DataLengths.ContainsKey(pointer))
			{
				length = DataLengths[pointer];
			}

			if (length <= 0)
			{
				throw new ArgumentException("Length must be greater than zero.");
			}

			// Set current context & copy data from device to host
			Context?.SetCurrent();
			byte[] data = new byte[length];
			Context?.CopyToHost(data, pointer);

			// Return data
			return data;
		}

		public int ClearPointers()
		{
			// New counter & set current context
			int count = 0;
			Context?.SetCurrent();

			// Free all DataPointers
			foreach (var pointer in DataPointers)
			{
				Context?.FreeMemory(pointer);
				count++;
			}

			// Clear DataPointers
			DataPointers = [];

			// Return count
			return count;
		}

		public void FreeUnusedMemory()
		{
			// Set current context
			Context?.SetCurrent();

			// Free all empty DataPointers
			foreach (var pointer in DataPointers)
			{
				if (DataLengths[pointer] <= 0)
				{
					// Free memory of pointer
					Context?.FreeMemory(pointer);

					// Remove pointer from DataPointers
					DataPointers = DataPointers.Where(pointer => DataLengths[pointer] > 0).ToList();

					// Remove pointer from DataLengths
					DataLengths.Remove(pointer);
				}
			}

			// Call garbage collector & free memory
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
	}
}
