

using System.Drawing;

namespace ConsoleWindowsSystem.Engine
{
	public class GraphicsDrawer
	{
		public char[] buffer;
		public Point size;
		public GraphicsDrawer() { 
			size = new Point(Console.WindowWidth, Console.WindowHeight);
			Clear();
		}
		public void Clear()
		{
			buffer = new char[size.Y * size.X];
			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = ' ';
			}
			/*
			for (int i = size.X; i < buffer.Length; i+=size.X) 
			{
				buffer[i] = '\n';
			}*/
		}
		public void Point(int x, int y, char c)
		{
			if (x < 0 || x >= size.X || y < 0 || y >= size.Y)
			{
				return;
			}
			buffer[y * size.X + x] = c;
		}
		public void Text(int x, int y, string s)
		{
			for (int i = x; i < x + s.Length; i++)
			{
				Point(i, y, s[i-x]);
			}
		}
		public void Line(Point p1, Point p2, char c = ':')
		{
			int dx = Math.Abs(p2.X - p1.X);
			int dy = Math.Abs(p2.Y - p1.Y);

			int sx = (p1.X < p2.X) ? 1 : -1;
			int sy = (p1.Y < p2.Y) ? 1 : -1;

			int err = dx - dy;

			Point p = p1;

			while (true)
			{
				Point(p.X, p.Y, c);

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

		public void Box(Point position, Point size)
		{
			var v1 = new Point(position.X, position.Y);
			var v2 = new Point(position.X + size.X, position.Y);
			var v3 = new Point(position.X, position.Y + size.Y);
			var v4 = new Point(position.X + size.X, position.Y + size.Y);
			Line(v1, v2, '═');
			Line(v1, v3, '║');
			Line(v2, v4, '║');
			Line(v3, v4, '═');
			Point(v1.X, v1.Y, '╔');
			Point(v2.X, v2.Y, '╗');
			Point(v3.X, v3.Y, '╚');
			Point(v4.X, v4.Y, '╝');
		}
		public void FillBox(Point position, Point size, char c = '.')
		{
			for (int i = 0; i < size.X; i++)
			{
				for (int j = 0; j < size.Y; j++)
				{
					Point(i+position.X, j+position.Y, c);
				}
			}
		}
	}
}
