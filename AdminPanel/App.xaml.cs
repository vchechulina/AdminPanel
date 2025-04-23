using System.Windows;

namespace AdminPanel
{
    public partial class App : Application
    {
        public static ApiClient ApiClient { get; private set; }
        public static AuthService AuthService { get; private set; }
        public static TokenTimerService TokenTimer { get; private set; }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            // Ініціалізація API клієнта
            ApiClient = new ApiClient("http://192.168.0.105:8000");
            AuthService = new AuthService(ApiClient);
            // Показати вікно логіну
            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }

        public static void InitializeTokenTimer(int tokenLifetimeMinutes, Action onTokenAboutToExpire, Action onTokenExpired)
        {
            TokenTimer?.Dispose();
            TokenTimer = new TokenTimerService(onTokenAboutToExpire, onTokenExpired);
            TokenTimer.Start(tokenLifetimeMinutes);
        }
    }
}