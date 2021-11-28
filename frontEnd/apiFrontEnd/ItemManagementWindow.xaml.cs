using apiFrontEnd.Models;
using apiFrontEnd.StaticValues;
using Newtonsoft.Json;
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
using System.Windows.Shapes;

namespace apiFrontEnd
{
    /// <summary>
    /// Interaction logic for ItemManagementWindow.xaml
    /// </summary>
    public partial class ItemManagementWindow : Window
    {
        private readonly string _userName;
        private readonly string _token;

        public ItemManagementWindow(string token, string userName)
        {
            InitializeComponent();
            _token = token;
            _userName = userName;
            PostItemLabel.Content = _userName + "'s Items Management";
            PostItemLabel.FontSize = 18;
        }

        private void HomeNav_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow(_userName, _token);
            mw.Top = this.Top;
            mw.Left = this.Left;
            mw.Show();
            this.Close();
        }

        private void PostBtn_Click(object sender, RoutedEventArgs e)
        {
            PostItem pIt = new PostItem(_userName, _token);

            pIt.Top = this.Top;
            pIt.Left = this.Left;
            pIt.Show();
            this.Close();
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Application.Current.Windows.OfType<OrdersWindow>().Any() ||
               Application.Current.Windows.OfType<ShoppingCart>().Any() ||
               Application.Current.Windows.OfType<LoginAndRegistration>().Any() ||
               Application.Current.Windows.OfType<ItemManagementWindow>().Any() ||
               Application.Current.Windows.OfType<MainWindow>().Any())
            {
                return;
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                    var response = await client.GetAsync(BackEndConnection.BaseUrl + BackEndConnection.logoutUrl + MainWindow._uniqueId.ToString());
                }
            }
        }

        private async void ItemListView_Loaded(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
            var response = await client.GetAsync(BackEndConnection.BaseUrl + BackEndConnection.postItemWindow_items_userName + _userName);
            if (response.IsSuccessStatusCode)
            {
                ItemListView.Items.Remove(LoadingLabel);
                GenerateListViewItem(response);
               

            }
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

                StackPanel stackPanel_right = new StackPanel();
                stackPanel_right.Width = 100;
                stackPanel_right.VerticalAlignment = VerticalAlignment.Center;
                stackPanel_right.HorizontalAlignment = HorizontalAlignment.Center;

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
                header.Click += (o, e) => ItemUpdateReview(o, e, itemId);
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


/*                Button delete = new Button();
                delete.Content = "Remove";
                delete.FontSize = 12;
                delete.Width = 50;
                delete.Height = 30;
                long ItemId = itemlist.Items.ElementAt(i).ItemId;
                delete.Click += (o, e) => Cancel(ItemId);*/

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

        private void ItemUpdateReview(object sender, RoutedEventArgs e, long itemId)
        {
            PostItem iw = new PostItem(itemId, _userName, _token);
            iw.Top = this.Top;
            iw.Left = this.Left;
            iw.Show();
            this.Close();
        }

       /* private async void Cancel(long itemId)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, BackEndConnection.BaseUrl +
                BackEndConnection.OrderWindow_Order_userName + _userName + "/" + orderId.ToString());

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                HttpRequestMessage request2 = new HttpRequestMessage(HttpMethod.Get, BackEndConnection.BaseUrl +
                BackEndConnection.OrderWindow_Order_userName + _userName);
                HttpClient client2 = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                var response2 = await client.SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    GenerateListViewItem(response2);
                }
            }
        }*/
    }
}
