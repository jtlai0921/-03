using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Navigation;

namespace TestServiceApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            runPackageName.Text = Windows.ApplicationModel.Package.Current.Id.FamilyName;
        }

        private void OnCopy(object sender, RoutedEventArgs e)
        {
            DataPackage package = new DataPackage();
            package.SetText(runPackageName.Text);
            Clipboard.SetContent(package);
        }
    }
}
