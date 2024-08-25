using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleWindowsSystem.Windows.Browser
{
	public class Parser
	{
        public List<ICommand> commands;
		public void parser(string code) {
            var commands = new List<ICommand>();
            foreach (var line in code.ReplaceLineEndings().Split(Environment.NewLine))
            {
                var data = line.Trim().Split(": ");
                if (data.Length > 0)
                {
                    switch (data[0])
                    {
                        case "Hr":
                            commands.Add(new CHr());
                            break;
						case "Text":
							commands.Add(new CText(data[1]));
							break;
						case "Button":
							commands.Add(new CButton(data[1], data[2]));
							break;
					}
                }
            }
            this.commands = commands;
        }
	}
}
