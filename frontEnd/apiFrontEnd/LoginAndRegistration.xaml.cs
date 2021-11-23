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
    /// Interaction logic for LoginAndRegistration.xaml
    /// </summary>
    public partial class LoginAndRegistration : Window
    {
        private string _token;
        private string _userName;
        public LoginAndRegistration()
        {
            InitializeComponent();
            UserNameTextBox_Login.Text = string.Empty;
            UserNameTextBox_Register.Text = string.Empty;

        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UserNameTextBox_Login.Text;
            string password = PasswordTextBox_Login.Password;

            if(string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ErrorMsgLable_Login.Content = "Please fill all field";
            }

            UserAuth user = new UserAuth();
            user.UserName = username;
            user.Password = password;
            StringContent content = new StringContent(JsonConvert.SerializeObject(user), System.Text.Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                var response = await client.PostAsync(BackEndConnection.BaseUrl + BackEndConnection.loginUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    UserAuth userReturn = JsonConvert.DeserializeObject<UserAuth>(response.Content.ReadAsStringAsync().Result);
                    _token = userReturn.Token;
                    _userName = userReturn.UserName;
                    MainWindow mw = new MainWindow(_userName, _token);
                    mw.Top = this.Top;
                    mw.Left = this.Left;
                    mw.Show();
                    this.Close();
                }
                else
                {
                    JObject errorDic = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                    List<string> errorList = errorDic["errors"].ToObject<List<string>>();
                    string error = string.Join(@"\n", errorList);
                    ErrorMsgLable_Login.Content = error;
                }
            }
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UserNameTextBox_Register.Text;
            string password = PasswordTextBox_Register.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ErrorMsgLable_Login.Content = "Please fill all field";
            }

            UserAuth user = new UserAuth();
            user.UserName = username;
            user.Password = password;
            StringContent content = new StringContent(JsonConvert.SerializeObject(user), System.Text.Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                var response = await client.PostAsync(BackEndConnection.BaseUrl + BackEndConnection.registerUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    UserAuth userReturn = JsonConvert.DeserializeObject<UserAuth>(response.Content.ReadAsStringAsync().Result);
                    _token = userReturn.Token;
                    _userName = userReturn.UserName;
                    MainWindow mw = new MainWindow(_userName, _token);
                    mw.Top = this.Top;
                    mw.Left = this.Left;
                    mw.Show();
                    this.Close();
                }
                else
                {
                    JObject errorDic = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                    List<string> errorList = errorDic["errors"].ToObject<List<string>>();
                    string error = string.Join(Environment.NewLine, errorList);
                    ErrorMsgLable_Register.Content = error;
                }
            }
        }

        private void HomeNav_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Top = this.Top;
            mw.Left = this.Left;
            mw.Show();
            this.Close();
        }

    }
}
