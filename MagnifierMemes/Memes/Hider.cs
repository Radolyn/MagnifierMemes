#region

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using PInvoke;

#endregion

namespace MagnifierMemes.Memes
{
    [Meme("hide", "Hides console window")]
    public class Hider : IMeme
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        /// <inheritdoc />
        public Task Execute()
        {
            Console.TreatControlCAsInput = true;

            var hwnd = Kernel32.GetConsoleWindow();
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

            Console.WindowTop = 0;
            Console.WindowLeft = 0;

            Console.WindowHeight = 1;
            Console.WindowWidth = 1;

            return Task.CompletedTask;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    }
}