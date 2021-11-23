﻿using apiFrontEnd.Models;
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
            _userName = userName;
            _token = token;
            UserNameLable.Content = "Welcome to " + userName + "'s shopping cart";
        }

        private void HomeNav_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow(_userName, _token);
            mw.Show();
            this.Close();
        }

        private async void ShoppingCartListView_Loaded(object sender, RoutedEventArgs e)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, BackEndConnection.BaseUrl +
                BackEndConnection.ShoppingCartWindow_allItem);
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
            ShoppingCartVM shoppingCart = JsonConvert.DeserializeObject<ShoppingCartVM>(response.Content.ReadAsStringAsync().Result);
            TotalCostLabel.Content = "Total cost: " + shoppingCart.TotalCost;
            
            foreach(var item in shoppingCart.ShoppingCartItems)
            {
                DockPanel dop_Items = new DockPanel();
                dop_Items.Height = 30;

                //right
                Image img = new Image();
                var filePath = item.CoverImagePath;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(filePath, UriKind.Absolute);
                bitmap.EndInit();
                img.Source = bitmap;
                img.Stretch = Stretch.Fill;
                img.Width = 30;
                img.Height = 30;
                dop_Items.Children.Add(img);

                //middle
                Label mdLable1 = new Label();
                mdLable1.Content = item.ItemName;
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
                quantyTB.Text = "1";
                dop_Items.Children.Add(quantyTB);

                //Add and delete
                Button delete = new Button();
                delete.Height = 30;
                delete.Width = 50;
                delete.Content = "-";
                delete.FontSize = 18;
                delete.FontWeight = FontWeights.Bold;
                delete.Margin = new Thickness(10, 5, 10, 5);

                Button add = new Button();
                add.Height = 30;
                add.Width = 50;
                add.Content = "+";
                add.FontSize = 18;
                add.FontWeight = FontWeights.Bold;
                add.Margin = new Thickness(10, 5, 10, 5);

                ShoppingCartListView.Items.Add(dop_Items);
            }

        }

        private async void delete(TextBox tb, ShoppingCartItem item)
        {
            string itemJsonValue = JsonConvert.SerializeObject(item);
            if(tb.Text.CompareTo("1") > 0)
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, BackEndConnection.BaseUrl + BackEndConnection.ShoppingCartWindow_Item_update);
                StringContent bodyContent = new StringContent(itemJsonValue);
                request.Content = bodyContent;
                HttpClient client = new HttpClient();
                await client.SendAsync(request);
                tb.Text = (int.Parse(tb.Text) - 1).ToString();
            }
            else
            {

            }
            

        }

    }
}
