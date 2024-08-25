using ConsoleWindowsSystem.Engine;
using ConsoleWindowsSystem.Windows.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleWindowsSystem.Windows
{
	public class BrowserWindow : Window
	{
		Parser parser = new();
		public override WindowFlags flags => WindowFlags.Closable | WindowFlags.Resizable;
		protected int last_button = -1;
		protected string url = "/";
		protected Button refresh = new Button();
		public BrowserWindow(SystemInfo system)
		{
			width = 50;
			height = 30;
			parser.parser(Request("127.0.0.1", 91, "/"));
			refresh.on_click = () => { parser.parser(Request("127.0.0.1", 91, url)); };
		}
		protected string Request(string ip, int port, string url)
		{
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			socket.Connect(ip, port);
			socket.Send(Encoding.UTF8.GetBytes($"STTP {url}\nBrowser: Cmd"));
			byte[] buffer = new byte[4096];
			int len = socket.Receive(buffer);
			Array.Resize(ref buffer, len);
			socket.Close();
			return $"Text: Url<gh> [{ip}<gh>{port}{url}]\n" + Encoding.UTF8.GetString(buffer).Split("\r\n\r\n")[1];
		}
		protected override void DrawSurface(Mouse.POINT mouse_pos, int mouse_button, GraphicsDrawer graphics)
		{
			int j = 0;
            foreach (var command in parser.commands)
            {
				switch (command.type_)
				{
					case "Hr":
						for (int i = x + 1; i < x + width; i++)
						{
							graphics.Point(i, y + 1 + j, '═');
						}
						break;
					case "Text":
						graphics.Text(x + 1, y + 1 + j, ((CText)command).text);
						break;
					case "Button":
						graphics.Text(x + 1, y + 1 + j, ((CButton)command).text);
						if (last_button == -1 && mouse_button == 0 && mouse_pos.X >= x + 1 && mouse_pos.X < x + 1 + ((CButton)command).text.Length && mouse_pos.Y >= y + 1 + j && mouse_pos.Y < y + 2 + j) 
						{
							parser.parser(Request("127.0.0.1", 91, ((CButton)command).url));
							url = ((CButton)command).url;
						}
						break;
				}
				j++;
            }
			int v = 5 + 1 + 9 + 1 + 2 + 1 + url.Length + 1;
			graphics.Text(x + 1 + v + 1, y + 1, "refrash");
			refresh.update(mouse_button, mouse_pos, x + 1 + v + 1, y + 1, 7, 1);
			last_button = mouse_button;
        }
	}
}
