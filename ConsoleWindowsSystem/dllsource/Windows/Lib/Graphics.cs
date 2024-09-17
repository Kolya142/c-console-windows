

namespace ConsoleWindowsSystem.Engine
{
	public class Graphics
	{
		public GraphicsDrawer Drawer { get; set; }
		public Graphics() { 
			Drawer = new GraphicsDrawer();
		}
		public void Clear()
		{
			Drawer.Clear();
		}
		public void Draw()
		{
			Console.SetCursorPosition(0, 0);
			Console.Out.Write(new string(Drawer.buffer));
		}
	}
}
