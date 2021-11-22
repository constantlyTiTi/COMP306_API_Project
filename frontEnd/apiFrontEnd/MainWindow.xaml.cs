using apiFrontEnd.StaticValues;
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

        private void SearchByDate_Click(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = StartDatePicker.SelectedDate.Value;
            DateTime? endDate = EndDatePicker.SelectedDate.Value;
            HttpClient client = new HttpClient();
            StringContent content = new StringContent(string.Empty, System.Text.Encoding.UTF8, "application/json");
        }
    }
}
