using ConsoleWindowsSystem.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleWindowsSystem
{
	public class ConsoleWindow : Window
	{

		protected ConsoleTerminal terminal = new(80, 25, "cmd.exe");
		protected string output = "";
		public override WindowFlags flags => WindowFlags.Closable;
		protected int scroll = 0;
		public ConsoleWindow(SystemInfo system)
		{
			width = 120;height = 25;
			x = 50;
			// terminal.Write("cd ..");
			terminal.Write("dir");
			Thread.Sleep(100);
			terminal.Write("dir");
			Thread.Sleep(100);
			output += terminal.Read();
		}

		protected override void DrawSurface(Mouse.POINT mouse_pos, int mouse_button, GraphicsDrawer graphics)
		{
			graphics.FillBox(new System.Drawing.Point(x+1, y+1), new System.Drawing.Point(width-1, height-1), ' ');
			output += terminal.Read();
			int n = -scroll;
			foreach (var line in output.Split("\r\n"))
			{
				if ( n + 1 <= 0 || n + 1 >= height)
				{
					n++;
					continue;
				}
				graphics.Text(x + 1, y + n + 1, line);
				n++;
			}
			graphics.Line(new System.Drawing.Point(x + 118, y + 1), new System.Drawing.Point(x + 118, y + 24));
			graphics.Point(x + 119, y + 1 + scroll, '#');
			if (mouse_button == 0 && mouse_pos.X == x + 119 && mouse_pos.Y >= y+1 && mouse_pos.Y <= y+height-1)
			{
				scroll = mouse_pos.Y - y - 1;
			}
        }
	}
}
