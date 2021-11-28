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
using System.Windows.Shapes;

namespace apiFrontEnd
{
    /// <summary>
    /// Interaction logic for PostItem.xaml
    /// </summary>
    public partial class PostItem : Window
    {
        private readonly string _userName;
        private readonly string _token;
        private List<string> categories = new List<string> { "Appliances", "Books", "Computers", "Clothes" , "Furniture" , "Miscelanious" , "Plants" };
        private string ErrorMsgLable_Post;

        //public object item_id { get; private set; }

        public PostItem(string userName, string token)
        {
            InitializeComponent();
            _userName = userName;
            _token = token;
            UserNameLable.Content = "Hello, " + userName + "! Now you can post your item!";
            ItemName.Text = string.Empty;
            itemDescription.Text = string.Empty;
            PostalCode.Text = string.Empty;
            itemPrice.Text = string.Empty;
            //this itemCategory.ItemSource = categories;
            
        }

        //private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    string selectedCategory = this.categories[itemCategory.SelectedIndex].ToString();
        //    //itemCategory.Text = itemCategory.SelectedItem.ToString();
        //    //return itemCategory.Text;
        //}
       

            private async void addBtn_Click(object sender, RoutedEventArgs e)
        {
            string itemName = ItemName.Text;
            string description = itemDescription.Text;
            string selectedCategory = this.categories[itemCategory.SelectedIndex].ToString();
            string postalCode = PostalCode.Text;
           // double Price = itemPrice.Text;
            if (string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(itemName))
            {
                ErrorMsgLable_Post = "Please fill all fields";
            }
         
            Item item = new Item();
            item.item_name = ItemName.Text;
            item.Description = itemDescription.Text;
            item.Category = selectedCategory;
           // item.Price = itemPrice.Text;
            // do another field
            StringContent content = new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                var response = await client.PostAsync(BackEndConnection.BaseUrl + BackEndConnection.postItemWindow_itemPost, content);
                if (response.IsSuccessStatusCode)
                {
                    //itemManagement window
                    ItemManagementWindow iw = new ItemManagementWindow(_token, _userName);
                    iw.Top = this.Top;
                    iw.Left = this.Left;
                    iw.Show();
                    this.Close();
                }
                else
                {
                    JObject errorDic = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                    List<string> errorList = errorDic["errors"].ToObject<List<string>>();
                    string error = string.Join(Environment.NewLine, errorList);
                    ErrorMsgLable_Post = error;
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

       

        private void ItemName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
