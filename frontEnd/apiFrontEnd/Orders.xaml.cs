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
    public partial class Orders : Window
    {
        private readonly string _userName;
        private readonly string _token;
        public Orders(string userName, string token)
        {
            InitializeComponent();
            _userName = userName;
            _token = token;
            UserNameLable.Content = "Hello, " + userName;
        }

        private async void OrderListView_Loaded(object sender, RoutedEventArgs e)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, BackEndConnection.BaseUrl + 
                BackEndConnection.OrderWindow_Order_userName + _userName);
            HttpClient client = new HttpClient();
            request.Headers.Add(BackEndConnection.Authentication, "Bearer " + _token);
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                GenerateListViewItem(response);
            }
            
        }


        private void GenerateListViewItem(HttpResponseMessage response)
        {
            OrderListViewModel orderList = JsonConvert.DeserializeObject<OrderListViewModel>(response.Content.ReadAsStringAsync().Result);
            PageInfo_TextBox.Text = orderList.Paginate.NextCursor;
            for (int i = 0; i < orderList.Orders.Count(); i++)
            {
                StackPanel outer = new StackPanel();
                DockPanel dop = new DockPanel();
                dop.Height = 30;

                StackPanel stackPanel_left = new StackPanel();
                stackPanel_left.Width = 100;
                stackPanel_left.VerticalAlignment = VerticalAlignment.Center;
                stackPanel_left.HorizontalAlignment = HorizontalAlignment.Center;

                StackPanel stackPanel_middle = new StackPanel();
                stackPanel_middle.Width = 300;
                stackPanel_middle.VerticalAlignment = VerticalAlignment.Center;
                stackPanel_middle.HorizontalAlignment = HorizontalAlignment.Center;

                // for order managmenet
                StackPanel stackPanel_right = new StackPanel();
                stackPanel_right.Width = 200;
                stackPanel_right.VerticalAlignment = VerticalAlignment.Center;
                stackPanel_right.HorizontalAlignment = HorizontalAlignment.Center;

                //left stackpanel
                Label lefts_1 = new Label();
                lefts_1.Content = orderList.Orders.ElementAt(i).OrderId;
                lefts_1.Height = 15;
                lefts_1.FontSize = 18;

                Label lefts_2 = new Label();
                lefts_2.Content = orderList.Orders.ElementAt(i).OrderTime.ToShortDateString();
                lefts_2.Height = 15;
                lefts_2.FontSize = 18;
                stackPanel_left.Children.Add(lefts_1);
                stackPanel_left.Children.Add(lefts_2);

                //middle stackPanel
                Label middle_1 = new Label();
                middle_1.Content = "Shipping Address: " + orderList.Orders.ElementAt(i).ShippingAddress;
                middle_1.Height = 15;
                middle_1.FontSize = 18;

                Label middle_2 = new Label();
                middle_2.Content = "Satus: " + orderList.Orders.ElementAt(i).Status;
                middle_2.Height = 15;
                middle_2.FontSize = 18;

                stackPanel_middle.Children.Add(lefts_1);
                stackPanel_middle.Children.Add(lefts_2);

                //right stackPanel
                Label right_1 = new Label();
                right_1.Content = "Total: " + orderList.Orders.ElementAt(i).TotalCost;
                right_1.Height = 15;
                right_1.FontSize = 18;

                Button delete = new Button();
                delete.Content = "Cancel";
                delete.FontSize = 12;
                delete.Width = 50;
                delete.Height = 15;
                delete.Click += (o, e) => Cancel(orderList.Orders.ElementAt(i).OrderId);

                stackPanel_right.Children.Add(lefts_1);
                stackPanel_right.Children.Add(lefts_2);

                /*stackPanel_right.Children.Add(delete);*/
                dop.Children.Add(stackPanel_left);
                dop.Children.Add(stackPanel_middle);
                dop.Children.Add(stackPanel_right);
                outer.Children.Add(dop);

                ListViewItem viewItem = new ListViewItem();
                if (i % 2 == 0)
                {
                    viewItem.Background = new SolidColorBrush(Colors.LightCoral);
                }
                viewItem.Background = new SolidColorBrush(Colors.FloralWhite);

                viewItem.Content = dop;

                outer.MouseLeftButtonUp += (o,e) => ToggleItemList(orderList.Orders.ElementAt(i).OrderId, outer);

                OrderListView.Items.Add(outer);

                
            }
        }

        private async void ToggleItemList(long orderId, StackPanel ourter)
        {

            if (ourter.Children.Count == 1)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, BackEndConnection.BaseUrl + BackEndConnection.OrderWindow_Order_orderId + orderId.ToString());
                request.Headers.Add(BackEndConnection.Authentication, "Bearer " + _token);
                HttpClient client = new HttpClient();
                var response = await client.SendAsync(request);

                StackPanel dop_stack = new StackPanel();

                if (response.IsSuccessStatusCode)
                {
                    OrderDetails orderDetails = JsonConvert.DeserializeObject<OrderDetails>(response.Content.ReadAsStringAsync().Result);

                    for(int i =0; i< orderDetails.items.Count(); i++)
                    {
                        DockPanel dop_Items = new DockPanel();
                        //dop_item left
                        Image img = new Image();
                        var filePath = orderDetails.items.ElementAt(i).CoverImagePath;
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(filePath, UriKind.Absolute);
                        bitmap.EndInit();
                        img.Source = bitmap;
                        img.Stretch = Stretch.Fill;
                        img.Width = 30;
                        img.Height = 30;
                        dop_Items.Children.Add(img);

                        //dop_item middle
                        Label m1l = new Label();
                        m1l.Content =  orderDetails.items.ElementAt(i).ItemName;
                        m1l.Height = 30;
                        m1l.Width = 100;
                        m1l.FontSize = 18;
                        dop_Items.Children.Add(m1l);

                        //dop_item middle
                        Label m2l = new Label();
                        m2l.Content = orderDetails.items.ElementAt(i).Price;
                        m2l.Height = 30;
                        m2l.Width = 80;
                        m2l.FontSize = 18;
                        dop_Items.Children.Add(m1l);

                        //dop_item right
                        Label mr = new Label();
                        mr.Content = "Quantity: " + orderDetails.items.ElementAt(i).Quantity;
                        mr.Height = 30;
                        mr.Width = 80;
                        mr.FontSize = 18;
                        dop_Items.Children.Add(m1l);

                        dop_stack.Children.Add(dop_Items);

                    }
                    ourter.Children.Add(dop_stack);
                }

                if (ourter.Children.Count > 1)
                {
                    ourter.Children.RemoveAt(1);
                }
            }
        }

        private async void Cancel(long orderId)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, BackEndConnection.BaseUrl + 
                BackEndConnection.OrderWindow_Order_userName + _userName);
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "order_id", orderId.ToString()}
            });
            request.Headers.Add(BackEndConnection.Authentication, "Bearer " + _token);
            await client.SendAsync(request);
        }

        private void HomeNav_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow(_userName, _token);
            mw.Show();
            this.Close();
        }
    }
}
