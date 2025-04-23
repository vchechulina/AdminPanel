using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace AdminPanel
{
    public partial class EditUserWindow : Window
    {
        public UsersPage.UserDto UpdatedUser { get; private set; }

        public EditUserWindow(UsersPage.UserDto user)
        {
            InitializeComponent();
            UpdatedUser = user;

            // Заповнити поля даними користувача
            FirstNameTextBox.Text = user.First;
            LastNameTextBox.Text = user.Second;
            EmailTextBox.Text = user.Email;

            // Завантажити ролі (припускаємо, що ми маємо API для отримання ролей)
            LoadRoles(user.Role);
        }

        private async void LoadRoles(int selectedRoleId)
        {
            try
            {
                var roles = await App.ApiClient.GetAsync<List<RoleDto>>("/roles/");
                RoleComboBox.ItemsSource = roles;
                RoleComboBox.DisplayMemberPath = "Name";
                RoleComboBox.SelectedValuePath = "Id";
                RoleComboBox.SelectedValue = selectedRoleId;
            }
            catch
            {
                MessageBox.Show("Не вдалося завантажити список ролей", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                RoleComboBox.SelectedValue == null)
            {
                MessageBox.Show("Будь ласка, заповніть усі поля", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            UpdatedUser.First = FirstNameTextBox.Text;
            UpdatedUser.Second = LastNameTextBox.Text;
            UpdatedUser.Email = EmailTextBox.Text;
            UpdatedUser.Role = (int)RoleComboBox.SelectedValue;

            DialogResult = true;
            Close();
        }

        public class RoleDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}