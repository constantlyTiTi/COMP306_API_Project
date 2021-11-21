﻿using System;
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
    }
}
