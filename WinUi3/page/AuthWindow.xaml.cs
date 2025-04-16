using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace WinUi3
{
    public sealed partial class AuthWindow : Window
    {
        // ���� ��� ���������� ������������ (users.txt) ���������� � �������� ������������ �����
        private readonly string UsersFile = Path.Combine(AppContext.BaseDirectory, "users.txt");
        // ������� ��� ���������� ����� � ������
        private Dictionary<string, string> users = new();

        // ���������, �� ������� ������� ����: false = ���������, true = ���������
        private bool _isEnglish = false;

        public AuthWindow()
        {
            this.InitializeComponent(); // ����������� ���������� � XAML
            LoadUsers(); // ������������ ������������ �� �����
        }

        private void LoadUsers()
        {
            try
            {
                if (File.Exists(UsersFile))
                {
                    var lines = File.ReadAllLines(UsersFile);
                    foreach (var line in lines)
                    {
                        var parts = line.Split(':');
                        if (parts.Length == 2)
                        {
                            users[parts[0]] = parts[1];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog($"������� ������������ ������������: {ex.Message}");
            }
        }

        private void SaveUsers()
        {
            try
            {
                var lines = new List<string>();
                foreach (var user in users)
                {
                    lines.Add($"{user.Key}:{user.Value}");
                }
                File.WriteAllLines(UsersFile, lines);
            }
            catch (Exception ex)
            {
                ShowErrorDialog($"������� ���������� ������������: {ex.Message}");
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var login = LoginTextBox.Text;
                var password = PasswordBox.Password;

                if (users.ContainsKey(login) && users[login] == password)
                {
                    // ������� ���� � �������� ��������� ����
                    var mainWindow = new MainWindow();
                    mainWindow.Activate();
                    this.Close();
                }
                else
                {
                    ContentDialog dialog = new()
                    {
                        Title = _isEnglish ? "Error" : "�������",
                        Content = _isEnglish ? "Incorrect login or password." : "������� ���� ��� ������.",
                        CloseButtonText = _isEnglish ? "OK" : "��",
                        XamlRoot = this.Content.XamlRoot
                    };
                    _ = dialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog(_isEnglish ? $"Error during login: {ex.Message}" : $"������� �� ��� �����: {ex.Message}");
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var login = LoginTextBox.Text;
                var password = PasswordBox.Password;

                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                {
                    ContentDialog dialog = new()
                    {
                        Title = _isEnglish ? "Error" : "�������",
                        Content = _isEnglish ? "Username and password cannot be empty." : "���� � ������ �� ������ ���� ��������.",
                        CloseButtonText = _isEnglish ? "OK" : "��",
                        XamlRoot = this.Content.XamlRoot
                    };
                    _ = dialog.ShowAsync();
                    return;
                }

                if (users.ContainsKey(login))
                {
                    ContentDialog dialog = new()
                    {
                        Title = _isEnglish ? "Error" : "�������",
                        Content = _isEnglish ? "User already exists." : "���������� � ����� ������ ��� ����.",
                        CloseButtonText = _isEnglish ? "OK" : "��",
                        XamlRoot = this.Content.XamlRoot
                    };
                    _ = dialog.ShowAsync();
                }
                else
                {
                    users[login] = password;
                    SaveUsers();
                    ContentDialog dialog = new()
                    {
                        Title = _isEnglish ? "Success" : "����",
                        Content = _isEnglish ? "Registration successful! You can now log in." : "��������� ������! ����� �� ������ �����.",
                        CloseButtonText = _isEnglish ? "OK" : "��",
                        XamlRoot = this.Content.XamlRoot
                    };
                    _ = dialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog(_isEnglish ? $"Error during registration: {ex.Message}" : $"������� �� ��� ���������: {ex.Message}");
            }
        }

        // �������� ���������� ������ ����������� ����
        private async void LangButton_Click(object sender, RoutedEventArgs e)
        {
            // �������� Storyboard � ������� ���������� �������� (RootGrid)
            Storyboard sb = (Storyboard)((FrameworkElement)Content).Resources["FadeOutInStoryboard"];
            sb.Begin();

            // ������� 300 �� (��������� �������)
            await Task.Delay(300);

            // ���������� ����
            _isEnglish = !_isEnglish;
            if (_isEnglish)
            {
                // ��������� ����������
                this.Title = "Authentication";
                LoginLabel.Text = "Username";
                PasswordLabel.Text = "Password";
                LoginTextBox.PlaceholderText = "Enter username";
                PasswordBox.PlaceholderText = "Enter password";
                LoginBtn.Content = "Login";
                RegisterBtn.Content = "Register";
                LangButton.Content = "UA"; // ��� ����������� ����� �� ���������
            }
            else
            {
                // ��������� ����������
                this.Title = "�����������";
                LoginLabel.Text = "����";
                PasswordLabel.Text = "������";
                LoginTextBox.PlaceholderText = "������ ����";
                PasswordBox.PlaceholderText = "������ ������";
                LoginBtn.Content = "�����";
                RegisterBtn.Content = "��������������";
                LangButton.Content = "EN"; // ��� ����������� �� ���������
            }
        }

        // ����� ��� ����������� ���������� ��� �������
        private async void ShowErrorDialog(string message)
        {
            try
            {
                ContentDialog errorDialog = new()
                {
                    Title = _isEnglish ? "Critical Error" : "�������� �������",
                    Content = message,
                    CloseButtonText = _isEnglish ? "OK" : "��",
                    XamlRoot = this.Content.XamlRoot
                };

                await errorDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"������� �� ��� ������ ������: {ex.Message}");
            }
        }
    }
}
