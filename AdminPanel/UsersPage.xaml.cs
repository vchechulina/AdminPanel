using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AdminPanel
{
    public partial class UsersPage : UserControl
    {
        public UsersPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        private async void LoadUsers()
        {
            try
            {
                // Завантаження користувачів з API
                var users = await App.ApiClient.GetAsync<List<UserDto>>("/users-with-roles/");
                UsersStackPanel.Children.Clear();

                foreach (var user in users)
                {
                    AddUserToUI(user);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження користувачів: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddUserToUI(UserDto user)
        {
            var border = new Border
            {
                BorderBrush = System.Windows.Media.Brushes.Purple,
                BorderThickness = new Thickness(1),
                Padding = new Thickness(8),
                Margin = new Thickness(0, 0, 0, 8),
                CornerRadius = new CornerRadius(4)
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // Іконка
            var icon = new MaterialDesignThemes.Wpf.PackIcon
            {
                Kind = MaterialDesignThemes.Wpf.PackIconKind.Account,
                Width = 32,
                Height = 32,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 8, 0)
            };

            // Інформація про користувача
            var stackPanel = new StackPanel();
            var nameText = new TextBlock
            {
                Text = $"{user.First} {user.Second}",
                FontWeight = FontWeights.Bold
            };
            var emailText = new TextBlock
            {
                Text = user.Email,
                Foreground = System.Windows.Media.Brushes.Gray
            };
            var roleText = new TextBlock
            {
                Text = $"Роль: {user.RoleName}",
                Foreground = System.Windows.Media.Brushes.Gray
            };
            stackPanel.Children.Add(nameText);
            stackPanel.Children.Add(emailText);
            stackPanel.Children.Add(roleText);

            // Кнопки дій
            var actionPanel = new StackPanel { Orientation = Orientation.Horizontal };
            var editButton = new Button
            {
                Style = (Style)FindResource("MaterialDesignIconButton"),
                ToolTip = "Редагувати",
                Tag = user.Id,
                Content = new MaterialDesignThemes.Wpf.PackIcon { Kind = MaterialDesignThemes.Wpf.PackIconKind.Edit }
            };
            editButton.Click += EditUser_Click;

            var deleteButton = new Button
            {
                Style = (Style)FindResource("MaterialDesignIconButton"),
                ToolTip = "Видалити",
                Tag = user.Id,
                Content = new MaterialDesignThemes.Wpf.PackIcon { Kind = MaterialDesignThemes.Wpf.PackIconKind.Delete },
                Foreground = System.Windows.Media.Brushes.HotPink
            };
            deleteButton.Click += DeleteUser_Click;

            actionPanel.Children.Add(editButton);
            actionPanel.Children.Add(deleteButton);

            // Додаємо елементи до сітки
            grid.Children.Add(icon);
            Grid.SetColumn(icon, 0);

            grid.Children.Add(stackPanel);
            Grid.SetColumn(stackPanel, 1);

            grid.Children.Add(actionPanel);
            Grid.SetColumn(actionPanel, 2);

            border.Child = grid;
            UsersStackPanel.Children.Add(border);
        }

        private async void EditUser_Click(object sender, RoutedEventArgs e)
        {
            var userId = (int)((Button)sender).Tag;
            try
            {
                var user = await App.ApiClient.GetAsync<UserDto>($"/users/{userId}");
                var editWindow = new EditUserWindow(user);
                if (editWindow.ShowDialog() == true)
                {
                    await App.ApiClient.PutAsync<UserDto>($"/users/{userId}", editWindow.UpdatedUser);
                    LoadUsers(); // Оновити список
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка редагування користувача: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            var userId = (int)((Button)sender).Tag;
            var result = MessageBox.Show("Ви впевнені, що хочете видалити цього користувача?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await App.ApiClient.DeleteAsync($"/users/{userId}");
                    LoadUsers(); // Оновити список
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка видалення користувача: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddUserWindow();
            if (addWindow.ShowDialog() == true)
            {
                try
                {
                    await App.ApiClient.PostAsync<UserDto>("/users/", addWindow.NewUser);
                    LoadUsers(); // Оновити список
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка додавання користувача: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public class UserDto
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("first")]
            public string First { get; set; }

            [JsonPropertyName("second")]
            public string Second { get; set; }

            [JsonPropertyName("email")]
            public string Email { get; set; }
            [JsonPropertyName("role")]
            public int Role { get; set; }
            [JsonPropertyName("role_name")]
            public string RoleName { get; set; }
        }
    }
}