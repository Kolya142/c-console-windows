using ConsoleWindowsSystem.Engine;

namespace ConsoleWindowsSystem
{
	public class SystemInfoWindow : Window
	{
		SystemInfo system;
		Button button = new Button();
		public override WindowFlags flags => WindowFlags.Closable;

		protected int count = 0;

		public SystemInfoWindow(SystemInfo system)
		{
			width = 20;height = 10;
			button.on_click = () => { count++; };
			this.system = system;
		}

		protected override void DrawSurface(Mouse.POINT mouse_pos, int mouse_button, GraphicsDrawer graphics)
		{
			graphics.Text(x + 1, y + 1, mouse_pos.ToString());
			graphics.Text(x + 1, y + 2, mouse_button.ToString());
			graphics.Text(x + 1, y + 3, system.windows.Count.ToString());
			graphics.Text(x + 1, y + 4, $"{x},{y}");
			graphics.Text(x + 6, y + 5, count.ToString());
			if (button.update(mouse_button, mouse_pos, x + 1, y + 5, 5, 1))
			{
				graphics.Text(x + 1, y + 5, "[XXX]");
			}
			else
			{
				graphics.Text(x + 1, y + 5, "[   ]");
			}
		}
	}
}
