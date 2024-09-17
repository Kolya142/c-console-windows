using ConsoleWindowsSystem.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleWindowsSystem.Windows
{
	public abstract class WindowWithBuffer : Window
	{
		private Graphics _graphics = new Graphics();
		public void init()
		{
			_graphics.Drawer.size.X = width - 1;
			_graphics.Drawer.size.Y = height - 1;
		}
		public override void Draw(Mouse.POINT mouse_pos, int mouse_button, GraphicsDrawer graphics)
		{
			_graphics.Drawer.buffer = new char[(width - 1) * (height - 1)];
			for (int i = 0; i < width - 1; i++)
			{
				for (int j = 0; j < height - 1; j++)
				{
					_graphics.Drawer.buffer[i + j * (width - 1)] = '.';
				}
			}
			DrawBorder(mouse_pos, mouse_button, graphics);
			DrawSurface(mouse_pos, mouse_button, _graphics.Drawer);
			for (int i = 0; i < width - 1; i++)
			{
				for (int j = 0; j < height - 1; j++)
				{
					graphics.Point(i+x+1, j+y+1, _graphics.Drawer.buffer[i + j * (width - 1)]);
				}
			}
		}
		protected override void DrawSurface(Mouse.POINT mouse_pos, int mouse_button, GraphicsDrawer graphics)
		{
			throw new NotImplementedException();
		}
	}
}
