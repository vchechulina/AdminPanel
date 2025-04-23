using System.Windows;
using System.Windows.Controls;

namespace AdminPanel
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new UsersPage());
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            var confirmExit = new ConfirmExitWindow();
            if (confirmExit.ShowDialog() == true)
            {
                Close();
            }
        }

        private void DragWindow(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void NavigateToUsers(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UsersPage());
        }

        private void NavigateToRoles(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new RolesPage());
        }

        private void NavigateToAccessRights(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AccessRightsPage());
        }

        private void NavigateToHistory(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new HistoryPage());
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var confirmExit = new ConfirmExitWindow();
            if (confirmExit.ShowDialog() == true)
            {
                App.TokenTimer?.Stop();
                new LoginWindow().Show();
                Close();
            }
        }
    }
}