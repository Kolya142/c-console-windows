using ConsoleWindowsSystem.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleWindowsSystem
{
	class SpawnMenuWindow : Window
	{
		Button window1_spawn = new Button();
		Button window2_spawn = new Button();
		Button window3_spawn = new Button();
		Button window4_spawn = new Button();
		Button error_spawn = new Button();
		SystemInfo system;
		public SpawnMenuWindow(SystemInfo system) {
			x = 10; y = 0;
			string message = null;
			width = 20; height = 50;
			this.system = system;
			window1_spawn.on_click = () => { system.windows.Add(new SimpleWindow(system, 20, 20)); };
			window2_spawn.on_click = () => { system.windows.Add(new ConsoleWindow(system)); };
			window3_spawn.on_click = () => { system.windows.Add(new SystemInfoWindow(system)); };
			window4_spawn.on_click = () => { system.windows.Add(new TunnelWindow(system, 30, 20)); };
			error_spawn.on_click = () => { throw new DivideByZeroException(); };
		}
		protected override void DrawSurface(Mouse.POINT mouse_pos, int mouse_button, GraphicsDrawer graphics)
		{
			graphics.Text(x + 1, y + 1, "SimpleWindow");
			graphics.Text(x + 1, y + 2, "ConsoleWindow");
			graphics.Text(x + 1, y + 3, "SystemInfoWindow");
			graphics.Text(x + 1, y + 4, "TunnelWindow");
			graphics.Text(x + 1, y + 5, "Runtime Error");
			window1_spawn.update(mouse_button, mouse_pos, x + 1, y + 1, 12, 1);
			window2_spawn.update(mouse_button, mouse_pos, x + 1, y + 2, 13, 1);
			window3_spawn.update(mouse_button, mouse_pos, x + 1, y + 3, 16, 1);
			window4_spawn.update(mouse_button, mouse_pos, x + 1, y + 4, 12, 1);
			error_spawn.update(mouse_button, mouse_pos, x + 1, y + 5, 13, 1);
		}
	}
}
