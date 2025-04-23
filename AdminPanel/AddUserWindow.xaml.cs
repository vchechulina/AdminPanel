using System.Collections.Generic;
using System.Windows;

namespace AdminPanel
{
    public partial class AddUserWindow : Window
    {
        public UsersPage.UserDto NewUser { get; private set; }

        public AddUserWindow()
        {
            InitializeComponent();
            NewUser = new UsersPage.UserDto();
            LoadRoles();
        }

        private async void LoadRoles()
        {
            try
            {
                var roles = await App.ApiClient.GetAsync<List<EditUserWindow.RoleDto>>("/roles/");
                RoleComboBox.ItemsSource = roles;
                RoleComboBox.DisplayMemberPath = "Name";
                RoleComboBox.SelectedValuePath = "Id";
            }
            catch
            {
                MessageBox.Show("Не вдалося завантажити список ролей", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password) ||
                RoleComboBox.SelectedValue == null)
            {
                MessageBox.Show("Будь ласка, заповніть усі поля", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NewUser.First = FirstNameTextBox.Text;
            NewUser.Second = LastNameTextBox.Text;
            NewUser.Email = EmailTextBox.Text;
            NewUser.Role = (int)RoleComboBox.SelectedValue;
            // Пароль буде оброблений на сервері

            DialogResult = true;
            Close();
        }
    }
}