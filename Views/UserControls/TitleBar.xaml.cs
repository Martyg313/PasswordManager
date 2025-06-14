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

namespace WPFPasswordManager.Views.UserControls
{
    /// <summary>
    /// Interaction logic for TitleBar.xaml
    /// </summary>
    public partial class TitleBar : UserControl
    {
        public TitleBar()
        {
            InitializeComponent();
        }

        // Closes the application
        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            //Application.Current.Shutdown();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.DialogResult = false; // This will close the dialog and return false
            parentWindow.Close();
        }
    }
}
