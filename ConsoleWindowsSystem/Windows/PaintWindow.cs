using ConsoleWindowsSystem.Engine;
using PInvoke;
using System.Drawing;

namespace ConsoleWindowsSystem.Windows
{
	public class PaintWindow : Window
	{
		public override WindowFlags flags => WindowFlags.Closable;
		public char[,] buffer = new char[70, 30];
		public char color = 'c';
		public int lx = -1;
		public int ly = -1;
		public PaintWindow() {
			for (int i = 0; i < 70; i++)
			{
				for (int j = 0; j < 30; j++)
				{
					buffer[i, j] = '.';
				}
			}
			width = 71; height = 32;
		}
		public void line(Point p1, Point p2)
		{
			int dx = Math.Abs(p2.X - p1.X);
			int dy = Math.Abs(p2.Y - p1.Y);

			int sx = (p1.X < p2.X) ? 1 : -1;
			int sy = (p1.Y < p2.Y) ? 1 : -1;

			int err = dx - dy;

			Point p = p1;

			while (true)
			{
				buffer[p.X, p.Y] = color;

				if (p.X == p2.X && p.Y == p2.Y) break;

				int e2 = 2 * err;
				if (e2 > -dy)
				{
					err -= dy;
					p.X += sx;
				}
				if (e2 < dx)
				{
					err += dx;
					p.Y += sy;
				}
			}
		}
		protected override void DrawSurface(Mouse.POINT mouse_pos, int mouse_button, GraphicsDrawer graphics)
		{
			for (int i = 0; i < 70; i++)
			{
				for (int j = 0; j < 30; j++)
				{
					graphics.Point(x + 1 + i, y + 1 + j, buffer[i, j]);
				}
			}
			string colors = "1234567890-=_+qwertyuiopasdfghjklzxcvbnm,.?@#$%^&*[];'\\,./ ";
			graphics.Text(x + 1, y - 1 + height, colors);
			graphics.Point(x + 1 + colors.Length + 1, y - 1 + height, '[');
			graphics.Point(x + 1 + colors.Length + 2, y - 1 + height, color);
			graphics.Point(x + 1 + colors.Length + 3, y - 1 + height, ']');
			if (mouse_button == 0)
			{
				if (mouse_pos.X >= x + 1 && mouse_pos.X <= x + 1 + colors.Length && mouse_pos.Y == y - 1 + height)
				{
					color = colors[mouse_pos.X - (x + 1)];
				}
				if (mouse_pos.X >= x + 1 && mouse_pos.X < x + width && mouse_pos.Y >= y + 1 && mouse_pos.Y <= y - 2 + height)
				{
					if (lx != -1 || ly != -1)
					{
						line(new Point(lx, ly), new Point(mouse_pos.X - (x + 1), mouse_pos.Y - (y + 1)));
					}
					lx = mouse_pos.X - (x + 1);
					ly = mouse_pos.Y - (y + 1);
					buffer[mouse_pos.X - (x + 1), mouse_pos.Y - (y + 1)] = color;
				}
			}
			else
			{
				lx = -1; ly = -1;
			}
		}
	}
}
