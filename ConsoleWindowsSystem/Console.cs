using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

public class ConsoleTerminal
{
	private IntPtr _hReadPipe;
	private IntPtr _hWritePipe;
	private Process _process;
	private StringBuilder _outputBuffer;
	private Thread _outputThread;

	public ConsoleTerminal(uint width, uint height, string console = "cmd.exe")
	{
		_outputBuffer = new StringBuilder();
		InitializePipes();
		StartConsoleProcess(width, height, console);
	}

	private void InitializePipes()
	{
		// Create pipes for communication
		if (!CreatePipe(out _hReadPipe, out _hWritePipe, IntPtr.Zero, 0))
		{
			throw new Exception("Failed to create pipes");
		}
	}

	private void StartConsoleProcess(uint width, uint height, string console)
	{
		var processStartInfo = new ProcessStartInfo
		{
			FileName = console,
			RedirectStandardInput = true,
			RedirectStandardOutput = true,
			UseShellExecute = false,
			CreateNoWindow = true
		};

		_process = new Process { StartInfo = processStartInfo };
		_process.OutputDataReceived += Process_OutputDataReceived;
		_process.Start();
		_process.BeginOutputReadLine();

		// Start a thread to read the output asynchronously
		_outputThread = new Thread(ReadOutput) { IsBackground = true };
		_outputThread.Start();
	}

	private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
	{
		if (!string.IsNullOrEmpty(e.Data))
		{
			lock (_outputBuffer)
			{
				_outputBuffer.AppendLine(e.Data);
			}
		}
	}

	private void ReadOutput()
	{
		byte[] buffer = new byte[1024];
		uint bytesRead;
		while (true)
		{
			if (ReadFile(_hReadPipe, buffer, (uint)buffer.Length, out bytesRead, IntPtr.Zero) && bytesRead > 0)
			{
				var output = Encoding.Default.GetString(buffer, 0, (int)bytesRead);
				lock (_outputBuffer)
				{
					_outputBuffer.Append(output);
				}
			}
			Thread.Sleep(50); // Prevents tight loop, reducing CPU usage
		}
	}

	public string Read()
	{
		lock (_outputBuffer)
		{
			string output = _outputBuffer.ToString();
			_outputBuffer.Clear();
			return output;
		}
	}

	public void Write(string input)
	{
		if (_process != null && !_process.HasExited)
		{
			using (var streamWriter = _process.StandardInput)
			{
				streamWriter.WriteLine(input);
			}
		}
	}

	~ConsoleTerminal()
	{
		CloseHandle(_hReadPipe);
		CloseHandle(_hWritePipe);
		_process?.Close();
		_outputThread?.Join();
	}
	// PInvoke declarations
	[DllImport("kernel32.dll", SetLastError = true)]
	static extern bool CreatePipe(out IntPtr hReadPipe, out IntPtr hWritePipe, IntPtr lpPipeAttributes, uint nSize);

	[DllImport("kernel32.dll", SetLastError = true)]
	static extern bool ReadFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, IntPtr lpOverlapped);

	[DllImport("kernel32.dll", SetLastError = true)]
	static extern bool CloseHandle(IntPtr hObject);
}