#region

using System.Threading.Tasks;

#endregion

namespace MagnifierMemes.Memes
{
    public interface IMeme
    {
        Task Execute();
    }
}