using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using System;
using System.Threading.Tasks;

namespace WinUi3
{
    public sealed partial class MainWindow : Window
    {
        // Прапорець для поточної мови: false = українська, true = англійська
        private bool _isEnglish = false;

        public MainWindow()
        {
            this.InitializeComponent();

            // Встановлення розмірів вікна з використанням AppWindow
            var appWindow = GetAppWindowForCurrentWindow();
            if (appWindow != null)
            {
                appWindow.Resize(new SizeInt32(1200, 800));
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
                string content = selectedItem.Content.ToString();
                if (!_isEnglish)
                {
                    switch (content)
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
                else
                {
                    switch (content)
                    {
                        case "Watering":
                            DescriptionTextBlock.Text = "Water the plant when the top soil feels dry to the touch.";
                            break;
                        case "Fertilizer":
                            DescriptionTextBlock.Text = "Use balanced fertilizer once a month during the growing season.";
                            break;
                        case "Pruning":
                            DescriptionTextBlock.Text = "Remove dry or yellow leaves to encourage new growth.";
                            break;
                        case "Repotting":
                            DescriptionTextBlock.Text = "Repot the plant every 1-2 years to refresh the soil.";
                            break;
                        default:
                            DescriptionTextBlock.Text = "Select a care method to view its description.";
                            break;
                    }
                }
            }
        }

        // Обробник натискання кнопки перемикання мови
        private async void LangButton_Click(object sender, RoutedEventArgs e)
        {
            // Отримуємо Storyboard з ресурсів кореневого елемента, який знаходиться у RootGrid
            Storyboard sb = (Storyboard)RootGrid.Resources["FadeOutInStoryboard"];
            sb.Begin();
            await Task.Delay(300);

            _isEnglish = !_isEnglish;
            if (_isEnglish)
            {
                this.Title = "Plant Care";
                DescriptionTextBlock.Text = "Select a care method to view its description.";
                LangButton.Content = "UA";
                UpdateListBoxItemsToEnglish();
            }
            else
            {
                this.Title = "Догляд за рослинами";
                DescriptionTextBlock.Text = "Оберіть метод догляду, щоб побачити опис.";
                LangButton.Content = "EN";
                UpdateListBoxItemsToUkrainian();
            }
        }

        private void UpdateListBoxItemsToEnglish()
        {
            foreach (var item in CareMethodsListBox.Items)
            {
                if (item is ListBoxItem lbItem)
                {
                    switch (lbItem.Content.ToString())
                    {
                        case "Поливка":
                            lbItem.Content = "Watering";
                            break;
                        case "Добриво":
                            lbItem.Content = "Fertilizer";
                            break;
                        case "Обрізка":
                            lbItem.Content = "Pruning";
                            break;
                        case "Пересадка":
                            lbItem.Content = "Repotting";
                            break;
                    }
                }
            }
        }

        private void UpdateListBoxItemsToUkrainian()
        {
            foreach (var item in CareMethodsListBox.Items)
            {
                if (item is ListBoxItem lbItem)
                {
                    switch (lbItem.Content.ToString())
                    {
                        case "Watering":
                            lbItem.Content = "Поливка";
                            break;
                        case "Fertilizer":
                            lbItem.Content = "Добриво";
                            break;
                        case "Pruning":
                            lbItem.Content = "Обрізка";
                            break;
                        case "Repotting":
                            lbItem.Content = "Пересадка";
                            break;
                    }
                }
            }
        }
    }
}
