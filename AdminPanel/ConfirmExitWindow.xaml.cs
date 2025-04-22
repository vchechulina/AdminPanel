using System.Windows;

namespace AdminPanel
{
    /// <summary>
    /// Interaction logic for ConfirmExitWindow.xaml
    /// </summary>
    public partial class ConfirmExitWindow : Window
    {
        public ConfirmExitWindow()
        {
            InitializeComponent();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true; // Встановлюємо результат діалогу як "Так"
            Close();
        }
    }
}