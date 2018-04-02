using System;
using System.IO;

namespace Shields.GraphViz.Models
{
    public class Id
    {
        private const string quote = "\"";
        private const string backslash = "\\";

        private readonly string value;

        public string Value
        {
            get { return value; }
        }

        public Id(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            this.value = value;
        }

        public static implicit operator Id(string value)
        {
            return new Id(value);
        }

        public void WriteTo(StreamWriter writer, bool putInQuotes = true)
        {
            var escapedValue = value.Replace(quote, backslash + quote);

            if(putInQuotes)
            {
                escapedValue = quote + escapedValue + quote;
                writer.Write(escapedValue);
            }
            else
            {
                writer.Write(value);
            }
        }

        public override string ToString()
        {
            return this.Value;
        }
    }
}
