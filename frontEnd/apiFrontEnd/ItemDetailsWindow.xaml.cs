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
    /// Interaction logic for ItemDetailsWindow.xaml
    /// </summary>
    public partial class ItemDetailsWindow : Window
    {
        public ItemDetailsWindow(long itemId)
        {
            InitializeComponent();
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
                foreach(var imgPath in itemDetail.ItemImagePaths)
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
        }
    }
}
