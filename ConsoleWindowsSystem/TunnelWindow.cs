

using ConsoleWindowsSystem.Engine;

namespace ConsoleWindowsSystem
{
	public class TunnelWindow : Window
	{
		public override WindowFlags flags => WindowFlags.Resizable | WindowFlags.Closable;
		public int t;
		public TunnelWindow(SystemInfo system, int x, int y)
		{
			width = 50;
			height = 30;
			this.x = x;
			this.y = y;
		}

		protected override void DrawSurface(Mouse.POINT mouse_pos, int mouse_button, GraphicsDrawer graphics)
		{
			char[] gradient = "$@B%8&WM#*oahkbdpqwmZO0QLCJUYXzcvunxrjft/\\|()1{}[]?-_+~<>i!lI;:,\"^`'. ".ToCharArray();
			float w = width - 1;
			float h = height - 1;
			t++;
			for (int i = 0; i < width - 1; i++)
			{
				for (int j = 0; j < height - 1; j++)
				{
					float px = i / w - 0.5f;
					float py = j / h - 0.5f;

					// Convert to polar coordinates
					float angle = MathF.Atan2(py, px);
					float radius = MathF.Sqrt(px * px + py * py);

					// Tunnel effect transformation
					float tunnel = MathF.Sin(radius * 10.0f - t * 0.05f) * 0.5f + 0.5f;

					// Map tunnel effect to character gradient
					float color = tunnel;
					if (color > 1)
					{
						color = 1;
					}
					if (color < 0)
					{
						color = 0;
					}

					int index = (int)(color * (gradient.Length - 1));
					graphics.Point(i + x + 1, j + y + 1, gradient[index]);
				}
			}
		}
	}
}
