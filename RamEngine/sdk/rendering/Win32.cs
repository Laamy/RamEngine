using System;

public static class Win32
{
    public const int PFD_TYPE_RGBA = 0;
    public const int PFD_MAIN_PLANE = 0x00000000;
    public const int PFD_DOUBLEBUFFER = 0x00000001;
    public const int PFD_DRAW_TO_WINDOW = 0x00000004;
    public const int PFD_SUPPORT_OPENGL = 0x00000020;

    [System.Runtime.InteropServices.DllImport("gdi32.dll")]
    public static extern int ChoosePixelFormat(IntPtr hdc, ref PIXELFORMATDESCRIPTOR pfd);

    [System.Runtime.InteropServices.DllImport("gdi32.dll")]
    public static extern bool SetPixelFormat(IntPtr hdc, int format, ref PIXELFORMATDESCRIPTOR pfd);

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    public static extern IntPtr GetDC(IntPtr hWnd);

    [System.Runtime.InteropServices.DllImport("opengl32.dll")]
    public static extern IntPtr wglCreateContext(IntPtr hdc);

    [System.Runtime.InteropServices.DllImport("opengl32.dll")]
    public static extern bool wglMakeCurrent(IntPtr hdc, IntPtr hglrc);

    [System.Runtime.InteropServices.DllImport("opengl32.dll")]
    public static extern bool SwapBuffers(IntPtr hdc);
}