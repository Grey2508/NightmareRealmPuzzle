using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NightmareRealmPuzzle
{
    internal static class Timer
    {
        private static Label _timerLbl;
        private static int _timer;

        private static bool _busy = false;

        public static void Initialize(Label timerLabel)
        {
            _timerLbl = timerLabel;
            _timer = 0;
        }

        public static async void Start()
        {
            if (_busy)
                return;

            _busy = true;

            while(_busy)
            {
                _timerLbl.Content = GetTime();
                _timer++;

                await Task.Run(() => Thread.Sleep(1000));
            }
        }

        public static string GetTimeAndStop()
        {
            _busy = false;

            return GetTime();
        }

        public static void Reset()
        {
            _timer = 0;

            _timerLbl.Content = GetTime();
        }

        private static string GetTime()
        {
            int min = _timer / 60;
            int sec = _timer % 60;

            string minStr = min < 10 ? "0" + min : min.ToString();
            string secStr = sec < 10 ? "0" + sec : sec.ToString();

            return $"{minStr}:{secStr}";
        }
    }
}
