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
        public ItemDetailsWindow(long itemId, string userName, string token)
        {
            InitializeComponent();
            _userName = userName;
            _token = token;
            Generate(itemId);

        }

        private async void Generate(long itemId)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(BackEndConnection.BaseUrl + BackEndConnection.mainWindow_itemDetail + itemId.ToString());
            if (response.IsSuccessStatusCode)
            {
                Item itemDetail = JsonConvert.DeserializeObject<Item>(response.Content.ReadAsStringAsync().Result);
                ItemDesLabel.Content = itemDetail.Description;
                ItemNameLabel.Content = itemDetail.ItemName;

                Button addToCart = new Button();
                addToCart.Height = 30;
                addToCart.Width = 100;
                addToCart.Content = "Add to Shopping Cart";
                addToCart.FontSize = 16;
                addToCart.Background = Brushes.LightYellow;

                ShoppingCartItem cartItem = new ShoppingCartItem();
                cartItem.ItemId = itemDetail.ItemId;
                cartItem.Price = itemDetail.Price;
                cartItem.Quantity = 1;
                cartItem.UserName = _userName;
                addToCart.Click += (o, e) => AddItemToShoppingCart(cartItem);

                DetailPanel.Children.Add(addToCart);

                foreach (var imgPath in itemDetail.ItemImagePaths)
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
                }

            }
            else
            {
                ItemDesLabel.Content = "Oops, nothing is found";

            }
        }

        private async void AddItemToShoppingCart(ShoppingCartItem item)
        {
            HttpClient client = new HttpClient();
            StringContent content = new StringContent(JsonConvert.SerializeObject(item));
            var response = await client.PostAsync(BackEndConnection.BaseUrl + BackEndConnection.ShoppingCartWindow_Item + item.ItemId.ToString(), content);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("The item has been added to shopping cart");
            }
            else
            {
                JObject errorObject = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                MessageBox.Show(errorObject.GetValue("Error").ToString());
            }

        }
    }
}
