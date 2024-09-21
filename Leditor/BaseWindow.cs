using Leditor.TextLib;
using static Leditor.Structs;
using static Leditor.SystemDLLs;
using static Leditor.WindowsMessages;
using static Leditor.MiscConsts;
using static Leditor.SystemMacros;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;

namespace Leditor.Leditor
{
    public class BaseWindow
    {
        IntPtr hInstance;
        WNDCLASS wndClass;
        IntPtr hWnd;
        ushort reg;
        uint color;

        private IText currentText = new StringText();
        private bool changed;
        private bool cursorIndexChanged;
        private TextCursor textCursor;

        private int cursorIndex = 0;
        private int margin = 10;

        IntPtr textCursorBM;

        const int CURSOR_WIDTH = 1;  // Width of the bitmap (in pixels)
        const int CURSOR_HEIGHT = 16; // Height of the bitmap (in pixels)
        public BaseWindow()
        {
            hInstance = GetModuleHandle(null);
            textCursor = new TextCursor();
            DefineWNDClass();   
        }

        private void DefineWNDClass()
        {
            wndClass = new WNDCLASS
            {
                style = 0,
                lpfnWndProc = WndProcedure,
                cbClsExtra = 0,
                cbWndExtra = 0,
                hInstance = hInstance,
                hIcon = IntPtr.Zero,
                hCursor = LoadCursor(IntPtr.Zero, 32512),  // IDC_ARROW = 32512
                hbrBackground = (IntPtr)(1 + 1),          // COLOR_WINDOW = 1
                lpszMenuName = null,
                lpszClassName = "BasicWinAPIGUI"
            };

            reg = RegisterClass(ref wndClass);
            if (reg == 0)
            {
                StringBuilder errorMsg = new StringBuilder(256);
                FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, IntPtr.Zero, (uint)Marshal.GetLastWin32Error(), 0, errorMsg, (uint)errorMsg.Capacity, IntPtr.Zero);
                Console.WriteLine($"Failed to register class: {errorMsg}");
                return;
            }

            hWnd = CreateWindowEx(0, "BasicWinAPIGUI", "Leditor", WS_OVERLAPPEDWINDOW,
            100, 100, 800, 600, IntPtr.Zero, IntPtr.Zero, hInstance, IntPtr.Zero);

            if (hWnd == IntPtr.Zero)
            {
                StringBuilder buffer = new StringBuilder(256);
                FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, IntPtr.Zero, (uint)Marshal.GetLastWin32Error(), 0, buffer, 256, IntPtr.Zero);
                Console.WriteLine($"Window creation failed: {buffer}");
                return;
            }
        }

        public void Show()
        {
            ShowWindow(hWnd, SW_SHOW);

            color = GetSysColor(COLOR_WINDOW);
            Console.WriteLine($"Window Background Color: #{color:X} or {color}");

            StartMessageLoop();
        }

        private void StartMessageLoop()
        {
            MSG msg;
            Stopwatch fpsWatch = new Stopwatch();
            fpsWatch.Start();
            int targetFPS = 60;
            int targetInterval = 1000 / targetFPS;
            int ticks = 0;
            int sum = 0;

            int tickInterval = 500;

            while (GetMessage(out msg, IntPtr.Zero, 0, 0))
            {
                TranslateMessage(ref msg);
                DispatchMessage(ref msg);

                ticks++;
                if(fpsWatch.Elapsed.Milliseconds >= 1)
                {
                    if (ticks < tickInterval)
                    {
                        sum += fpsWatch.Elapsed.Milliseconds;
                    }
                    else
                    {
                        int average = sum / tickInterval;

                        if(average / targetInterval > 1.5f)
                        {
                            tickInterval -= (average * 4/3);
                        }
                        sum = 0;

                        int delta = fpsWatch.Elapsed.Milliseconds;

                        ticks = 0;
                        textCursor.Update(delta);

                        fpsWatch.Restart();
                    }
                }               
            }
            fpsWatch.Stop();
        }
        private IntPtr WndProcedure(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            IntPtr hdc;
            string[] textRows = currentText.ToString().Split('\n');
            switch (msg)
            {
                case WM_SETFOCUS:
                    Console.WriteLine("Focusing!");
                    
                    int bitmapSize = (CURSOR_WIDTH + 7) / 8 * CURSOR_HEIGHT;
                    byte[] whiteBits = new byte[bitmapSize];

                    for (int i = 0; i < whiteBits.Length; i++)
                    {
                        whiteBits[i] = 0xFF;
                    }

                    textCursorBM = CreateBitmap(CURSOR_WIDTH, CURSOR_HEIGHT, 1, 1, whiteBits);
                    bool createCaret = CreateCaret(hWnd, IntPtr.Zero, 1, 16);
                    Console.WriteLine($"Create Caret: {createCaret}");
                    SetCaretPos(100, 150);
                    bool showCaret = ShowCaret(hWnd);    
                    
                    if (showCaret == false)
                    {
                        StringBuilder messageBuffer = new StringBuilder(256);
                        uint e = GetLastError();
                        Console.WriteLine("Showing Caret failed");
                        Console.WriteLine(FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
                                IntPtr.Zero,
                                e,
                                0, // Use default language
                                messageBuffer,
                                (uint)messageBuffer.Length,
                                IntPtr.Zero));
                    }
                    Console.WriteLine($"Showing Caret: {showCaret}");
                    break;
                case WM_KILLFOCUS:
                    DeleteObject(textCursorBM);
                    DestroyCaret();
                    break;
                case WM_DESTROY:
                    DestroyWindow(hWnd);
                    Console.WriteLine("Window destroyed.");
                    break;
                case WM_LBUTTONDOWN:
                    GetCursorPos(out POINT point);
                    ScreenToClient(hWnd, ref point);
                    Console.WriteLine($"Cursor Position: ({point.x}, {point.y})");

                    hdc = GetDC(hWnd);

                    cursorIndex = GetCursorIndexFromPosition(
                        hdc, textRows[0], point.x);
                    cursorIndexChanged = true;
                    Console.WriteLine($"New index: {cursorIndex}");

                    ReleaseDC(hWnd, hdc);

                    break;
                case WM_PAINT:
                    // Retrieve device context
                    hdc = GetDC(hWnd);

                    //NewDrawTextCursor(hWnd, hdc, textRows[0]);
                    
                    if (!changed)
                    {
                        ReleaseDC(hWnd, hdc);
                        return IntPtr.Zero;
                    }

                    // Clear the window (this will erase the previously drawn text)
                    RECT rect;
                    GetClientRect(hWnd, out rect);
                    var fill = FillRect(hdc, ref rect, (IntPtr)(COLOR_BACKGROUND + 1));
                    
                    int bkMode = SetBkMode(hdc, 1);

                    SetTextColor(hdc, rgbWhite);

                    for (int i= 0; i < textRows.Length; i++)
                    {
                        int rowPos = 25 + (18 * i);
                        TextOut(hdc, margin, rowPos, textRows[i], textRows[i].Length);
                    }

                    /*TEXTMETRIC tm;
                    if (GetTextMetrics(hdc, out tm))
                    {
                        // tmHeight gives the height of a line of text in the current font
                        //Console.WriteLine($"Text Height: {tm.tmHeight}");
                    } */               

                    ReleaseDC(hWnd, hdc);
                    //Console.WriteLine($"Text: {currentText.ToString()}");

                    changed = false;
                    break;
                case WM_CHAR:
                    char character = (char)wParam;

                    int curIndex = cursorIndex;
                    if (character == '\b')
                    {
                        currentText.RemoveText(cursorIndex - 1);
                        cursorIndex -= 1;
                    }
                    else if(character == '\r')
                    {
                        currentText.AddText('\n', cursorIndex);
                        cursorIndex = 0;
                    }
                    else if(character == '\t')
                    {
                        currentText.AddText("    ", cursorIndex);
                        cursorIndex += 4;
                    }
                    else
                    {
                        currentText.AddText(character, cursorIndex);
                        cursorIndex++;
                    }
                    Console.WriteLine($"Text: {currentText} {cursorIndex}");

                    string[] newTextRows = currentText.ToString().Split('\n');

                    if (cursorIndex < 0)
                        cursorIndex = 0;
                    else if (cursorIndex > newTextRows[0].Length)
                        cursorIndex = newTextRows[0].Length;

                    changed = true;
                    break;
                case WM_MOUSEMOVE:
                    RECT clientRect = new RECT();
                    if(GetWindowRect(hWnd, out clientRect))
                    {
                        int left = clientRect.left;
                        int right = clientRect.right;
                        int bottom = clientRect.bottom;
                        int top= clientRect.top;

                        int width = right - left;
                        int height = bottom - top;

                        ushort x = LOWORD((uint)lParam);
                        ushort y = HIWORD((uint)lParam);

                        string m_Pos = $"m_pos: ({x}, {y})";

                        hdc = GetDC(hWnd);

                        TextOut(hdc, width - 140, height - 58, m_Pos, m_Pos.Length);
                        ReleaseDC(hWnd, hdc);
                    }

                    break;
                default:
                    return DefWindowProc(hWnd, msg, wParam, lParam);
            }
            return IntPtr.Zero;
        }

        private void NewDrawTextCursor(IntPtr Hwnd, IntPtr hdc, string text)
        {
            SIZE size;
            GetTextExtentPoint32(hdc, text, cursorIndex + 1, out size);
            textCursor.x = size.cx;
            /*SIZE endOfText = GetEndOfText(hdc, text);

            if (cursorIndex >= text.Length)
                textCursor.x = endOfText.cx*/;

            if (cursorIndexChanged)
            {
                Console.WriteLine($"Starting x: {textCursor.x}");
                textCursor.x += margin;
                Console.WriteLine($"x: {textCursor.x} index: {cursorIndex}");
                cursorIndexChanged = false;
            }
            
            PaintTextCursor(hWnd, hdc);
        }

        private int GetCursorIndexFromPosition(IntPtr hdc, string text, int x)
        {
            int index = 0;
            SIZE size;

            Console.WriteLine($"Getting Index From: {text} of length {text.Length} " +
                $"with Point :{x}");
            for (int i = 0; i < text.Length; i++)
            {
                GetTextExtentPoint32(hdc, text, i + 1, out size);
                Console.WriteLine($"[{i}] {text[i]} size: {size.cx}");
                if (size.cx + margin > x)
                {
                    return i;
                }
                index = i;
            }
            return index;
        }

        private void PaintTextCursor(IntPtr Hwnd, IntPtr hdc)
        {
            SetCaretPos(textCursor.x, textCursor.y);
            
            /*if (textCursor.isShowing)
            {
                if (textCursor.NewPaintedPosition())
                {
                    PatBlt(hdc, textCursor.lastPaintedX, textCursor.lastPaintedY,
                    textCursor.w, textCursor.h, BLACKNESS);
                }

                PatBlt(hdc, textCursor.x, textCursor.y,
                    textCursor.w, textCursor.h, WHITENESS);

                textCursor.Painted();
            }
            else
            {
                PatBlt(hdc, textCursor.x, textCursor.y,
                    textCursor.w, textCursor.h, BLACKNESS);
            }*/
        }
        private SIZE GetEndOfText(IntPtr hdc, string text)
        {
            SIZE size;
            GetTextExtentPoint32(hdc, text, text.Length + 1, out size);
            size.cx += 2;
            return size;
        }
    }
}
