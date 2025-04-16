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
        // Файл для збереження користувачів (users.txt) знаходиться у директорії виконуваного файлу
        private readonly string UsersFile = Path.Combine(AppContext.BaseDirectory, "users.txt");
        // Словник для збереження логінів та паролів
        private Dictionary<string, string> users = new();
        // _isEnglish: false = українська, true = англійська
        private bool _isEnglish = false;
        // _isRegisterMode: false = режим "Вхід", true = режим "Реєстрація"
        private bool _isRegisterMode = false;

        public AuthWindow()
        {
            InitializeComponent(); // Ініціалізація компонентів з XAML
            LoadUsers();         // Завантаження користувачів із файлу
            UpdateUIForMode();   // Оновлення інтерфейсу залежно від режиму
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
                    $"Помилка завантаження користувачів: {ex.Message}");
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
                    $"Помилка збереження користувачів: {ex.Message}");
            }
        }

        // Обробник для кнопки дії (Login або Register залежно від режиму)
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
                ShowErrorDialog(_isEnglish ?
                    $"Error during login: {ex.Message}" :
                    $"Помилка під час входу: {ex.Message}");
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
                        Title = _isEnglish ? "Error" : "Помилка",
                        Content = _isEnglish ?
                                  "Username and passwords cannot be empty." :
                                  "Логін і паролі не можуть бути порожніми.",
                        CloseButtonText = _isEnglish ? "OK" : "ОК",
                        XamlRoot = this.Content.XamlRoot
                    };
                    _ = dialog.ShowAsync();
                    return;
                }

                if (password != confirmPassword)
                {
                    ContentDialog dialog = new()
                    {
                        Title = _isEnglish ? "Error" : "Помилка",
                        Content = _isEnglish ?
                                  "Passwords do not match." :
                                  "Паролі не співпадають.",
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
                        Content = _isEnglish ?
                                  "User already exists." :
                                  "Користувач з таким логіном вже існує.",
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
                        Content = _isEnglish ?
                                  "Registration successful! You can now log in." :
                                  "Реєстрація успішна! Тепер ви можете увійти.",
                        CloseButtonText = _isEnglish ? "OK" : "ОК",
                        XamlRoot = this.Content.XamlRoot
                    };
                    _ = dialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog(_isEnglish ?
                    $"Error during registration: {ex.Message}" :
                    $"Помилка під час реєстрації: {ex.Message}");
            }
        }

        // Обробник кнопки перемикання режиму ("Вхід" / "Реєстрація")
        private void SwitchModeButton_Click(object sender, RoutedEventArgs e)
        {
            _isRegisterMode = !_isRegisterMode;
            UpdateUIForMode();
        }

        // Метод оновлення інтерфейсу залежно від режиму
        private void UpdateUIForMode()
        {
            if (_isRegisterMode)
            {
                RegisterPanel.Visibility = Visibility.Visible;
                ActionButton.Content = _isEnglish ? "Register" : "Зареєструватися";
                SwitchModeButton.Content = _isEnglish ? "Login" : "Вхід";
            }
            else
            {
                RegisterPanel.Visibility = Visibility.Collapsed;
                ActionButton.Content = _isEnglish ? "Login" : "Увійти";
                SwitchModeButton.Content = _isEnglish ? "Register" : "Реєстрація";
            }
        }

        // Обробник перемикання мови
        private async void LangButton_Click(object sender, RoutedEventArgs e)
        {
            // Запуск анімації fade-out/fade-in
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
                this.Title = "Авторизація";
                LoginLabel.Text = "Логін";
                PasswordLabel.Text = "Пароль";
                LoginTextBox.PlaceholderText = "Введіть логін";
                PasswordBox.PlaceholderText = "Введіть пароль";
                ConfirmPasswordLabel.Text = "Підтвердження паролю";
                ConfirmPasswordBox.PlaceholderText = "Підтвердіть пароль";
                if (_isRegisterMode)
                    ActionButton.Content = "Зареєструватися";
                else
                    ActionButton.Content = "Увійти";
                SwitchModeButton.Content = _isRegisterMode ? "Вхід" : "Реєстрація";
                LangButton.Content = "EN";
            }
        }

        // Метод відображення повідомлень про помилки
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
