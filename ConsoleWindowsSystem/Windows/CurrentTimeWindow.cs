using ConsoleWindowsSystem.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleWindowsSystem.Windows
{
    public class CurrentTimeWindow : Window
    {

        public override WindowFlags flags => WindowFlags.NoMovable;

        public CurrentTimeWindow()
        {
            width = 20; height = 3;
        }

        protected override void DrawSurface(Mouse.POINT mouse_pos, int mouse_button, GraphicsDrawer graphics)
        {
            DateTime dt = DateTime.Now;
            graphics.Text(x + 5, y + 1, $"{dt.Hour}:{dt.Minute}.{dt.Second}");
            graphics.Text(x + 5, y + 2, $"{dt.Day}.{dt.Month}.{dt.Year}");
        }
    }
}
