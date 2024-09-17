using ConsoleWindowsSystem.Engine;
using System.Drawing;
using static ConsoleWindowsSystem.Engine.Mouse;

namespace ConsoleWindowsSystem.Windows
{
    [Flags]
    public enum WindowFlags
    {
        None = 0,
        Resizable = 1,
        NoMovable = 2,
        Closable = 4,
    }
    public abstract class BaseWindow
    {
        public int x;
        public int y;
        public bool grap { get; set; } = false;
        public int width { get; set; }
        public int height { get; set; }
        public virtual WindowFlags flags { get; } = WindowFlags.None;
        protected abstract void DrawSurface(POINT mouse_pos, int mouse_button, GraphicsDrawer graphics);
        public virtual void Draw(POINT mouse_pos, int mouse_button, GraphicsDrawer graphics)
        {
            DrawBorder(mouse_pos, mouse_button, graphics);
            DrawSurface(mouse_pos, mouse_button, graphics);
        }
        public abstract void DrawBorder(POINT mouse_pos, int mouse_button, GraphicsDrawer graphics);
    }
}
