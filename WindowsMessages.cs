namespace Leditor
{
    public static class WindowsMessages
    {
        // Constants for Windows Message Codes
        public const uint WM_CREATE = 0x0001;
        public const uint WM_DESTROY = 0x0002;
        public const uint WM_MOVE = 0x0003;
        public const uint WM_SIZE = 0x0005;
        public const uint WM_PAINT = 0x000F;
        public const uint WM_CLOSE = 0x0010;
        public const uint WM_QUIT = 0x0012;
        public const uint WM_CHAR = 0x0102;
        public const uint WM_KEYDOWN = 0x0100;
        public const uint WM_KEYUP = 0x0101;
        public const uint WM_LBUTTONDOWN = 0x0201;
        public const uint WM_RBUTTONDOWN = 0x0204;
        public const uint WM_MOUSEMOVE = 0x0200;
        public const uint WM_SETFOCUS = 0x0007;
        public const uint WM_KILLFOCUS = 0x0008;

        public static Dictionary<uint, string> messageMap = new Dictionary<uint, string>
        {
            { WM_CREATE, "WM_CREATE" },
            { WM_DESTROY, "WM_DESTROY" },
            { WM_MOVE, "WM_MOVE" },
            { WM_SIZE, "WM_SIZE" },
            { WM_PAINT, "WM_PAINT" },
            { WM_CLOSE, "WM_CLOSE" },
            { WM_QUIT, "WM_QUIT" },
            { WM_CHAR, "WM_CHAR"},
            { WM_KEYDOWN, "WM_KEYDOWN" },
            { WM_KEYUP, "WM_KEYUP" },
            { WM_LBUTTONDOWN, "WM_LBUTTONDOWN" },
            { WM_RBUTTONDOWN, "WM_RBUTTONDOWN" },
            { WM_MOUSEMOVE, "WM_MOUSEMOVE" }
        };
    }
}
