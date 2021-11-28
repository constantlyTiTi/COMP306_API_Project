using apiFrontEnd.Models;
using apiFrontEnd.StaticValues;
using AutoMapper;
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
using System.Windows.Shapes;

namespace apiFrontEnd
{
    /// <summary>
    /// Interaction logic for ItemDetailsWindow.xaml
    /// </summary>
    public partial class ItemDetailsWindow : Window
    {
        private readonly string _userName;
        private readonly string _token;
        private readonly long _itemId;
        public ItemDetailsWindow(long itemId, string userName, string token)
        {
            InitializeComponent();
            _userName = userName;
            _token = token;
            Generate(itemId);
            _itemId = itemId;

        }

        private async void Generate(long itemId)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(BackEndConnection.BaseUrl + BackEndConnection.mainWindow_itemDetail + itemId.ToString());
            if (response.IsSuccessStatusCode)
            {
                Item itemDetail = JsonConvert.DeserializeObject<Item>(response.Content.ReadAsStringAsync().Result);
                ItemDesLabel.Content = itemDetail.Description;
                ItemNameLabel.Content = itemDetail.item_name;
                ItemPriceLabel.Content = itemDetail.Price;

                Button addToCart = new Button();
                addToCart.Height = 40;
                addToCart.Width = 200;
                addToCart.Content = "Add to Shopping Cart";
                addToCart.FontSize = 16;
                addToCart.Background = Brushes.LightYellow;

                ShoppingCartItem cartItem = new ShoppingCartItem();
                cartItem.item_id = itemDetail.ItemId;
                cartItem.Price = itemDetail.Price;
                cartItem.Quantity = 1;
                cartItem.user_name = _userName;
                addToCart.Click += (o, e) => AddItemToShoppingCart(cartItem);

                DetailPanel.Children.Add(addToCart);

/*                foreach (var imgPath in itemDetail.item_imgs_paths)
                {
                    Image img = new Image();
                    var filePath = imgPath;
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(filePath, UriKind.Absolute);
                    bitmap.EndInit();
                    img.Source = bitmap;
                    img.Stretch = Stretch.Fill;
                    img.Width = 50;
                    img.Height = 50;
                    img.Margin = new Thickness(0, 2, 0, 0);

                    DetailPanel.Children.Add(img);
                }*/

            }
            else
            {
                ItemDesLabel.Content = "Oops, nothing is found";

            }
        }

        private async void AddItemToShoppingCart(ShoppingCartItem item)
        {
            HttpClient client = new HttpClient();
            StringContent content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
            
            var response = await client.PostAsync(BackEndConnection.BaseUrl + BackEndConnection.ShoppingCartWindow_Item 
                + item.item_id.ToString() + @"/" + MainWindow._uniqueId.ToString()
                , content);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("The item has been added to shopping cart");
            }
            else
            {
                JObject errorObject = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                MessageBox.Show(errorObject.GetValue("error").ToString());
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
