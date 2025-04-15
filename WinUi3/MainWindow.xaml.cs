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

            // ������������ ������ ����
            var appWindow = GetAppWindowForCurrentWindow();
            if (appWindow != null)
            {
                appWindow.Resize(new SizeInt32(1200, 800)); // ������� ������ ����
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
        }
    }
}
