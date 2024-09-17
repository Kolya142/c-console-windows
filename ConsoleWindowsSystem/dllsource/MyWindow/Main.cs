using ConsoleWindowsSystem.Engine;
using ConsoleWindowsSystem.Windows;
using ConsoleWindowsSystem;
namespace MyWindow;
public class Main : Window
{
    public override WindowFlags flags => WindowFlags.Resizable | WindowFlags.Closable;
    public Main()
    {
        width = 50;
        height = 30;
        this.x = 10;
        this.y = 15;
    }

    public static Main Create()
    {
        Main window = new Main();
        return window;
    }
    protected override void DrawSurface(Mouse.POINT mouse_pos, int mouse_button, GraphicsDrawer graphics)
    {
        graphics.Text(2+x, 2+y, "1231");
    }
}