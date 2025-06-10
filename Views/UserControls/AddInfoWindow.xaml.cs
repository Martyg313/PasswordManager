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

namespace WPFPasswordManager.Views.UserControls
{
    /// <summary>
    /// Interaction logic for AddInfoWindow.xaml
    /// </summary>
    public partial class AddInfoWindow : Window
    {
        public AddInfoWindow()
        {
            InitializeComponent();
        }

        /**
         * Enables window dragging
         */
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        /**
         * Input box response
         */
        private void TextBox_Subject_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(subjectBox.Text))
            {
                subjectBlock.Visibility = Visibility.Visible;
            }
            else
            {
                subjectBlock.Visibility = Visibility.Hidden;
            }
        }

        /**
         * Input box response
         */
        private void TextBox_Info_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(infoBox.Text))
            {
                infoBlock.Visibility = Visibility.Visible;
            }
            else
            {
                infoBlock.Visibility = Visibility.Hidden;
            }
        }

        /**
         * Closes the window and returns true for main window logic
         */
        private void Button_Enter_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true; 
        }
    }
}
