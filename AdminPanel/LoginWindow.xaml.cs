using System.Windows;

namespace AdminPanel
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            // Тут повинна бути ваша реальна логіка перевірки логіну та паролю
            if (username == "admin" && password == "password") // Замініть на реальну перевірку
            {
                // Успішна авторизація - відкриваємо головне вікно та закриваємо вікно логіну
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
            else
            {
                lblErrorMessage.Text = "Невірне ім'я користувача або пароль.";
                lblErrorMessage.Visibility = Visibility.Visible;
            }
        }
        private void txtUsername_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // Тут ви можете додати код, який буде виконуватися при зміні тексту в полі txtUsername
            // Наприклад, ви можете очищати повідомлення про помилку при введенні нових даних.
            lblErrorMessage.Visibility = Visibility.Collapsed;
        }
    }
}