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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace apiFrontEnd
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string _userName;
        private readonly string _token;
        public MainWindow()
        {
            InitializeComponent();
            ItemNav.Visibility = Visibility.Hidden;
            OrderNav.Visibility = Visibility.Hidden;
            LogoutNav.Visibility = Visibility.Hidden;
            _token = string.Empty;
            _userName = string.Empty;
        }

        public MainWindow(string userName, string token)
        {
            InitializeComponent();
            _userName = userName;
            _token = token;
        }

        private void AuthNav_Click(object sender, RoutedEventArgs e)
        {
            LoginAndRegistration lrw = new LoginAndRegistration();
            lrw.Show();
            this.Close();
        }

        private void HomeNav_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_token))
            {
                ItemNav.Visibility = Visibility.Hidden;
                OrderNav.Visibility = Visibility.Hidden;
            }
            else
            {
                ItemNav.Visibility = Visibility.Visible;
                OrderNav.Visibility = Visibility.Visible;
            }
        }

        private async void LogoutNav_Click(object sender, RoutedEventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(string.Empty, System.Text.Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Add(BackEndConnection.Authentication, _token);
                var response = await client.PostAsync(BackEndConnection.BaseUrl + BackEndConnection.logoutUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    MainWindow mw = new MainWindow();
                    mw.Show();
                    this.Close();
                }
            }   
        }

        private void ItemNav_Click(object sender, RoutedEventArgs e)
        {
            ItemManagementWindow iw = new ItemManagementWindow(_token, _userName);
            iw.Show();
            iw.Close();
        }

        private async void SearchByDate_Click(object sender, RoutedEventArgs e)
        {
            DateTime? date = EndDatePicker.SelectedDate;
            var request = new HttpRequestMessage(HttpMethod.Get, BackEndConnection.BaseUrl + BackEndConnection.mainWindow_items);
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "upload_date_time", date == null ? string.Empty: date.Value.ToString("yyyy-MM-dd") }
            });
            HttpClient client = new HttpClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                ItemListViewModel itemlist = JsonConvert.DeserializeObject<ItemListViewModel>(response.Content.ReadAsStringAsync().Result);
                PageInfo_TextBox.Text = itemlist.Paginate.NextCursor;
                for(int i = 0; i<itemlist.Items.Count();i++)
                {
                    DockPanel dop = new DockPanel();
                    dop.Height = 30;
                    DockPanel dop_left = new DockPanel();
                    dop_left.Width = 30;
                    StackPanel stackPanel_middle = new StackPanel();
                    stackPanel_middle.Width = 300;


                    Button header = new Button();
                    header.Content = itemlist.Items.ElementAt(i).ItemName;
                    header.Width = 300;
                    header.Height = 15;
                    header.Click += (o, e) => itemListViewEH(o, e, itemlist.Items.ElementAt(i).ItemId);

                    Label desc = new Label();
                    desc.Content = itemlist.Items.ElementAt(i).Description.Substring(0,50);
                    header.Width = 300;
                    header.Height = 15;




                    Image img = new Image();
                    var filePath = itemlist.Items.ElementAt(i).CoverImagePath;
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(filePath, UriKind.Absolute);
                    bitmap.EndInit();
                    img.Source = bitmap;

                    ListViewItem viewItem = new ListViewItem();
                    if (i % 2 == 0)
                    {
                        viewItem.Background = new SolidColorBrush(Colors.LightCoral);
                    }
                    viewItem.Background = new SolidColorBrush(Colors.FloralWhite);
                    

                    ItemListView.Items.Add(viewItem);
                }

            }
        }

        private void ItemListView_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void itemListViewEH(object sender, RoutedEventArgs e, long itemId)
        {
            ItemDetailsWindow iw = new ItemDetailsWindow(itemId);
            iw.Show();
            this.Close();
        }

    }
}
