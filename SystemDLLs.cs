using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using static Leditor.Structs;

namespace Leditor
{
    public static class SystemDLLs
    {
    #region user32

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int MessageBox(IntPtr h, string m, string c, int type);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr CreateWindowEx(
            int dwExStyle,
            string lpClassName,
            string lpWindowName,
            uint dwStyle,
            int x, int y, int nWidth, int nHeight,
            IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect); 
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint GetSysColorBrush(int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint GetSysColor(int nIndex); // To retrieve system color values
    [DllImport("user32.dll")]
    public static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern ushort RegisterClass(ref WNDCLASS lpWndClass);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr DefWindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool DestroyWindow(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool TranslateMessage(ref MSG lpMsg);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr DispatchMessage(ref MSG lpMsg);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport("user32.dll")]
    public static extern int FillRect(IntPtr hDC, ref RECT lprc, IntPtr hBrush);
    [DllImport("user32.dll")]
    public static extern int FillRect(IntPtr hDC, ref RECT lprc, int colorValue );

        [DllImport("user32.dll")]
    public static extern bool CreateCaret(IntPtr hdc, IntPtr hBitmap, int nWidth, int nHeight);

    [DllImport("user32.dll")]
    public static extern bool DestroyCaret();

    [DllImport("user32.dll")]
    public static extern bool SetCaretPos(int x, int y);
    [DllImport("user32.dll")]
    public static extern bool ShowCaret(IntPtr Hwnd);

        #endregion

    [DllImport("kernel32.dll")]
    public static extern uint GetLastError();

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern int FormatMessage(
        uint dwFlags,
        IntPtr lpSource,
        uint dwMessageID,
        uint dwLanguageID,
        StringBuilder lpBuffer,
        uint nSize,
        IntPtr Arguments);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr h);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        public static extern bool TextOut(IntPtr hdc, int x, int y, string lpString, int nCount);
        [DllImport("gdi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool GetTextExtentPoint32(IntPtr hdc, string lpString, int c, out SIZE lpSize);
        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetTextMetrics(IntPtr hdc, out TEXTMETRIC lptm);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetTextColor(IntPtr hdc, int color);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern int SetBkMode(IntPtr hdc, int mode);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern bool PatBlt(IntPtr hdc, int x, int y, int w, int h, int rop);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateBitmap(int nWidth, int nHeight, uint cPlanes, uint cBitsPerPel, byte[] lpvBits);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern int SetBkColor(IntPtr hdc, uint crColor);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);

    }

}
