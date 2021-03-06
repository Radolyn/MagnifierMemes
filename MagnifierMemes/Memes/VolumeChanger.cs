﻿#region

using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace MagnifierMemes.Memes
{
    [Meme("volume_upper", "Sets volume to 100% and unmutes it")]
    public class VolumeChanger : IMeme
    {
        private const byte VK_VOLUME_MUTE = 0xAD;
        private const byte VK_VOLUME_UP = 0xAF;
        private const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        /// <inheritdoc />
        public Task Execute()
        {
            for (var i = 0; i < int.MaxValue; i++)
            {
                VolumeUp();
                Thread
                    .Sleep(20);

                Mute();
            }

            return Task.CompletedTask;
        }

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern byte MapVirtualKey(uint uCode, uint uMapType);

        public static void VolumeUp()
        {
            keybd_event(VK_VOLUME_UP, MapVirtualKey(VK_VOLUME_UP, 0), KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_VOLUME_UP, MapVirtualKey(VK_VOLUME_UP, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        public static void Mute()
        {
            keybd_event(VK_VOLUME_MUTE, MapVirtualKey(VK_VOLUME_MUTE, 0), KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_VOLUME_MUTE, MapVirtualKey(VK_VOLUME_MUTE, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }
    }
}