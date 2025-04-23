using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace AdminPanel
{
    public class AuthService
    {
        private readonly ApiClient _apiClient;
        private const string LoginEndpoint = "/token";

        public AuthService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<string> Login(string username, string password)
        {
            try
            {
                var loginData = new
                {
                    username = username,
                    password = password,
                    grant_type = "password"
                };

                var json = JsonSerializer.Serialize(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _apiClient.PostAsync<TokenResponse>(LoginEndpoint, loginData);
                return response.AccessToken;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка авторизації: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        private class TokenResponse
        {
            public string AccessToken { get; set; }
            public string TokenType { get; set; }
        }
    }
}