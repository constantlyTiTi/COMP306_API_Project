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
        public static long _uniqueId = 0;
        public MainWindow()
        {
            InitializeComponent();
            ItemNav.Visibility = Visibility.Hidden;
            OrderNav.Visibility = Visibility.Hidden;
            LogoutNav.Visibility = Visibility.Hidden;
            _token = string.Empty;
            _userName = string.Empty;
            GenerateUniqueId();
        }

        private void GenerateUniqueId()
        {
            if (_uniqueId == 0)
            {
                HttpClient client = new HttpClient();
                var response = client.GetAsync(BackEndConnection.BaseUrl + BackEndConnection.ShoppingCartWindow_GenerateRandomId).Result;
                if (response.IsSuccessStatusCode)
                {
                    _uniqueId = JsonConvert.DeserializeObject<long>(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        public MainWindow(string userName, string token)
        {
            InitializeComponent();
            this._userName = userName;
            _token = token;
            AuthNav.Content = "Hello, " + _userName;
            AuthNav.IsEnabled = false;
            GenerateUniqueId();
        }

        private void AuthNav_Click(object sender, RoutedEventArgs e)
        {
            LoginAndRegistration lrw = new LoginAndRegistration();
            lrw.Top = this.Top;
            lrw.Left = this.Left;
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

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                var response = await client.GetAsync(BackEndConnection.BaseUrl + BackEndConnection.logoutUrl + _uniqueId.ToString());
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
            iw.Top = this.Top;
            iw.Left = this.Left;
            iw.Show();
            this.Close();
        }

        private async void SearchByDate_Click(object sender, RoutedEventArgs e)
        {
            DateTime? date = EndDatePicker.SelectedDate;

            ItemListView.Items.Clear();
            Label LoadingLabel = new Label();
            LoadingLabel.Margin = new Thickness(20, 0, 0, 0);
            LoadingLabel.FontSize = 18;
            LoadingLabel.FontWeight = FontWeights.Bold;
            ItemListView.Items.Add(LoadingLabel);

            string dateString = date == null ? string.Empty : date.Value.ToString("yyyy-MM-dd");
            HttpClient client = new HttpClient();
            var response =  await client.GetAsync(BackEndConnection.BaseUrl + BackEndConnection.mainWindow_items + "?upload_date_time=" + date.Value.ToString("yyyy-MM-dd"));
/*            var response = await client.SendAsync(request);*/
            if (response.IsSuccessStatusCode)
            {
                ItemListView.Items.Remove(LoadingLabel);
                GenerateListViewItem(response);

            }
        }

        private async void ItemListView_Loaded(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(BackEndConnection.BaseUrl + BackEndConnection.mainWindow_allItem);
            if (response.IsSuccessStatusCode)
            {
                ItemListView.Items.Remove(LoadingLabel);
                GenerateListViewItem(response);

            }
        }

        private void OrderNav_Click(object sender, RoutedEventArgs e)
        {
            OrdersWindow ow = new OrdersWindow(_userName, _token);
            ow.Top = this.Top;
            ow.Left = this.Left;
            ow.Show();
            this.Close();
        }


        // methods

        private void ItemDetailReview(object sender, RoutedEventArgs e, long itemId)
        {
            ItemDetailsWindow iw = new ItemDetailsWindow(itemId, _userName, _token);
            iw.Top = this.Top;
            iw.Left = this.Left;
            iw.Show();
            this.Close();
        }

        private void GenerateListViewItem(HttpResponseMessage response)
        {
            ItemListView.Items.Clear();
            ItemListViewModel itemlist = JsonConvert.DeserializeObject<ItemListViewModel>(response.Content.ReadAsStringAsync().Result);

            PageInfo_TextBox.Text = itemlist.Paginate.next_curesor;
            for (int i = 0; i < itemlist.Items.Count(); i++)
            {
                DockPanel dop = new DockPanel();
                dop.Height = 30;

                StackPanel stackPanel_left = new StackPanel();
                stackPanel_left.Width = 100;
                stackPanel_left.VerticalAlignment = VerticalAlignment.Center;
                stackPanel_left.HorizontalAlignment = HorizontalAlignment.Center;

                StackPanel stackPanel_middle = new StackPanel();
                stackPanel_middle.Width = 150;
                stackPanel_middle.VerticalAlignment = VerticalAlignment.Center;
                stackPanel_middle.HorizontalAlignment = HorizontalAlignment.Center;

                StackPanel stackPanel_middle2 = new StackPanel();
                stackPanel_middle2.Width = 300;
                stackPanel_middle2.VerticalAlignment = VerticalAlignment.Center;
                stackPanel_middle2.HorizontalAlignment = HorizontalAlignment.Center;

                // for item managmenet
                /*                StackPanel stackPanel_right = new StackPanel();
                                stackPanel_right.Width = 300;
                                stackPanel_right.VerticalAlignment = VerticalAlignment.Center;
                                stackPanel_right.HorizontalAlignment = HorizontalAlignment.Center;*/

                //middle stackPanel
                Button header = new Button();
                header.Content = itemlist.Items.ElementAt(i).item_name;
                header.Width = 100;
                header.Height = 30;
                long itemId = itemlist.Items.ElementAt(i).ItemId;
                header.Click += (o, e) => ItemDetailReview(o, e, itemId);
                header.FontSize = 14;

                Label desc = new Label();
                desc.Content = itemlist.Items.ElementAt(i).Description.Length > 50 ? 
                    itemlist.Items.ElementAt(i).Description.Substring(0, 50) : itemlist.Items.ElementAt(i).Description;
                desc.Width = 250;
                desc.Height = 30;
                desc.FontSize = 14;

                /*                //right stackpanel only in item management
                                Button delete = new Button();
                                delete.Content = "Delete";
                                delete.FontSize = 18;
                                delete.Width = 80;
                                delete.Height = 40;*/

                //left stackpanel
                /*                Image img = new Image();
                                var filePath = itemlist.Items.ElementAt(i).cover_Image_path;
                                BitmapImage bitmap = new BitmapImage();
                                bitmap.BeginInit();
                                bitmap.UriSource = new Uri(filePath, UriKind.Absolute);
                                bitmap.EndInit();
                                img.Source = bitmap;
                                img.Stretch = Stretch.Fill;
                                img.Width = 60;
                                img.Height = 60;*/
                Label uploadedTime = new Label();
                uploadedTime.Width = 100;
                uploadedTime.FontSize = 14;
                uploadedTime.VerticalAlignment = VerticalAlignment.Center;
                uploadedTime.HorizontalAlignment = HorizontalAlignment.Center;
                uploadedTime.Content = itemlist.Items.ElementAt(i).upload_item_date_time.ToShortDateString();

                //combine
                stackPanel_left.Children.Add(uploadedTime);
                stackPanel_middle.Children.Add(header);
                stackPanel_middle2.Children.Add(desc);
                dop.Children.Add(stackPanel_left);
                dop.Children.Add(stackPanel_middle);
                dop.Children.Add(stackPanel_middle2);
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

        private void CartNav_Click(object sender, RoutedEventArgs e)
        {
            ShoppingCart sw = new ShoppingCart(_userName, _token);
            sw.Top = this.Top;
            sw.Left = this.Left;
            sw.Show();
            this.Close();
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(Application.Current.Windows.OfType<OrdersWindow>().Any()||
               Application.Current.Windows.OfType<ShoppingCart>().Any()||
               Application.Current.Windows.OfType<LoginAndRegistration>().Any() ||
               Application.Current.Windows.OfType<ItemManagementWindow>().Any()||
               Application.Current.Windows.OfType<MainWindow>().Any())
            {
                return;
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                    var response = await client.GetAsync(BackEndConnection.BaseUrl + BackEndConnection.logoutUrl + _uniqueId.ToString());
                }
            }
        }
    }
}
