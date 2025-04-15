using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Windowing;
using Windows.Graphics;

namespace WinUi3
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            // Встановлюємо розміри вікна
            var appWindow = GetAppWindowForCurrentWindow();
            if (appWindow != null)
            {
                appWindow.Resize(new SizeInt32(1200, 800)); // Збільшені розміри вікна
            }
        }

        private AppWindow GetAppWindowForCurrentWindow()
        {
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(windowId);
        }

        private void CareMethodsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CareMethodsListBox.SelectedItem is ListBoxItem selectedItem)
            {
                switch (selectedItem.Content.ToString())
                {
                    case "Поливка":
                        DescriptionTextBlock.Text = "Поливайте рослину, коли верхній шар ґрунту стає сухим на дотик.";
                        break;
                    case "Добриво":
                        DescriptionTextBlock.Text = "Використовуйте збалансоване добриво раз на місяць у період росту.";
                        break;
                    case "Обрізка":
                        DescriptionTextBlock.Text = "Видаляйте сухе або жовте листя, щоб стимулювати новий ріст.";
                        break;
                    case "Пересадка":
                        DescriptionTextBlock.Text = "Пересаджуйте рослину кожні 1-2 роки, щоб оновити ґрунт.";
                        break;
                    default:
                        DescriptionTextBlock.Text = "Оберіть метод догляду, щоб побачити опис.";
                        break;
                }
            }
        }
    }
}
