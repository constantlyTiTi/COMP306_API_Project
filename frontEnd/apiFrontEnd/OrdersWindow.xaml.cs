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
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class OrdersWindow : Window
    {
        private readonly string _userName;
        private readonly string _token;
        private Style style;
        public OrdersWindow(string userName, string token)
        {
            InitializeComponent();
            _userName = userName;
            _token = token;
            UserNameLable.Content = "Hello, " + userName;

            style = new Style();

            style.TargetType = typeof(ListViewItem);

            style.Setters.Add(new Setter(ListView.HorizontalContentAlignmentProperty, HorizontalAlignment.Stretch));

        }

        private async void OrderListView_Loaded(object sender, RoutedEventArgs e)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, BackEndConnection.BaseUrl + 
                BackEndConnection.OrderWindow_Order_userName + _userName);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                OrderListView.Items.Remove(LoadingLabel);
                GenerateListViewItem(response);
            }
            
        }


        private void GenerateListViewItem(HttpResponseMessage response)
        {
            OrderListView.Items.Clear();
            OrderListViewModel orderList = JsonConvert.DeserializeObject<OrderListViewModel>(response.Content.ReadAsStringAsync().Result);
            PageInfo_TextBox.Text = orderList.Paginate.next_curesor;
            for (int i = 0; i < orderList.Orders.Count(); i++)
            {
                StackPanel outer = new StackPanel();
                ListView lsView = new ListView();
                lsView.BorderBrush = Brushes.White;
                lsView.ItemContainerStyle = style;
                DockPanel dop = new DockPanel();
                dop.Height = 30;

                DockPanel dockPanel_left = new DockPanel();
                dockPanel_left.Width = 150;
                dockPanel_left.VerticalAlignment = VerticalAlignment.Center;
                dockPanel_left.HorizontalAlignment = HorizontalAlignment.Center;

                DockPanel dockPanel_middel = new DockPanel();
                dockPanel_middel.Width = 300;
                dockPanel_middel.VerticalAlignment = VerticalAlignment.Center;
                dockPanel_middel.HorizontalAlignment = HorizontalAlignment.Center;

                // for order managmenet
                DockPanel dockPanel_right = new DockPanel();
                dockPanel_right.Width = 200;
                dockPanel_right.VerticalAlignment = VerticalAlignment.Center;
                dockPanel_right.HorizontalAlignment = HorizontalAlignment.Center;

                //left stackpanel
                Label lefts_1 = new Label();
                lefts_1.Content = orderList.Orders.ElementAt(i).order_id;
                lefts_1.Height = 30;
                lefts_1.FontSize = 16;

                Label lefts_2 = new Label();
                lefts_2.Content = orderList.Orders.ElementAt(i).order_time.ToShortDateString();
                lefts_2.Height = 30;
                lefts_2.FontSize = 16;
                dockPanel_left.Children.Add(lefts_1);
                dockPanel_left.Children.Add(lefts_2);

                //middle stackPanel
                Label middle_1 = new Label();
                middle_1.Content = "Shipping Address: " + orderList.Orders.ElementAt(i).shipping_address;
                middle_1.Height = 30;
                middle_1.FontSize = 14;

                Label middle_2 = new Label();
                middle_2.Content = "Satus: " + orderList.Orders.ElementAt(i).Status;
                middle_2.Height = 30;
                middle_2.FontSize = 14;

                dockPanel_middel.Children.Add(middle_1);
                dockPanel_middel.Children.Add(middle_2);

                //right stackPanel
                Label right_1 = new Label();
                right_1.Content = "Total: " + orderList.Orders.ElementAt(i).total_cost;
                right_1.Height = 30;
                right_1.FontSize = 14;

                Button delete = new Button();
                delete.Content = "Cancel";
                delete.FontSize = 12;
                delete.Width = 50;
                delete.Height = 30;
                long orderId = orderList.Orders.ElementAt(i).order_id;
                delete.Click += (o, e) => Cancel(orderId);

                dockPanel_right.Children.Add(right_1);
                dockPanel_right.Children.Add(delete);

                /*stackPanel_right.Children.Add(delete);*/
                dop.Children.Add(dockPanel_left);
                dop.Children.Add(dockPanel_middel);
                dop.Children.Add(dockPanel_right);
                outer.Children.Add(dop);

                ListViewItem viewItem = new ListViewItem();
                if (i % 2 == 0)
                {
                    viewItem.Background = new SolidColorBrush(Colors.LightCoral);
                }
                viewItem.Background = new SolidColorBrush(Colors.FloralWhite);

                viewItem.Content = dop;

                outer.MouseLeftButtonUp += (o,e) => ToggleItemList(orderId, lsView);

                OrderListView.Items.Add(outer); 
                OrderListView.Items.Add(lsView);



            }
        }

        /*private async void SearchByDate_Click(object sender, RoutedEventArgs e)
        {
            DateTime? endDate = EndDatePicker.SelectedDate;
            DateTime? startDate = StartDatePicker.SelectedDate;
            if(endDate == null || startDate == null)
            {
                MessageBox.Show("Please fill both start date and end date");
                return;
            }
            if (endDate.Value < startDate.Value)
            {
                MessageBox.Show("end date cannot be before start date");
                return;
            }
            OrderListView.Items.Clear();
            Label LoadingLabel = new Label();
            LoadingLabel.Margin = new Thickness(20, 0, 0, 0);
            LoadingLabel.FontSize = 18;
            LoadingLabel.FontWeight = FontWeights.Bold;
            OrderListView.Items.Add(LoadingLabel);

            HttpClient client = new HttpClient();
            var response = await client.GetAsync(BackEndConnection.BaseUrl + BackEndConnection.mainWindow_items + 
                "? start_date ="+ startDate.Value.ToString("yyyy-MM-dd")+  "&&end_date =" + endDate.Value.ToString("yyyy-MM-dd"));
            *//*            var response = await client.SendAsync(request);*//*
            if (response.IsSuccessStatusCode)
            {
                OrderListView.Items.Remove(LoadingLabel);
                GenerateListViewItem(response);

            }
        }*/

        private async void ToggleItemList(long orderId, ListView lsView)
        {

            if (lsView.Items.Count == 0)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, BackEndConnection.BaseUrl + BackEndConnection.OrderWindow_Order_orderId + orderId.ToString());
                
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

                var response = await client.SendAsync(request);

                StackPanel dop_stack = new StackPanel();
                dop_stack.VerticalAlignment = VerticalAlignment.Stretch;

                if (response.IsSuccessStatusCode)
                {
                    OrderDetails orderDetails = JsonConvert.DeserializeObject<OrderDetails>(response.Content.ReadAsStringAsync().Result);

                    for(int i =0; i< orderDetails.items.Count(); i++)
                    {
                        DockPanel dop_Items = new DockPanel();
                        //dop_item left
                        Image img = new Image();
/*                        var filePath = orderDetails.items.ElementAt(i).cover_Image_path;
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(filePath, UriKind.Absolute);
                        bitmap.EndInit();
                        img.Source = bitmap;
                        img.Stretch = Stretch.Fill;*/
                        img.Width = 30;
                        img.Height = 30;
                        dop_Items.Children.Add(img);

                        //dop_item middle
                        Label m1l = new Label();
                        m1l.HorizontalAlignment = HorizontalAlignment.Center;
                        m1l.Content =  orderDetails.items.ElementAt(i).item_name;
                        m1l.Height = 30;
                        m1l.Width = 150;
                        m1l.FontSize = 14;
                        dop_Items.Children.Add(m1l);

                        //dop_item middle
                        Label m2l = new Label();
                        m2l.HorizontalAlignment = HorizontalAlignment.Center;
                        m2l.Content = "Price:" + orderDetails.items.ElementAt(i).Price;
                        m2l.Height = 30;
                        m2l.Width = 150;
                        m2l.FontSize = 14;
                        dop_Items.Children.Add(m2l);

                        //dop_item right
                        Label mr = new Label();
                        mr.HorizontalAlignment = HorizontalAlignment.Center;
                        mr.Content = "Quantity: " + orderDetails.items.ElementAt(i).Quantity;
                        mr.Height = 30;
                        mr.Width = 250;
                        mr.FontSize = 14;
                        dop_Items.Children.Add(mr);

                        dop_stack.Children.Add(dop_Items);
                    }

                    lsView.Items.Add(dop_stack);
                }

                if (lsView.Items.Count > 1)
                {
                    lsView.Items.RemoveAt(1);
                }
            }
        }

        private async void Cancel(long orderId)
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
        }

        private void HomeNav_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow(_userName, _token);
            mw.Top = this.Top;
            mw.Left = this.Left;
            mw.Show();
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
    }
}
