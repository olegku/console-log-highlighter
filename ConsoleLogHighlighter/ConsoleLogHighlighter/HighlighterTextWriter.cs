using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleLogHighlighter
{
    internal sealed class HighlighterTextWriter : TextWriter
    {
        private readonly TextWriter m_textWriter;
        private readonly Func<string, string> m_lineTransformer;

        internal HighlighterTextWriter(TextWriter textWriter, Func<string, string> lineTransformer)
        {
            m_textWriter = textWriter;
            m_lineTransformer = lineTransformer;
        }

        public override Encoding Encoding => m_textWriter.Encoding;

        public override void Write(char[] buffer, int index, int count)
        {
            m_textWriter.Write(buffer, index, count);
        }

        public override void Write(string value)
        {
            m_textWriter.Write(value);
        }

        public override void WriteLine()
        {
            m_textWriter.WriteLine();
        }

        public override void WriteLine(string value)
        {
            m_textWriter.WriteLine(m_lineTransformer(value));
        }

        //public override void WriteLine(object value)
        //{
        //}
    }
}
