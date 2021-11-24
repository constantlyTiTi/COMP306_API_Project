using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for ItemManagementWindow.xaml
    /// </summary>
    public partial class ItemManagementWindow : Window
    {
        private readonly string _userName;
        private readonly string _token;

        public ItemManagementWindow(string token, string userName)
        {
            InitializeComponent();
            _token = token;
            _userName = userName;
        }

        private void HomeNav_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow(_userName, _token);
            mw.Top = this.Top;
            mw.Left = this.Left;
            mw.Show();
            this.Close();
        }

        private void PostBtn_Click(object sender, RoutedEventArgs e)
        {
            PostItem pIt = new PostItem(_userName, _token);

            pIt.Top = this.Top;
            pIt.Left = this.Left;
            pIt.Show();
            this.Close();
        }

     
    }
}
