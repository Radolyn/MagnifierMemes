#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PInvoke;
using RadLibrary.Configuration;

#endregion

namespace MagnifierMemes.Memes
{
    [Meme("color_changer", "Changes colors on the screen", "smooth_changing", "sleep_time")]
    public class MatrixChanger : IMeme
    {
        private readonly AppConfiguration _configuration;

        public MatrixChanger(AppConfiguration configuration)
        {
            _configuration = configuration;
        }

        private static List<float[,]> Matrices => GenerateMatrices();

        /// <inheritdoc />
        public async Task Execute()
        {
            var res = Magnification.MagInitialize();

            if (!res)
            {
                Console.WriteLine("Failed to initialize magnification API");
                return;
            }

            var timeoutSet = int.TryParse(_configuration["sleep_time"], out var sleepTime);

            var effect = new Magnification.MAGCOLOREFFECT();

            var random = new Random();

            for (var i = 0; i < int.MaxValue; i++)
            {
                var matr = Matrices[random.Next(0, Matrices.Count - 1)];

                var t = random.Next(0, 8);

                switch (t)
                {
                    case 0:
                        matr = MoreBlue(matr);
                        break;
                    case 1:
                        matr = MoreGreen(matr);
                        break;
                    case 2:
                        matr = MoreRed(matr);
                        break;
                    default:
                        matr = Multiply(matr, Matrices[random.Next(0, Matrices.Count - 1)]);
                        break;
                }

                if (!_configuration.GetBool("smooth_changing"))
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
                else
                {
                    var transitions = Interpolate(effect, matr);

                    foreach (var transition in transitions)
                    {
                        Apply(transition);
                        await Task.Delay(15);
                    }
                }

                if (timeoutSet)
                    await Task.Delay(sleepTime);
            }
        }

        private static List<float[,]> GenerateMatrices()
        {
            var l = new List<float[,]>(50);

            var rnd = new Random();

            for (var i = 0; i < l.Capacity; i++)
                l.Add(new[,]
                {
                    {
                        rnd.NextFloat(-4, 4), rnd.NextFloat(-4, 4), rnd.NextFloat(-4, 4), rnd.NextFloat(-4, 4),
                        rnd.NextFloat(-4, 4)
                    },
                    {
                        rnd.NextFloat(-4, 4), rnd.NextFloat(-4, 4), rnd.NextFloat(-4, 4), rnd.NextFloat(-4, 4),
                        rnd.NextFloat(-4, 4)
                    },
                    {
                        rnd.NextFloat(-4, 4), rnd.NextFloat(-4, 4), rnd.NextFloat(-4, 4), rnd.NextFloat(-4, 4),
                        rnd.NextFloat(-4, 4)
                    },
                    {
                        rnd.NextFloat(-4, 4), rnd.NextFloat(-4, 4), rnd.NextFloat(-4, 4), rnd.NextFloat(-4, 4),
                        rnd.NextFloat(-4, 4)
                    },
                    {
                        rnd.NextFloat(-4, 4), rnd.NextFloat(-4, 4), rnd.NextFloat(-4, 4), rnd.NextFloat(-4, 4),
                        rnd.NextFloat(-4, 4)
                    }
                });

            return l;
        }

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
                    result[i][x, y] = A[x, y] + (i + 1) * (B[x, y] - A[x, y]) / STEPS;
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
    }
}