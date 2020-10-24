#region

using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

#endregion

namespace MagnifierMemes.Memes
{
    [Meme("autostart", "Creates autorun task")]
    public class AutoStart : IMeme
    {
        /// <inheritdoc />
        public Task Execute()
        {
            try
            {
                var rk = Registry.CurrentUser.OpenSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce", true);

                rk.SetValue("Prank", Application.ExecutablePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return Task.CompletedTask;
        }
    }
}