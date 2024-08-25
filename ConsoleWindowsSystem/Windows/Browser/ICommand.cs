using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleWindowsSystem.Windows.Browser
{
	public interface ICommand
	{
		string type_ { get; }
	}
	public class CHr : ICommand
	{
		string ICommand.type_  => "Hr";
	}
	public class CText : ICommand
	{
		string ICommand.type_ => "Text";
		public string text;
		public CText(string text) 
		{
			set_text(text);
		}
		public void set_text(string text) 
		{ 
			this.text = text.Replace("<gh>", ":").Replace("<a>", "<").Replace("<b>", ">");
		}
	}
	public class CButton : ICommand
	{
		string ICommand.type_ => "Button";
		public string text;
		public string url;
		public CButton(string text, string url)
		{
			set_text(text);
			set_url(url);
		}
		public void set_text(string text)
		{
			this.text = text.Replace("<gh>", ":").Replace("<a>", "<").Replace("<b>", ">");
		}
		public void set_url(string url)
		{
			this.url = url.Trim().Replace("\n", "").Replace("\r", "");
		}
	}
}
