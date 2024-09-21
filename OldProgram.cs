using System;
using System.Runtime.InteropServices;
using System.Text;
using static Leditor.Structs;
using static Leditor.SystemDLLs;
using static Leditor.WindowsMessages;

public class OldProgram
{
    // Constants
    const int WS_OVERLAPPEDWINDOW = 0x00CF0000;
    const uint FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
    const uint WM_DESTROY = 0x0002;
    const int SW_SHOW = 5;

    static int CreateMessageBox()
    {
        string myString;
        Console.Write("Enter your message: ");
        myString = Console.ReadLine();
        return MessageBox((IntPtr)0, myString, "My Message Box", 0);
    }

    // Delegate for the window procedure
    public delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    public static void OldMain(string[] args)
    {
        IntPtr hInstance = GetModuleHandle(null);

        WNDCLASS wndClass = new WNDCLASS
        {
            style = 0,
            lpfnWndProc = WndProcedure,
            cbClsExtra = 0,
            cbWndExtra = 0,
            hInstance = hInstance,
            hIcon = IntPtr.Zero,
            hCursor = LoadCursor(IntPtr.Zero, 32512),  // IDC_ARROW = 32512,
            hbrBackground = (IntPtr) 1 + 1,
            lpszMenuName = null,
            lpszClassName = "BasicWinAPIGUI"
        };

        ushort reg = RegisterClass(ref wndClass);
        if (reg == 0)
        {
            StringBuilder errorMsg = new StringBuilder(256);

            uint errorCode = (uint) Marshal.GetLastWin32Error();

            FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, IntPtr.Zero, 
                errorCode, 0, errorMsg, 
                (uint)errorMsg.Capacity, IntPtr.Zero);
            Console.WriteLine($"Failed to register class {errorCode}: {errorMsg}");
            return;
        }

        IntPtr hWnd = CreateWindowEx(0, "BasicWinAPIGUI", "My First Window", 
            WS_OVERLAPPEDWINDOW,
            100, 100, 800, 600, IntPtr.Zero, IntPtr.Zero, hInstance, IntPtr.Zero);

        if (hWnd == IntPtr.Zero)
        {
            StringBuilder windowCreate = new StringBuilder(256);
            uint errorCode = (uint)Marshal.GetLastWin32Error();
            FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, IntPtr.Zero, 
                errorCode, 0, windowCreate, 256, IntPtr.Zero);
            Console.WriteLine($"Window creation failed {errorCode}: {windowCreate}");
            //return;
        }
        Console.WriteLine($"Creating Window: {hWnd}");

        var show = ShowWindow(hWnd, SW_SHOW);

        MSG msg;
        while (GetMessage(out msg, IntPtr.Zero, 0, 0))
        {
            TranslateMessage(ref msg);
            DispatchMessage(ref msg);
        }
    }

    // Window procedure
    static IntPtr WndProcedure(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        switch (msg)
        {
            case WM_CREATE:
                return IntPtr.Zero;
            case WM_DESTROY:
                //DestroyWindow(hWnd);
                Console.WriteLine("Destroy");
                break;
            default:
                return DefWindowProc(hWnd, msg, wParam, lParam);
        }
        return IntPtr.Zero;
    }
}