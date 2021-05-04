namespace ConsoleLogHighlighter
{
    // https://docs.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences#text-formatting
    public static class Styles
    {
        // Returns all attributes to the default state prior to modification
        public static readonly string Default = "\x1b[0m";

        // Applies brightness/intensity flag to Foreground color
        public static readonly string BoldBright = "\x1b[1m";

        // Removes brightness/intensity flag from Foreground color
        public static readonly string NoBoldBright = "\x1b[22m";

        // Adds underline
        public static readonly string Underline = "\x1b[4m";

        // Removes underline
        public static readonly string NoUnderline = "\x1b[24m";

        // Swaps Foreground and Background colors
        public static readonly string Negative = "\x1b[7m";

        // Returns foreground/Background to normal
        public static readonly string PositiveNoNegative = "\x1b[27m";

        // Applies non-bold/bright black to foreground
        public static readonly string ForegroundBlack = "\x1b[30m";

        // Applies non-bold/bright red to foreground
        public static readonly string ForegroundRed = "\x1b[31m";

        // Applies non-bold/bright green to foreground
        public static readonly string ForegroundGreen = "\x1b[32m";

        // Applies non-bold/bright yellow to foreground
        public static readonly string ForegroundYellow = "\x1b[33m";

        // Applies non-bold/bright blue to foreground
        public static readonly string ForegroundBlue = "\x1b[34m";

        // Applies non-bold/bright magenta to foreground
        public static readonly string ForegroundMagenta = "\x1b[35m";

        // Applies non-bold/bright cyan to foreground
        public static readonly string ForegroundCyan = "\x1b[36m";

        // Applies non-bold/bright white to foreground
        public static readonly string ForegroundWhite = "\x1b[37m";

        // Applies extended color value to the Foreground(see details below)
        public static readonly string ForegroundExtended = "\x1b[38m";

        // Applies only the Foreground portion of the defaults (see 0)
        public static readonly string ForegroundDefault = "\x1b[39m";

        // Applies non-bold/bright black to background
        public static readonly string BackgroundBlack = "\x1b[40m";

        // Applies non-bold/bright red to background
        public static readonly string BackgroundRed = "\x1b[41m";

        // Applies non-bold/bright green to background
        public static readonly string BackgroundGreen = "\x1b[42m";

        // Applies non-bold/bright yellow to background
        public static readonly string BackgroundYellow = "\x1b[43m";

        // Applies non-bold/bright blue to background
        public static readonly string BackgroundBlue = "\x1b[44m";

        // Applies non-bold/bright magenta to background
        public static readonly string BackgroundMagenta = "\x1b[45m";

        // Applies non-bold/bright cyan to background
        public static readonly string BackgroundCyan = "\x1b[46m";

        // Applies non-bold/bright white to background
        public static readonly string BackgroundWhite = "\x1b[47m";

        // Applies extended color value to the Background(see details below)
        public static readonly string BackgroundExtended = "\x1b[48m";

        // Applies only the Background portion of the defaults (see 0)
        public static readonly string BackgroundDefault = "\x1b[49m";

        // Applies bold/bright black to foreground
        public static readonly string BrightForegroundBlack = "\x1b[90m";

        // Applies bold/bright red to foreground
        public static readonly string BrightForegroundRed = "\x1b[91m";

        // Applies bold/bright green to foreground
        public static readonly string BrightForegroundGreen = "\x1b[92m";

        // Applies bold/bright yellow to foreground
        public static readonly string BrightForegroundYellow = "\x1b[93m";

        // Applies bold/bright blue to foreground
        public static readonly string BrightForegroundBlue = "\x1b[94m";

        // Applies bold/bright magenta to foreground
        public static readonly string BrightForegroundMagenta = "\x1b[95m";

        // Applies bold/bright cyan to foreground
        public static readonly string BrightForegroundCyan = "\x1b[96m";

        // Applies bold/bright white to foreground
        public static readonly string BrightForegroundWhite = "\x1b[97m";

        // Applies bold/bright black to background
        public static readonly string BrightBackgroundBlack = "\x1b[100m";

        // Applies bold/bright red to background
        public static readonly string BrightBackgroundRed = "\x1b[101m";

        // Applies bold/bright green to background
        public static readonly string BrightBackgroundGreen = "\x1b[102m";

        // Applies bold/bright yellow to background
        public static readonly string BrightBackgroundYellow = "\x1b[103m";

        // Applies bold/bright blue to background
        public static readonly string BrightBackgroundBlue = "\x1b[104m";

        // Applies bold/bright magenta to background
        public static readonly string BrightBackgroundMagenta = "\x1b[105m";

        // Applies bold/bright cyan to background
        public static readonly string BrightBackgroundCyan = "\x1b[106m";

        // Applies bold/bright white to background
        public static readonly string BrightBackgroundWhite = "\x1b[107m";
    }
}