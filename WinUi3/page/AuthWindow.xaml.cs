using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;

namespace WinUi3
{
    public sealed partial class AuthWindow : Window
    {
        private readonly string UsersFile = Path.Combine(AppContext.BaseDirectory, "users.txt"); // Файл для збереження користувачів у папці проєкту
        private Dictionary<string, string> users = new(); // Словник для збереження логінів і паролів

        public AuthWindow()
        {
            this.InitializeComponent(); // Ініціалізація компонентів із XAML
            LoadUsers(); // Завантаження користувачів із файлу
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
                            users[parts[0]] = parts[1]; // Додаємо логін і пароль у словник
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog($"Помилка завантаження користувачів: {ex.Message}");
            }
        }

        private void SaveUsers()
        {
            try
            {
                var lines = new List<string>();
                foreach (var user in users)
                {
                    lines.Add($"{user.Key}:{user.Value}"); // Формат логін:пароль
                }
                File.WriteAllLines(UsersFile, lines);
            }
            catch (Exception ex)
            {
                ShowErrorDialog($"Помилка збереження користувачів: {ex.Message}");
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var login = LoginTextBox.Text; // Отримання тексту з LoginTextBox
                var password = PasswordBox.Password; // Отримання пароля з PasswordBox

                if (users.ContainsKey(login) && users[login] == password)
                {
                    // Успішний вхід
                    var mainWindow = new MainWindow();
                    mainWindow.Activate();
                    this.Close();
                }
                else
                {
                    ContentDialog dialog = new()
                    {
                        Title = "Помилка",
                        Content = "Невірний логін або пароль.",
                        CloseButtonText = "ОК",
                        XamlRoot = this.Content.XamlRoot
                    };
                    _ = dialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog($"Помилка під час входу: {ex.Message}");
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var login = LoginTextBox.Text; // Отримання тексту з LoginTextBox
                var password = PasswordBox.Password; // Отримання пароля з PasswordBox

                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                {
                    ContentDialog dialog = new()
                    {
                        Title = "Помилка",
                        Content = "Логін і пароль не можуть бути порожніми.",
                        CloseButtonText = "ОК",
                        XamlRoot = this.Content.XamlRoot
                    };
                    _ = dialog.ShowAsync();
                    return;
                }

                if (users.ContainsKey(login))
                {
                    ContentDialog dialog = new()
                    {
                        Title = "Помилка",
                        Content = "Користувач з таким логіном вже існує.",
                        CloseButtonText = "ОК",
                        XamlRoot = this.Content.XamlRoot
                    };
                    _ = dialog.ShowAsync();
                }
                else
                {
                    users[login] = password; // Додаємо нового користувача
                    SaveUsers(); // Зберігаємо користувачів у файл

                    ContentDialog dialog = new()
                    {
                        Title = "Успіх",
                        Content = "Реєстрація успішна! Тепер ви можете увійти.",
                        CloseButtonText = "ОК",
                        XamlRoot = this.Content.XamlRoot
                    };
                    _ = dialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog($"Помилка під час реєстрації: {ex.Message}");
            }
        }

        private async void ShowErrorDialog(string message)
        {
            try
            {
                ContentDialog errorDialog = new()
                {
                    Title = "Критична помилка",
                    Content = message,
                    CloseButtonText = "ОК",
                    XamlRoot = this.Content.XamlRoot
                };

                // Викликаємо діалог
                await errorDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                // Логування або обробка винятку
                System.Diagnostics.Debug.WriteLine($"Помилка під час показу діалогу: {ex.Message}");
            }
        }
    }
}

