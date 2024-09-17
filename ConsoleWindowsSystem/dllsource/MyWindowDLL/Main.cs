using ConsoleWindowsSystem.Engine;
using ConsoleWindowsSystem.Windows;
using ConsoleWindowsSystem;
using System;

namespace MyWindow;
public class Main : Window
{
    public override WindowFlags flags => WindowFlags.Closable;
    
    private int textX;
    private int textY;
    private int directionX = 1;
    private int directionY = 1;
    private string movingText = "Look at me!";

    public Main()
    {
        width = 30;
        height = 10;  // Сделаем окно немного меньше
        this.x = 20;
        this.y = 10;
        textX = 2;
        textY = 2;
    }

    public static Main Create()
    {
        Main window = new Main();
        return window;
    }

    protected override void DrawSurface(Mouse.POINT mouse_pos, int mouse_button, GraphicsDrawer graphics)
    {
        graphics.Text(x, y, "MyLittleWindowDLL");
        // Очищаем внутреннюю часть окна
        for (int i = 0; i < height; i++)
        {
            graphics.Text(x + 1, y + 1 + i, new string(' ', width - 2)); // Заполняем пустым пространством
        }

        // Двигаем текст в пределах окна
        graphics.Text(textX + x, textY + y, movingText);

        // Обновляем координаты для анимации
        UpdateTextPosition();
    }

    private void UpdateTextPosition()
    {
        // Меняем направление, если текст достигает края окна
        if (textX + movingText.Length >= width - 1 || textX <= 1)
        {
            directionX *= -1;
        }
        if (textY >= height - 2 || textY <= 1)
        {
            directionY *= -1;
        }

        // Изменяем позицию текста
        textX += directionX;
        textY += directionY;
    }
}
