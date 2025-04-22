using System.Windows;
using System.Windows.Controls;

namespace AdminPanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Завантаження початкової сторінки (можна змінити)
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
            Close();
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
            MainFrame.Navigate(new RolesPage()); // Створіть RolesPage.xaml та RolesPage.xaml.cs
        }

        private void NavigateToAccessRights(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AccessRightsPage()); // Створіть AccessRightsPage.xaml та AccessRightsPage.xaml.cs
        }

        private void NavigateToHistory(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new HistoryPage()); // Створіть HistoryPage.xaml та HistoryPage.xaml.cs
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            ConfirmExitWindow confirmExitWindow = new ConfirmExitWindow();
            if (confirmExitWindow.ShowDialog() == true)
            {
                // Тут можна додати логіку виходу з акаунту
                Close();
            }
        }
    }
}