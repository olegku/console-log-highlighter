using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ConsoleLogHighlighter
{
    public partial class ConsoleLogHighlighter : IDisposable
    {
        private readonly TextWriter m_stdOut;
        private readonly TextWriter m_highlightedOut;

        public ConsoleLogHighlighter()
        {
            EnableVirtualTerminalMode();

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

            var highlighter = new Highlighter();

            highlighter.AddStyle(StyleKeys.Date, Styles.ForegroundGreen);
            highlighter.AddStyle(StyleKeys.ExceptionType, Styles.BrightForegroundRed);
            highlighter.AddStyle(StyleKeys.Exception, Styles.ForegroundYellow);
            highlighter.AddStyle(StyleKeys.String, Styles.ForegroundYellow);
            highlighter.AddStyle(StyleKeys.Constant, Styles.BrightForegroundBlue);

            // https://github.com/emilast/vscode-logfile-highlighter/blob/master/syntaxes/log.tmLanguage

            // ISO dates ("2021-04-24")
            highlighter.AddPattern(@"\b\d{4}-\d{2}-\d{2}(T|\b)", StyleKeys.Date);

            // Culture specific dates ("23/08/2016", "23.08.2016")
            highlighter.AddPattern(@"(?<=(^|\s))\d{2}[^\w\s]\d{2}[^\w\s]\d{4}\b", StyleKeys.Date);

            // Clock times with optional timezone ("09:13:16", "09:13:16.323", "09:13:16+01:00")
            highlighter.AddPattern(@"\d{1,2}:\d{2}(:\d{2}([.,]\d{1,})?)?(Z| ?[+-]\d{1,2}:\d{2})?\b", StyleKeys.Date);

            // Git commit hashes of length 40, 10 or 7
            highlighter.AddPattern(@"\b([0-9a-fA-F]{40}|[0-9a-fA-F]{10}|[0-9a-fA-F]{7})\b", StyleKeys.Constant);

            // GUIDs
            highlighter.AddPattern(@"\b[0-9a-fA-F]{8}[-]?([0-9a-fA-F]{4}[-]?){3}[0-9a-fA-F]{12}\b", StyleKeys.Constant);

            // MAC addresses: 89:A1:23:45:AB:C0
            highlighter.AddPattern(@"\b([0-9a-fA-F]{2,}[:-])+[0-9a-fA-F]{2,}\b", StyleKeys.Constant);

            // Constants
            highlighter.AddPattern(@"\b([0-9]+|True|true|False|false|null)\b", StyleKeys.Constant);

            // String constants
            highlighter.AddPattern(@"""[^""]*""", StyleKeys.String);
            highlighter.AddPattern(@"(?<![\w])'[^']*'", StyleKeys.String);
            
            // Exception type names
            highlighter.AddPattern(@"\b([a-zA-Z.]*Exception)\b", StyleKeys.ExceptionType);

            // Colorize rows of exception call stacks
            highlighter.AddPattern(@"^[\t ]*at .*$", StyleKeys.Exception);

            // Match Urls
            highlighter.AddPattern(@"\b[a-z]+://\S+\b/?", StyleKeys.Constant);

            // Match character and . sequences (such as namespaces)
            highlighter.AddPattern(@"(?<![\w/])([\w-]+\.)+([\w-])+(?![\w/\\])", StyleKeys.Constant);

            var highlighterTextWriter = new HighlighterTextWriter(streamWriter, highlighter.Transform);
            return TextWriter.Synchronized(highlighterTextWriter);
        }

        #region EnableVirtualTerminalMode

        private static void EnableVirtualTerminalMode()
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
