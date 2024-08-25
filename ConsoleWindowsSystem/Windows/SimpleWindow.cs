using ConsoleWindowsSystem.Engine;

namespace ConsoleWindowsSystem.Windows
{
    public class SimpleWindow : Window
    {
        public override WindowFlags flags => WindowFlags.Resizable | WindowFlags.Closable;
        public SimpleWindow(SystemInfo system, int x, int y)
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
            for (int i = 0; i < width - 1; i++)
            {
                for (int j = 0; j < height - 1; j++)
                {
                    float px = i;
                    float py = j;
                    px /= w;
                    py /= h;
                    float color = (MathF.Abs(px - .5f) + MathF.Abs(py - .5f)) * 2;
                    if (color > 1)
                    {
                        color = 1;
                    }
                    graphics.Point(i + x + 1, j + y + 1, gradient[(int)(color * (gradient.Length - 1))]);
                }
            }
        }
    }
}
