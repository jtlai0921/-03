using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Syndication;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MyApp
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

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            SyndicationItem feeditem = e.ClickedItem as SyndicationItem;
            if (feeditem != null)
            {
                // 若果Content屬性為null，則取得Summary屬性的內容
                if (feeditem.Content == null)
                {
                    wv.NavigateToString(feeditem.Summary.Text);
                }
                else
                {
                    wv.NavigateToString(feeditem.Content.Text);
                }
            }
        }

        private async void OnGetdata(object sender, RoutedEventArgs e)
        {
            if (txtUri.Text.Length == 0) return;

            Button button = sender as Button;
            button.IsEnabled = false;

            SyndicationClient client = new SyndicationClient();
            // 開始請求RSS資源
            SyndicationFeed feed = await client.RetrieveFeedAsync(new Uri(txtUri.Text.Trim()));
            if (feed != null)
            {
                this.lvItems.ItemsSource = feed.Items;
            }

            button.IsEnabled = true;
        }
    }
}
