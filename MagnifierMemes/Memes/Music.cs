#region

using System.IO;
using System.Media;
using System.Threading.Tasks;

#endregion

namespace MagnifierMemes.Memes
{
    [Meme("music", "Plays music from assets\\music.wav (only .wav)")]
    public class Music : IMeme
    {
        /// <inheritdoc />
        public Task Execute()
        {
            var player = new SoundPlayer {Stream = File.Open("assets\\music.wav", FileMode.Open)};
            player.PlayLooping();

            return Task.CompletedTask;
        }
    }
}