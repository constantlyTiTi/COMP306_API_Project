using apiFrontEnd.Models;
using apiFrontEnd.StaticValues;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace apiFrontEnd
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string _userName;
        private readonly string _token;
        public MainWindow()
        {
            InitializeComponent();
            ItemNav.Visibility = Visibility.Hidden;
            OrderNav.Visibility = Visibility.Hidden;
            LogoutNav.Visibility = Visibility.Hidden;
            _token = string.Empty;
            _userName = string.Empty;
        }

        public MainWindow(string userName, string token)
        {
            InitializeComponent();
            _userName = userName;
            _token = token;
            AuthNav.Content = "Hello, " + _userName;
            AuthNav.IsEnabled = false;
        }

        private void AuthNav_Click(object sender, RoutedEventArgs e)
        {
            LoginAndRegistration lrw = new LoginAndRegistration();
            lrw.Show();
            this.Close();
        }

        private void HomeNav_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_token))
            {
                ItemNav.Visibility = Visibility.Hidden;
                OrderNav.Visibility = Visibility.Hidden;
            }
            else
            {
                ItemNav.Visibility = Visibility.Visible;
                OrderNav.Visibility = Visibility.Visible;
                AuthNav.Content = "Hello, " + _userName;
                AuthNav.IsEnabled = false;
            }
        }

        private async void LogoutNav_Click(object sender, RoutedEventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(string.Empty, System.Text.Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Add(BackEndConnection.Authentication, _token);
                var response = await client.PostAsync(BackEndConnection.BaseUrl + BackEndConnection.logoutUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    MainWindow mw = new MainWindow();
                    mw.Show();
                    this.Close();
                }
            }   
        }

        private void ItemNav_Click(object sender, RoutedEventArgs e)
        {
            ItemManagementWindow iw = new ItemManagementWindow(_token, _userName);
            iw.Show();
            iw.Close();
        }

        private async void SearchByDate_Click(object sender, RoutedEventArgs e)
        {
            DateTime? date = EndDatePicker.SelectedDate;
            var request = new HttpRequestMessage(HttpMethod.Get, BackEndConnection.BaseUrl + BackEndConnection.mainWindow_items);
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "upload_date_time", date == null ? string.Empty: date.Value.ToString("yyyy-MM-dd") }
            });
            HttpClient client = new HttpClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                GenerateListViewItem(response);

            }
        }

        private async void ItemListView_Loaded(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(BackEndConnection.BaseUrl + BackEndConnection.mainWindow_allItem);
            if (response.IsSuccessStatusCode)
            {
                GenerateListViewItem(response);

            }
        }

        private void ItemDetailReview(object sender, RoutedEventArgs e, long itemId)
        {
            ItemDetailsWindow iw = new ItemDetailsWindow(itemId);
            iw.Show();
            this.Close();
        }

        private void GenerateListViewItem(HttpResponseMessage response)
        {
            ItemListViewModel itemlist = JsonConvert.DeserializeObject<ItemListViewModel>(response.Content.ReadAsStringAsync().Result);
            PageInfo_TextBox.Text = itemlist.Paginate.NextCursor;
            for (int i = 0; i < itemlist.Items.Count(); i++)
            {
                DockPanel dop = new DockPanel();
                dop.Height = 30;

                StackPanel stackPanel_left = new StackPanel();
                stackPanel_left.Width = 30;
                stackPanel_left.VerticalAlignment = VerticalAlignment.Center;
                stackPanel_left.HorizontalAlignment = HorizontalAlignment.Center;

                StackPanel stackPanel_middle = new StackPanel();
                stackPanel_middle.Width = 300;
                stackPanel_middle.VerticalAlignment = VerticalAlignment.Center;
                stackPanel_middle.HorizontalAlignment = HorizontalAlignment.Center;

                // for item managmenet
/*                StackPanel stackPanel_right = new StackPanel();
                stackPanel_right.Width = 300;
                stackPanel_right.VerticalAlignment = VerticalAlignment.Center;
                stackPanel_right.HorizontalAlignment = HorizontalAlignment.Center;*/

                //middle stackPanel
                Button header = new Button();
                header.Content = itemlist.Items.ElementAt(i).ItemName;
                header.Width = 300;
                header.Height = 15;
                header.Click += (o, e) => ItemDetailReview(o, e, itemlist.Items.ElementAt(i).ItemId);
                header.FontSize = 18;

                Label desc = new Label();
                desc.Content = itemlist.Items.ElementAt(i).Description.Substring(0, 50);
                desc.Width = 300;
                desc.Height = 15;
                desc.FontSize = 18;

/*                //right stackpanel only in item management
                Button delete = new Button();
                delete.Content = "Delete";
                delete.FontSize = 18;
                delete.Width = 80;
                delete.Height = 40;*/

                //left stackpanel
                Image img = new Image();
                var filePath = itemlist.Items.ElementAt(i).CoverImagePath;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(filePath, UriKind.Absolute);
                bitmap.EndInit();
                img.Source = bitmap;
                img.Stretch = Stretch.Fill;
                img.Width = 30;
                img.Height = 30;

                //combine
                stackPanel_left.Children.Add(img);
                stackPanel_middle.Children.Add(header);
                stackPanel_middle.Children.Add(desc);
                /*stackPanel_right.Children.Add(delete);*/
                dop.Children.Add(stackPanel_left);
                dop.Children.Add(stackPanel_middle);
                /*dop.Children.Add(stackPanel_right);*/

                ListViewItem viewItem = new ListViewItem();
                if (i % 2 == 0)
                {
                    viewItem.Background = new SolidColorBrush(Colors.LightCoral);
                }
                viewItem.Background = new SolidColorBrush(Colors.FloralWhite);

                viewItem.Content = dop;

                ItemListView.Items.Add(viewItem);
            }
        }

    }
}
