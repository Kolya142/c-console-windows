using ConsoleWindowsSystem;
using ConsoleWindowsSystem.Engine;
using ConsoleWindowsSystem.Windows;
using System.Drawing;
using System.Reflection;
using System.IO;
using System;


public class LogEntry
{
	public int time { get; set; }
	public string text { get; set; }
}

class Program {
	static List<BaseWindow> GetWindows()
	{
		string[] files = Directory.GetFiles("dlls");
		List<BaseWindow> windows = new List<BaseWindow>();
		foreach (string file in files)
		{
			if (!file.EndsWith(".dll")) {
				continue;
			}
			Assembly asm = Assembly.LoadFrom(file);
			Type? t = asm.GetType("MyWindow.Main");
			Console.WriteLine(asm.GetTypes());
			if (t != null)
			{
				object? obj = t.GetMethod("Create", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { });
				Console.Write(obj);
				if (obj != null)
				{
					windows.Add((BaseWindow)obj);
					Console.WriteLine($"DllLoader Ok: Succesfly create object Main from {file}");
				}
				else
				{
					Console.WriteLine($"DllLoader ERROR: failed to create object Main from {file}");
				}
			}
			else
			{
				Console.WriteLine($"DllLoader ERROR: failed to load type Main from {file}");
			}
		}
		return windows;
	}
	static void Main(string[] args) {
		List<LogEntry> log = new();
		Graphics graphics = new Graphics();
		Mouse mouse = new Mouse();
		SystemInfo system = new();
		system.windows = new()
		{
			new SpawnMenuWindow(system),
			new CurrentTimeWindow(),
		};
		system.windows.AddRange(GetWindows());
		//return;
		int mouse_button = -1;
		BaseWindow? grapped = null;
		bool size = false;
		int grap_x = 0;
		int grap_y = 0;
		int t = 0;
		while (true)
		{
			if (Console.WindowWidth*Console.WindowHeight != graphics.Drawer.buffer.Length)
			{
				graphics.Drawer.size = new Point(Console.WindowWidth, Console.WindowHeight);
				graphics.Clear();
			}
			t++;
			int last_button = mouse_button;
			int but = mouse.GetMouseButtons();
			if (but != -2)
			{
				mouse_button = but;
			}
			graphics.Clear();
			var mouse_pos = mouse.GetMousePos();
			int len = system.windows.Count;
			try
			{
				foreach (BaseWindow window in system.windows)
				{
					window.Draw(mouse_pos, mouse_button, graphics.Drawer);
					if ((window.flags & WindowFlags.Resizable) != 0)
					{
						graphics.Drawer.Point(window.width + window.x, window.height + window.y, '#');
					}
					if ((window.flags & WindowFlags.Closable) != 0)
					{
						graphics.Drawer.Point(window.width + window.x, window.y, 'X');
						if (mouse_pos.X == window.x + window.width && mouse_pos.Y == window.y && mouse_button == 0 && last_button == -1)
						{
							system.windows.Remove(window);
							break;
						}
					}
					if (grapped == null || grapped == window)
					{
						if (mouse_button == 0)
						{
							if ((window.flags & WindowFlags.Resizable) != 0 && !size && !window.grap && mouse_pos.X == window.x + window.width && mouse_pos.Y == window.y + window.height)
							{
								grapped = window;
								size = true;
								window.grap = true;
								grap_x = mouse_pos.X;
								grap_y = mouse_pos.Y;
							}
							else if (!window.grap && (window.flags & WindowFlags.NoMovable) == 0)
							{
								if ((mouse_pos.X >= window.x && mouse_pos.X <= window.x + window.width && mouse_pos.Y == window.y) ||
									(mouse_pos.X >= window.x && mouse_pos.X <= window.x + window.width - 1 && mouse_pos.Y == window.y + window.height) ||
									(mouse_pos.Y >= window.y && mouse_pos.Y <= window.y + window.height && mouse_pos.X == window.x) ||
									(mouse_pos.Y >= window.y && mouse_pos.Y <= window.y + window.height - 1 && mouse_pos.X == window.x + window.width))
								{
									grapped = window;
									window.grap = true;
									grap_x = window.x - mouse_pos.X;
									grap_y = window.y - mouse_pos.Y;
								}
							}
						}
						else
						{
							grapped = null;
							size = false;
							window.grap = false;
						}
						if (window.grap)
						{
							if (!size)
							{
								window.x = mouse_pos.X + grap_x; window.y = mouse_pos.Y + grap_y;
							}
							else
							{
								window.width = mouse_pos.X - window.x; window.height = mouse_pos.Y - window.y;
							}
						}
						if (len != system.windows.Count)
						{
							break;
						}
					}
				}
				int y = 0;
				var removes = new List<LogEntry>();
				foreach (var logentry in log)
				{
                    foreach (var text in logentry.text.Split(Environment.NewLine))
					{
						graphics.Drawer.Text(Console.WindowWidth - text.Length, y, text);
						y++;
					}
					if (t - logentry.time > 100)
					{
						removes.Add(logentry);
					}
				}
				foreach (var logentry in removes)
				{
					log.Remove(logentry);
				}
				graphics.Drawer.Point(mouse_pos.X, mouse_pos.Y, 'M');
				// graphics.Drawer.Text(10, 20, $"Mouse: {point};Window: {Console.WindowLeft},{Console.WindowTop};MouseButton: {button}");
				graphics.Draw();
			}
			catch (Exception e)
			{
				log.Add(new LogEntry() { text = e.Message + e.StackTrace, time = t });
			}
			Thread.Sleep(20);
		}
	}
}