using Microsoft.UI.Xaml;

namespace WinUi3
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            var authWindow = new AuthWindow();
            authWindow.Activate();
        }
    }
}
