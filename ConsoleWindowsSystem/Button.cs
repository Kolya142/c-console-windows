using ConsoleWindowsSystem.Engine;

namespace ConsoleWindowsSystem
{
	public class Button
	{
		bool last_button = false;
		public Action on_click;
		public bool update(int mouse_button, Mouse.POINT mouse, int x, int y, int w, int h)
		{
			int mx = mouse.X; int my = mouse.Y;
			if (!last_button && mouse_button == 0 && mx >= x && mx < x+w && my >= y && my < y+h) 
			{
				on_click();
			}
			last_button = mouse_button == 0;
			return mx >= x && mx < x + w && my >= y && my < y + h && last_button;
		}
	}
}
