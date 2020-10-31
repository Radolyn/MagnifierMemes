#region

using System;
using System.Threading.Tasks;
using PInvoke;
using RadLibrary.Configuration;

#endregion

namespace MagnifierMemes.Memes
{
    [Meme("zoomer", "Enables random zooming", "zoomer_sleep_time")]
    public class Zoomer : IMeme
    {
        private readonly AppConfiguration _configuration;

        public Zoomer(AppConfiguration configuration)
        {
            _configuration = configuration;
        }

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

            var rnd = new Random();

            while (true)
            {
                Magnification.MagSetFullscreenTransform(rnd.NextFloat(1, 6), rnd.Next(10, 500), rnd.Next(10, 500));

                if (timeoutSet)
                    await Task.Delay(sleepTime);
            }
        }
    }
}