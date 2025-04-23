using System;
using System.Timers;
using System.Windows;

namespace AdminPanel
{
    public class TokenTimerService : IDisposable
    {
        private readonly System.Timers.Timer _timer;
        private readonly Action _onTokenExpired;
        private readonly Action _onTokenAboutToExpire;
        private bool _warningShown = false;

        public TokenTimerService(Action onTokenAboutToExpire, Action onTokenExpired)
        {
            _onTokenAboutToExpire = onTokenAboutToExpire;
            _onTokenExpired = onTokenExpired;

            _timer = new System.Timers.Timer(60000); // Перевіряємо кожну хвилину
            _timer.Elapsed += TimerElapsed;
        }

        public void Start(int tokenLifetimeMinutes)
        {
            // Встановлюємо час сповіщення про закінчення (за 5 хвилин)
            var warningTime = (tokenLifetimeMinutes - 5) * 60 * 1000;
            var expirationTime = tokenLifetimeMinutes * 60 * 1000;

            _timer.Interval = Math.Min(60000, warningTime); // Перевіряємо не рідше ніж раз на хвилину
            _warningShown = false;
            _timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (!_warningShown)
                {
                    _warningShown = true;
                    _onTokenAboutToExpire?.Invoke();
                }
                else
                {
                    _timer.Stop();
                    _onTokenExpired?.Invoke();
                }
            });
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}