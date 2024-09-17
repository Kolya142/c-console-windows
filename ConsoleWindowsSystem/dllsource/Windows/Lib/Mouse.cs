using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ConsoleWindowsSystem.Engine
{
	public class Mouse
	{
		// Define the POINT structure (can replace with System.Drawing.Point for integration)
		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int X;
			public int Y;
			public override string ToString()
			{
				return $"POINT({X},{Y})";
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct COORD
		{
			public short X;
			public short Y;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct CONSOLE_FONT_INFO
		{
			public int nFont;
			public COORD dwFontSize;
		}

		// Define INPUT_RECORD and MOUSE_EVENT_RECORD for mouse button detection
		[StructLayout(LayoutKind.Explicit)]
		public struct INPUT_RECORD
		{
			[FieldOffset(0)] public ushort EventType;
			[FieldOffset(4)] public MOUSE_EVENT_RECORD MouseEvent;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MOUSE_EVENT_RECORD
		{
			public COORD dwMousePosition;
			public uint dwButtonState;
			public uint dwControlKeyState;
			public uint dwEventFlags;
		}

		// Constants for console modes and handles
		private const int STD_INPUT_HANDLE = -10;
		private const int STD_OUTPUT_HANDLE = -11;
		private const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
		private const uint ENABLE_EXTENDED_FLAGS = 0x0080;
		private const uint ENABLE_MOUSE_INPUT = 0x0010;

		// Import required functions from Windows API
		[DllImport("user32.dll")]
		public static extern bool GetCursorPos(out POINT lpPoint);

		[DllImport("kernel32.dll")]
		public static extern IntPtr GetConsoleWindow();

		[DllImport("user32.dll")]
		public static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr GetStdHandle(int nStdHandle);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool GetCurrentConsoleFont(
			IntPtr hConsoleOutput,
			bool bMaximumWindow,
			out CONSOLE_FONT_INFO lpConsoleCurrentFont);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern COORD GetConsoleFontSize(IntPtr hConsoleOutput, int nFont);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool ReadConsoleInput(
			IntPtr hConsoleInput,
			[Out] INPUT_RECORD[] lpBuffer,
			uint nLength,
			out uint lpNumberOfEventsRead);
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool PeekConsoleInput(
			IntPtr hConsoleInput,
			[Out] INPUT_RECORD[] lpBuffer,
			uint nLength,
			out uint lpNumberOfEventsRead);

		// Fields
		private IntPtr consoleHandle;
		private int charWidth;
		private int charHeight;

		// Constructor
		public Mouse()
		{
			consoleHandle = GetConsoleWindow();
			InitializeCharacterSize();
			DisableMouseSelection();
			EnableMouseInput();
		}

		// Initialize character size dynamically
		private void InitializeCharacterSize()
		{
			IntPtr outputHandle = GetStdHandle(STD_OUTPUT_HANDLE);
			if (GetCurrentConsoleFont(outputHandle, false, out CONSOLE_FONT_INFO fontInfo))
			{
				COORD fontSize = GetConsoleFontSize(outputHandle, fontInfo.nFont);
				charWidth = fontSize.X;
				charHeight = fontSize.Y;
			}
			else
			{
				// Fallback to typical values if unable to retrieve dynamically
				charWidth = 8;
				charHeight = 16;
			}
		}

		// Disable mouse selection (Quick Edit mode)
		private void DisableMouseSelection()
		{
			IntPtr inputHandle = GetStdHandle(STD_INPUT_HANDLE);
			if (GetConsoleMode(inputHandle, out uint mode))
			{
				mode &= ~ENABLE_QUICK_EDIT_MODE; // Disable Quick Edit mode
				mode |= ENABLE_EXTENDED_FLAGS;   // Ensure extended flags are set
				SetConsoleMode(inputHandle, mode);
			}
		}

		// Enable mouse input mode
		private void EnableMouseInput()
		{
			IntPtr inputHandle = GetStdHandle(STD_INPUT_HANDLE);
			if (GetConsoleMode(inputHandle, out uint mode))
			{
				mode |= ENABLE_MOUSE_INPUT | ENABLE_EXTENDED_FLAGS;
				SetConsoleMode(inputHandle, mode);
			}
		}

		// Get mouse position relative to console
		public POINT GetMousePos()
		{
			if (GetCursorPos(out POINT mousePos))
			{
				if (ScreenToClient(consoleHandle, ref mousePos))
				{
					// Adjust to console character grid
					mousePos.X /= charWidth;
					mousePos.Y /= charHeight;
					return mousePos;
				}
			}
			return new POINT { X = -1, Y = -1 }; // Indicate error if unable to get position
		}

		// Get mouse button state
		public int GetMouseButtons()
		{
			IntPtr inputHandle = GetStdHandle(STD_INPUT_HANDLE);
			INPUT_RECORD[] inputRecord = new INPUT_RECORD[1];
			uint eventsRead;
			if (PeekConsoleInput(inputHandle, inputRecord, 1, out eventsRead) && eventsRead > 0)
			{
				if (ReadConsoleInput(inputHandle, inputRecord, 1, out eventsRead) && eventsRead > 0)
				{
					if (inputRecord[0].EventType == 0x0002) // Mouse event
					{
						uint buttonState = inputRecord[0].MouseEvent.dwButtonState;

						if (buttonState == 0x0001)
							return 0;
						if (buttonState == 0x0002)
							return 1;
						if (buttonState == 0x0004)
							return 2;
					}
				}
			}
			else
			{
				return -2;
			}
			return -1;
		}
	}
}
