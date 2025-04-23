using System.Windows;
using System.Windows.Controls;

namespace AdminPanel
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                lblErrorMessage.Text = "Будь ласка, введіть ім'я користувача та пароль";
                lblErrorMessage.Visibility = Visibility.Visible;
                return;
            }

            try
            {
                var token = await App.AuthService.Login(username, password);
                if (token != null)
                {
                    App.ApiClient.SetAuthToken(token);

                    // Ініціалізуємо таймер для токена (60 хвилин)
                    App.InitializeTokenTimer(60,
                        () => MessageBox.Show("Ваша сесія закінчиться через 5 хвилин. Будь ласка, збережіть свою роботу та увійдіть знову.", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning),
                        () => {
                            MessageBox.Show("Ваша сесія закінчилася. Будь ласка, увійдіть знову.", "Сесія завершена", MessageBoxButton.OK, MessageBoxImage.Information);
                            new LoginWindow().Show();
                            Application.Current.MainWindow?.Close();
                        });

                    // Успішна авторизація - відкриваємо головне вікно
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    Close();
                }
            }
            catch
            {
                lblErrorMessage.Text = "Невірне ім'я користувача або пароль";
                lblErrorMessage.Visibility = Visibility.Visible;
            }
        }

        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblErrorMessage.Visibility = Visibility.Collapsed;
        }
    }
}