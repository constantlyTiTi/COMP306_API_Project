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

        public object item_id { get; private set; }

        public PostItem(string userName, string token)
        {
            InitializeComponent();
            _userName = userName;
            _token = token;
            UserNameLable.Content = "Hello, " + userName + "! Now you can post your item!";
        }

        private async void PostItemView_Loaded(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(BackEndConnection.BaseUrl + BackEndConnection.mainWindow_allItem);
            if (response.IsSuccessStatusCode)
            {
                _ = response;

            }
        }

        private async void AddItemToPostItem(PostItem item)
        {
            HttpClient client = new HttpClient();
            StringContent content = new StringContent(JsonConvert.SerializeObject(item));
            var response = await client.PostAsync(BackEndConnection.BaseUrl + BackEndConnection.PostItemWindow_Item + item.item_id.ToString(), content);
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
        private void addBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HomeNav_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow(_userName, _token);
            mw.Top = this.Top;
            mw.Left = this.Left;
            mw.Show();
            this.Close();
        }

        
    }
}
