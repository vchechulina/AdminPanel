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
        private DateTime _expirationTime;
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
            _expirationTime = DateTime.Now.AddMinutes(tokenLifetimeMinutes);
            _warningShown = false;
            _timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var timeLeft = _expirationTime - DateTime.Now;

                if (!_warningShown && timeLeft.TotalMinutes <= 5)
                {
                    _warningShown = true;
                    _onTokenAboutToExpire?.Invoke();
                }
                else if (timeLeft.TotalMinutes <= 0)
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