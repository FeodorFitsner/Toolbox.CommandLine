using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.CommandLine
{
    class StringCollector
    {
        private int indent;

        public StringCollector(int witdh)
        {
            Witdh = witdh;
        }

        private StringBuilder Builder { get; } = new StringBuilder();
        public int Witdh { get; }
        public int Indent 
        { 
            get => indent;
            set 
            {
                indent = value;
            }
        }

        public override string ToString()
        {
            var text = Builder.ToString();
            if (Line != null)
                text += new string(' ',Indent) + Line;

            return text;
        }

        private string Line { get; set; }

        public void AppendLine(string text)
        {
            Append(text);
            if (Line != null)
            {
                Builder.AppendLine(new string(' ', Indent) + Line);
                Line = null;
            }
        }


        public void Append(string text)
        {
            var parts = text.Split(' ');
            foreach (var part in parts)
            {
                if (Line != null)
                {
                    if (Line.Length + part.Length + 1 > Witdh-Indent)
                    {
                        Builder.AppendLine(new string(' ',Indent) + Line);
                        Line = null;
                    }
                    else
                    {
                        Line += " " + part;
                    }
                }
                if (Line == null)
                {
                    if (part.Length > Witdh-Indent)
                        throw new ArgumentException($"text too wide '{part}'", nameof(text));
                    
                    Line = part;
                }
            }
        }
    }
}
