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
    /// Interaction logic for ShoppingCart.xaml
    /// </summary>
    public partial class ShoppingCart : Window
    {
        private readonly string _userName;
        private readonly string _token;
        public ShoppingCart(string userName, string token)
        {
            InitializeComponent();

            Style style = new Style();

            style.TargetType = typeof(ListViewItem);

            style.Setters.Add(new Setter(ListView.HorizontalContentAlignmentProperty, HorizontalAlignment.Stretch));

            ShoppingCartListView.ItemContainerStyle = style;

            _userName = userName;
            _token = token;
            if (string.IsNullOrWhiteSpace(userName))
            {
                UserNameLable.Content = "Welcome to your shopping cart";
            }
            else
            {
                UserNameLable.Content = "Welcome back " + userName + "'s shopping cart";
            }
            
        }

        private void HomeNav_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            if (!string.IsNullOrWhiteSpace(_userName) && !string.IsNullOrWhiteSpace(_token))
            {
                mw = new MainWindow(_userName, _token);
            }
                
            mw.Top = this.Top;
            mw.Left = this.Left;
            mw.Show();
            this.Close();
        }

        private async void ShoppingCartListView_Loaded(object sender, RoutedEventArgs e)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, BackEndConnection.BaseUrl +
                BackEndConnection.ShoppingCartWindow_allItem + MainWindow._uniqueId.ToString());
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                GenerateListViewItem(response);
                OuterStackPanel.Children.Remove(LoadingLabel);

            }
            else
            {
                LoadingLabel.Content = "Oops, something is wrong.";
            }
        }

        private void GenerateListViewItem(HttpResponseMessage response)
        {
            ShoppingCartListView.Items.Clear();
            ShoppingCartVM shoppingCart = JsonConvert.DeserializeObject<ShoppingCartVM>(response.Content.ReadAsStringAsync().Result);
            TotalCostLabel.Content = shoppingCart.total_cost;
            if(shoppingCart.shopping_cart_items == null)
            {
                return;
            }

            foreach (var item in shoppingCart.shopping_cart_items)
            {
                ShoppingCartItem cartItem = new ShoppingCartItem();
                cartItem.item_id = item.ItemId;
                cartItem.Price = item.Price;
                cartItem.user_name = _userName;
                DockPanel dop_Items = new DockPanel();
                dop_Items.Height = 30;

                //right
                Image img = new Image();
/*                var filePath = item.cover_Image_path;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(filePath, UriKind.Absolute);
                bitmap.EndInit();
                img.Source = bitmap;
                img.Stretch = Stretch.Fill;*/
                img.Width = 30;
                img.Height = 30;
                dop_Items.Children.Add(img);

                //middle
                Label mdLable1 = new Label();
                mdLable1.Content = item.item_name;
                mdLable1.Width = 100;
                mdLable1.FontSize = 14;
                dop_Items.Children.Add(mdLable1);

                Label mdLable2 = new Label();
                mdLable2.Content = item.Price;
                mdLable2.Width = 100;
                mdLable2.FontSize = 14;
                dop_Items.Children.Add(mdLable2);

                TextBox quantyTB = new TextBox();
                quantyTB.Width = 80;
                quantyTB.VerticalAlignment = VerticalAlignment.Center;
                quantyTB.HorizontalAlignment = HorizontalAlignment.Center;
                quantyTB.FontSize = 14;
                quantyTB.Text = (item.Quantity).ToString();
                dop_Items.Children.Add(quantyTB);

                //Add and delete
                Button delete = new Button();
                delete.Height = 20;
                delete.Width = 50;
                delete.Content = "-";
                delete.FontSize = 14;
                delete.FontWeight = FontWeights.Bold;
                delete.Margin = new Thickness(10, 5, 10, 5);
                delete.Click += (o, e) => reduceQuantity(quantyTB, cartItem, dop_Items);
                dop_Items.Children.Add(delete);

                Button add = new Button();
                add.Height = 20;
                add.Width = 50;
                add.Content = "+";
                add.FontSize = 14;
                add.FontWeight = FontWeights.Bold;
                add.Margin = new Thickness(10, 5, 10, 5);
                add.Click += (o, e) => addQuantity(quantyTB, cartItem);
                dop_Items.Children.Add(add);

                ShoppingCartListView.Items.Add(dop_Items);
            }

        }

        private async void reduceQuantity(TextBox tb, ShoppingCartItem item, DockPanel dop_Items)
        {
            
            if (tb.Text.CompareTo("1") > 0)
            {
                tb.Text = (int.Parse(tb.Text) - 1).ToString();
                item.Quantity = int.Parse(tb.Text);
                string itemJsonValue = JsonConvert.SerializeObject(item);

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, BackEndConnection.BaseUrl 
                    + BackEndConnection.ShoppingCartWindow_Item_update + MainWindow._uniqueId.ToString());
                StringContent bodyContent = new StringContent(itemJsonValue, Encoding.UTF8, "application/json");
                request.Content = bodyContent;
                HttpClient client = new HttpClient();
                await client.SendAsync(request);
            }
            else
            {
                HttpClient client = new HttpClient();
                var response = await client.DeleteAsync(BackEndConnection.BaseUrl + BackEndConnection.ShoppingCartWindow_Item 
                    + item.item_id.ToString() + @"/" + MainWindow._uniqueId.ToString());
                if (response.IsSuccessStatusCode)
                {
                    ShoppingCartListView.Items.Remove(dop_Items);
                }
            }

        }

        private async void addQuantity(TextBox tb, ShoppingCartItem item)
        {
            tb.Text = (int.Parse(tb.Text) + 1).ToString();
            item.Quantity = int.Parse(tb.Text);
            string itemJsonValue = JsonConvert.SerializeObject(item);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, 
                BackEndConnection.BaseUrl + BackEndConnection.ShoppingCartWindow_Item_update + MainWindow._uniqueId.ToString());
            StringContent bodyContent = new StringContent(itemJsonValue, Encoding.UTF8, "application/json");
            request.Content = bodyContent;
            HttpClient client = new HttpClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                TotalCostLabel.Content = ((double)Decimal.Parse(TotalCostLabel.Content.ToString())) + item.Price;
            }

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            ShoppingCartVM cart = new ShoppingCartVM();
            cart.shipping_address = ShippingAddressTextBox.Text;
            cart.user_name = _userName;

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, BackEndConnection.BaseUrl 
                + BackEndConnection.ShoppingCartWindow_PlaceOrder + "?user_name="+ _userName + "&unique_tempor_user_id=" + MainWindow._uniqueId);
            
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
            request.Content = new StringContent(JsonConvert.SerializeObject(cart), Encoding.UTF8, "application/json");
            var response = await client.SendAsync(request);


            if (response.IsSuccessStatusCode)
            {
                OrdersWindow ow = new OrdersWindow(_userName, _token);
                ow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please login/register");
                LoginAndRegistration lgw = new LoginAndRegistration();
                lgw.Show();
                this.Close();
            }
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
