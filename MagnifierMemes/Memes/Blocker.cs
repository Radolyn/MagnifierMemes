#region

using System.Runtime.InteropServices;
using System.Threading.Tasks;

#endregion

namespace MagnifierMemes.Memes
{
    [Meme("block_input", "Blocks any mouse and keyboard events except CTRL+ALT+DEL")]
    public class Blocker : IMeme
    {
        /// <inheritdoc />
        public Task Execute()
        {
            BlockInput(true);
            ShowSystemCursor(false);

            return Task.CompletedTask;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int BlockInput(bool fBlockIt);

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowSystemCursor(bool fShowCursor);
    }
}