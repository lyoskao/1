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
        // ��������� ��� ������� ����: false = ���������, true = ���������
        private bool _isEnglish = false;

        public MainWindow()
        {
            this.InitializeComponent();

            // ������������ ������ ���� � ������������� AppWindow
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
                        case "�������":
                            DescriptionTextBlock.Text = "��������� �������, ���� ������ ��� ������ ��� ����� �� �����.";
                            break;
                        case "�������":
                            DescriptionTextBlock.Text = "�������������� ������������ ������� ��� �� ����� � ����� �����.";
                            break;
                        case "������":
                            DescriptionTextBlock.Text = "��������� ���� ��� ����� �����, ��� ����������� ����� ���.";
                            break;
                        case "���������":
                            DescriptionTextBlock.Text = "������������ ������� ���� 1-2 ����, ��� ������� �����.";
                            break;
                        default:
                            DescriptionTextBlock.Text = "������ ����� �������, ��� �������� ����.";
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

        // �������� ���������� ������ ����������� ����
        private async void LangButton_Click(object sender, RoutedEventArgs e)
        {
            // �������� Storyboard � ������� ���������� ��������, ���� ����������� � RootGrid
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
                this.Title = "������ �� ���������";
                DescriptionTextBlock.Text = "������ ����� �������, ��� �������� ����.";
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
                        case "�������":
                            lbItem.Content = "Watering";
                            break;
                        case "�������":
                            lbItem.Content = "Fertilizer";
                            break;
                        case "������":
                            lbItem.Content = "Pruning";
                            break;
                        case "���������":
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
                            lbItem.Content = "�������";
                            break;
                        case "Fertilizer":
                            lbItem.Content = "�������";
                            break;
                        case "Pruning":
                            lbItem.Content = "������";
                            break;
                        case "Repotting":
                            lbItem.Content = "���������";
                            break;
                    }
                }
            }
        }
    }
}
