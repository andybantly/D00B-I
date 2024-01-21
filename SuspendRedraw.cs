using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace D00B
{
    public static class ControlExtensions
    {
        public static IDisposable SuspendDrawing(this Control control)
        {
            return new SuspendDrawing(control);
        }
    }

    public class SuspendDrawing : IDisposable
    {
        private const int WM_SETREDRAW = 0x000B;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private Control control;

        public SuspendDrawing(Control control)
        {
            this.control = control;
            SendMessage(control.Handle, WM_SETREDRAW, false, 0);
        }
        public void Dispose()
        {
            SendMessage(control.Handle, WM_SETREDRAW, true, 0);
            control.Refresh();
        }
    }
}
