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
        // Файл для збереження користувачів (users.txt) зберігається в директорії виконуваного файлу
        private readonly string UsersFile = Path.Combine(AppContext.BaseDirectory, "users.txt");
        // Словник для збереження логінів і паролів
        private Dictionary<string, string> users = new();

        // Прапорець, що визначає поточну мову: false = українська, true = англійська
        private bool _isEnglish = false;

        public AuthWindow()
        {
            this.InitializeComponent(); // Ініціалізація компонентів з XAML
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
                            users[parts[0]] = parts[1];
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
                    lines.Add($"{user.Key}:{user.Value}");
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
                var login = LoginTextBox.Text;
                var password = PasswordBox.Password;

                if (users.ContainsKey(login) && users[login] == password)
                {
                    // Успішний вхід — відкриття головного вікна
                    var mainWindow = new MainWindow();
                    mainWindow.Activate();
                    this.Close();
                }
                else
                {
                    ContentDialog dialog = new()
                    {
                        Title = _isEnglish ? "Error" : "Помилка",
                        Content = _isEnglish ? "Incorrect login or password." : "Невірний логін або пароль.",
                        CloseButtonText = _isEnglish ? "OK" : "ОК",
                        XamlRoot = this.Content.XamlRoot
                    };
                    _ = dialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog(_isEnglish ? $"Error during login: {ex.Message}" : $"Помилка під час входу: {ex.Message}");
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
                        Title = _isEnglish ? "Error" : "Помилка",
                        Content = _isEnglish ? "Username and password cannot be empty." : "Логін і пароль не можуть бути порожніми.",
                        CloseButtonText = _isEnglish ? "OK" : "ОК",
                        XamlRoot = this.Content.XamlRoot
                    };
                    _ = dialog.ShowAsync();
                    return;
                }

                if (users.ContainsKey(login))
                {
                    ContentDialog dialog = new()
                    {
                        Title = _isEnglish ? "Error" : "Помилка",
                        Content = _isEnglish ? "User already exists." : "Користувач з таким логіном вже існує.",
                        CloseButtonText = _isEnglish ? "OK" : "ОК",
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
                        Title = _isEnglish ? "Success" : "Успіх",
                        Content = _isEnglish ? "Registration successful! You can now log in." : "Реєстрація успішна! Тепер ви можете увійти.",
                        CloseButtonText = _isEnglish ? "OK" : "ОК",
                        XamlRoot = this.Content.XamlRoot
                    };
                    _ = dialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog(_isEnglish ? $"Error during registration: {ex.Message}" : $"Помилка під час реєстрації: {ex.Message}");
            }
        }

        // Обробник натискання кнопки перемикання мови
        private async void LangButton_Click(object sender, RoutedEventArgs e)
        {
            // Отримуємо Storyboard з ресурсів кореневого елемента (RootGrid)
            Storyboard sb = (Storyboard)((FrameworkElement)Content).Resources["FadeOutInStoryboard"];
            sb.Begin();

            // Очікуємо 300 мс (тривалість анімації)
            await Task.Delay(300);

            // Перемикаємо мову
            _isEnglish = !_isEnglish;
            if (_isEnglish)
            {
                // Англійська локалізація
                this.Title = "Authentication";
                LoginLabel.Text = "Username";
                PasswordLabel.Text = "Password";
                LoginTextBox.PlaceholderText = "Enter username";
                PasswordBox.PlaceholderText = "Enter password";
                LoginBtn.Content = "Login";
                RegisterBtn.Content = "Register";
                LangButton.Content = "UA"; // Для перемикання назад на українську
            }
            else
            {
                // Українська локалізація
                this.Title = "Авторизація";
                LoginLabel.Text = "Логін";
                PasswordLabel.Text = "Пароль";
                LoginTextBox.PlaceholderText = "Введіть логін";
                PasswordBox.PlaceholderText = "Введіть пароль";
                LoginBtn.Content = "Увійти";
                RegisterBtn.Content = "Зареєструватися";
                LangButton.Content = "EN"; // Для перемикання на англійську
            }
        }

        // Метод для відображення повідомлень про помилки
        private async void ShowErrorDialog(string message)
        {
            try
            {
                ContentDialog errorDialog = new()
                {
                    Title = _isEnglish ? "Critical Error" : "Критична помилка",
                    Content = message,
                    CloseButtonText = _isEnglish ? "OK" : "ОК",
                    XamlRoot = this.Content.XamlRoot
                };

                await errorDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Помилка під час показу діалогу: {ex.Message}");
            }
        }
    }
}
