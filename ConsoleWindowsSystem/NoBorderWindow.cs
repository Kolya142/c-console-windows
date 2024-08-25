
using ConsoleWindowsSystem.Engine;
using System.Drawing;
using static ConsoleWindowsSystem.Engine.Mouse;

namespace ConsoleWindowsSystem
{
	public abstract class NoBorderWindow : BaseWindow
	{
		public override void DrawBorder(POINT mouse_pos, int mouse_button, GraphicsDrawer graphics)
		{
			graphics.Line(new Point(x, y), new Point(x+width, y), '═');
			// graphics.Box(new Point(x, y), new Point(width, height));
			graphics.FillBox(new Point(x + 1, y + 1), new Point(width - 1, height - 1));
			// graphics.Text(x + 1, y, "Test Window");
		}
	}
}
