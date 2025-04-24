using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using static AdminPanel.UsersPage;

namespace AdminPanel
{
    public class AuthService
    {
        private readonly ApiClient _apiClient;
        private const string LoginEndpoint = "/token";

        public AuthService(ApiClient apiClient)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        }

        public async Task<string> Login(string username, string password)
        {
            try
            {
                // Підготовка даних для x-www-form-urlencoded
                var formData = new Dictionary<string, string>
                {
                    {"grant_type", "password"},
                    {"username", username},
                    {"password", password}
                    // Додаткові параметри (якщо потрібні):
                    // {"scope", ""},
                    // {"client_id", ""},
                    // {"client_secret", ""}
                };

                // Спеціальний метод тільки для логіну
                var response = await _apiClient.PostFormUrlEncodedAsync<TokenResponse>(LoginEndpoint, formData);

                if (response?.AccessToken == null)
                {
                    MessageBox.Show("Invalid server response", "Error",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
                App.ApiClient.SetAuthToken(response.AccessToken);
                CurrentUser = await _apiClient.GetAsync<UserDto>("/users/me/");
                return response.AccessToken;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login failed: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public UserDto CurrentUser { get; private set; }

        private class TokenResponse
        {
            [JsonPropertyName("access_token")]
            public string AccessToken { get; set; }
            [JsonPropertyName("token_type")]
            public string TokenType { get; set; }
        }
    }
}