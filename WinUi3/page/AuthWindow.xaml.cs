using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;

namespace WinUi3
{
    public sealed partial class AuthWindow : Window
    {
        private readonly string UsersFile = Path.Combine(AppContext.BaseDirectory, "users.txt"); // ���� ��� ���������� ������������ � ����� ������
        private Dictionary<string, string> users = new(); // ������� ��� ���������� ����� � ������

        public AuthWindow()
        {
            this.InitializeComponent(); // ����������� ���������� �� XAML
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
                            users[parts[0]] = parts[1]; // ������ ���� � ������ � �������
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
                    lines.Add($"{user.Key}:{user.Value}"); // ������ ����:������
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
                var login = LoginTextBox.Text; // ��������� ������ � LoginTextBox
                var password = PasswordBox.Password; // ��������� ������ � PasswordBox

                if (users.ContainsKey(login) && users[login] == password)
                {
                    // ������� ����
                    var mainWindow = new MainWindow();
                    mainWindow.Activate();
                    this.Close();
                }
                else
                {
                    ContentDialog dialog = new()
                    {
                        Title = "�������",
                        Content = "������� ���� ��� ������.",
                        CloseButtonText = "��",
                        XamlRoot = this.Content.XamlRoot
                    };
                    _ = dialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog($"������� �� ��� �����: {ex.Message}");
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var login = LoginTextBox.Text; // ��������� ������ � LoginTextBox
                var password = PasswordBox.Password; // ��������� ������ � PasswordBox

                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                {
                    ContentDialog dialog = new()
                    {
                        Title = "�������",
                        Content = "���� � ������ �� ������ ���� ��������.",
                        CloseButtonText = "��",
                        XamlRoot = this.Content.XamlRoot
                    };
                    _ = dialog.ShowAsync();
                    return;
                }

                if (users.ContainsKey(login))
                {
                    ContentDialog dialog = new()
                    {
                        Title = "�������",
                        Content = "���������� � ����� ������ ��� ����.",
                        CloseButtonText = "��",
                        XamlRoot = this.Content.XamlRoot
                    };
                    _ = dialog.ShowAsync();
                }
                else
                {
                    users[login] = password; // ������ ������ �����������
                    SaveUsers(); // �������� ������������ � ����

                    ContentDialog dialog = new()
                    {
                        Title = "����",
                        Content = "��������� ������! ����� �� ������ �����.",
                        CloseButtonText = "��",
                        XamlRoot = this.Content.XamlRoot
                    };
                    _ = dialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog($"������� �� ��� ���������: {ex.Message}");
            }
        }

        private async void ShowErrorDialog(string message)
        {
            try
            {
                ContentDialog errorDialog = new()
                {
                    Title = "�������� �������",
                    Content = message,
                    CloseButtonText = "��",
                    XamlRoot = this.Content.XamlRoot
                };

                // ��������� �����
                await errorDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                // ��������� ��� ������� �������
                System.Diagnostics.Debug.WriteLine($"������� �� ��� ������ ������: {ex.Message}");
            }
        }
    }
}

