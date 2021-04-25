using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ConsoleLogHighlighter
{
    // https://docs.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences
    public class ConsoleLogHighlighter : IDisposable
    {
        private readonly TextWriter m_stdOut;
        private readonly TextWriter m_highlightedOut;

        public ConsoleLogHighlighter()
        {
            EnableConsoleVirtualTerminalSequences();

            m_stdOut = Console.Out;
            m_highlightedOut = CreateHighlightedOut();
            Console.SetOut(m_highlightedOut);
        }

        public void Dispose()
        {
            m_highlightedOut.Dispose();
            Console.SetOut(m_stdOut);
        }

        private static TextWriter CreateHighlightedOut()
        {
            var stream = Console.OpenStandardOutput();
            if (stream == Stream.Null)
            {
                return TextWriter.Synchronized(StreamWriter.Null);
            }

            var outputEncoding = Console.OutputEncoding;
            
            // TODO: review args
            var streamWriter = new StreamWriter(stream, outputEncoding, 256, true)
            {
                AutoFlush = true,
            };

            var highlighter = new Highlighter
            {
                // ISO dates ("2021-04-24")
                { @"\b\d{4}-\d{2}-\d{2}(T|\b)", "Date"},

                // Culture specific dates ("23/08/2016", "23.08.2016")
                { @"(?<=(^|\s))\d{2}[^\w\s]\d{2}[^\w\s]\d{4}\b", "Date" },

                // Clock times with optional timezone ("09:13:16", "09:13:16.323", "09:13:16+01:00")
                { @"\d{1,2}:\d{2}(:\d{2}([.,]\d{1,})?)?(Z| ?[+-]\d{1,2}:\d{2})?\b", "Date" },

                // Git commit hashes of length 40, 10 or 7
                {@"\b([0-9a-fA-F]{40}|[0-9a-fA-F]{10}|[0-9a-fA-F]{7})\b", "Constant" },

                // Guids
                { @"\b[0-9a-fA-F]{8}[-]?([0-9a-fA-F]{4}[-]?){3}[0-9a-fA-F]{12}\b", "Constant" },

                // MAC addresses: 89:A1:23:45:AB:C0
                {@"\b([0-9a-fA-F]{2,}[:-])+[0-9a-fA-F]{2,}\b", "Constant" },

                // Constants
                {@"\b([0-9]+|True|true|False|false|null)\b", "Constant" },

                // String constants
                { @"""[^""]*""", "String"},
                { @"(?<![\w])'[^']*'", "String"},

                // Exception type names
                {@"\b([a-zA-Z.]*Exception)\b", "ExceptionType" },
                
                // Colorize rows of exception call stacks
                { @"^[\t ]*at .*$", "Exception" },

                // Match Urls
                {@"\b[a-z]+://\S+\b/?", "Constant" },

                // Match character and . sequences (such as namespaces)
                { @"(?<![\w/])([\w-]+\.)+([\w-])+(?![\w/\\])", "Constant" },
            };

            highlighter.AddStyle("Date", "\x1b[33m");
            highlighter.AddStyle("ExceptionType", "\x1b[91m");
            highlighter.AddStyle("Exception", "\x1b[33m");
            highlighter.AddStyle("String", "\x1b[33m");
            highlighter.AddStyle("Constant", "\x1b[1m");

            var highlighterTextWriter = new HighlighterTextWriter(streamWriter, highlighter.Transform);
            return TextWriter.Synchronized(highlighterTextWriter);
        }

        #region EnableConsoleVirtualTerminalSequences

        private static void EnableConsoleVirtualTerminalSequences()
        {
            const int STD_OUTPUT_HANDLE = -11;
            const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var stdOutHandle = GetStdHandle(STD_OUTPUT_HANDLE);
                GetConsoleMode(stdOutHandle, out var outConsoleMode);
                SetConsoleMode(stdOutHandle, outConsoleMode | ENABLE_VIRTUAL_TERMINAL_PROCESSING);
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        #endregion
    }
}
