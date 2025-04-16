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
        // ���� ��� ���������� ������������ (users.txt) ����������� � �������� ������������ �����
        private readonly string UsersFile = Path.Combine(AppContext.BaseDirectory, "users.txt");
        // ������� ��� ���������� ����� �� ������
        private Dictionary<string, string> users = new();
        // _isEnglish: false = ���������, true = ���������
        private bool _isEnglish = false;
        // _isRegisterMode: false = ����� "����", true = ����� "���������"
        private bool _isRegisterMode = false;

        public AuthWindow()
        {
            InitializeComponent(); // ����������� ���������� � XAML
            LoadUsers();         // ������������ ������������ �� �����
            UpdateUIForMode();   // ��������� ���������� ������� �� ������
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
                ShowErrorDialog(_isEnglish ?
                    $"Error loading users: {ex.Message}" :
                    $"������� ������������ ������������: {ex.Message}");
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
                ShowErrorDialog(_isEnglish ?
                    $"Error saving users: {ex.Message}" :
                    $"������� ���������� ������������: {ex.Message}");
            }
        }

        // �������� ��� ������ 䳿 (Login ��� Register ������� �� ������)
        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isRegisterMode)
                Register();
            else
                Login();
        }

        private void Login()
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
                ShowErrorDialog(_isEnglish ?
                    $"Error during login: {ex.Message}" :
                    $"������� �� ��� �����: {ex.Message}");
            }
        }

        private void Register()
        {
            try
            {
                var login = LoginTextBox.Text;
                var password = PasswordBox.Password;
                var confirmPassword = ConfirmPasswordBox.Password;

                if (string.IsNullOrWhiteSpace(login) ||
                    string.IsNullOrWhiteSpace(password) ||
                    string.IsNullOrWhiteSpace(confirmPassword))
                {
                    ContentDialog dialog = new()
                    {
                        Title = _isEnglish ? "Error" : "�������",
                        Content = _isEnglish ?
                                  "Username and passwords cannot be empty." :
                                  "���� � ����� �� ������ ���� ��������.",
                        CloseButtonText = _isEnglish ? "OK" : "��",
                        XamlRoot = this.Content.XamlRoot
                    };
                    _ = dialog.ShowAsync();
                    return;
                }

                if (password != confirmPassword)
                {
                    ContentDialog dialog = new()
                    {
                        Title = _isEnglish ? "Error" : "�������",
                        Content = _isEnglish ?
                                  "Passwords do not match." :
                                  "����� �� ����������.",
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
                        Content = _isEnglish ?
                                  "User already exists." :
                                  "���������� � ����� ������ ��� ����.",
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
                        Content = _isEnglish ?
                                  "Registration successful! You can now log in." :
                                  "��������� ������! ����� �� ������ �����.",
                        CloseButtonText = _isEnglish ? "OK" : "��",
                        XamlRoot = this.Content.XamlRoot
                    };
                    _ = dialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog(_isEnglish ?
                    $"Error during registration: {ex.Message}" :
                    $"������� �� ��� ���������: {ex.Message}");
            }
        }

        // �������� ������ ����������� ������ ("����" / "���������")
        private void SwitchModeButton_Click(object sender, RoutedEventArgs e)
        {
            _isRegisterMode = !_isRegisterMode;
            UpdateUIForMode();
        }

        // ����� ��������� ���������� ������� �� ������
        private void UpdateUIForMode()
        {
            if (_isRegisterMode)
            {
                RegisterPanel.Visibility = Visibility.Visible;
                ActionButton.Content = _isEnglish ? "Register" : "��������������";
                SwitchModeButton.Content = _isEnglish ? "Login" : "����";
            }
            else
            {
                RegisterPanel.Visibility = Visibility.Collapsed;
                ActionButton.Content = _isEnglish ? "Login" : "�����";
                SwitchModeButton.Content = _isEnglish ? "Register" : "���������";
            }
        }

        // �������� ����������� ����
        private async void LangButton_Click(object sender, RoutedEventArgs e)
        {
            // ������ ������� fade-out/fade-in
            Storyboard sb = (Storyboard)((FrameworkElement)Content).Resources["FadeOutInStoryboard"];
            sb.Begin();
            await Task.Delay(300);

            _isEnglish = !_isEnglish;
            if (_isEnglish)
            {
                this.Title = "Authentication";
                LoginLabel.Text = "Username";
                PasswordLabel.Text = "Password";
                LoginTextBox.PlaceholderText = "Enter username";
                PasswordBox.PlaceholderText = "Enter password";
                ConfirmPasswordLabel.Text = "Confirm Password";
                ConfirmPasswordBox.PlaceholderText = "Re-enter password";
                if (_isRegisterMode)
                    ActionButton.Content = "Register";
                else
                    ActionButton.Content = "Login";
                SwitchModeButton.Content = _isRegisterMode ? "Login" : "Register";
                LangButton.Content = "UA";
            }
            else
            {
                this.Title = "�����������";
                LoginLabel.Text = "����";
                PasswordLabel.Text = "������";
                LoginTextBox.PlaceholderText = "������ ����";
                PasswordBox.PlaceholderText = "������ ������";
                ConfirmPasswordLabel.Text = "ϳ����������� ������";
                ConfirmPasswordBox.PlaceholderText = "ϳ�������� ������";
                if (_isRegisterMode)
                    ActionButton.Content = "��������������";
                else
                    ActionButton.Content = "�����";
                SwitchModeButton.Content = _isRegisterMode ? "����" : "���������";
                LangButton.Content = "EN";
            }
        }

        // ����� ����������� ���������� ��� �������
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
