using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPFPasswordManager
{
    /// <summary>
    /// Interaction logic for PasswordWindow.xaml
    /// </summary>
    public partial class PasswordCreationWindow : Window
    {
        // Regex pattern for: 12+ chars, lowercase, uppercase, symbol (no spaces)
        private const string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\s).{12,}$";

        public PasswordCreationWindow()
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
         * Closes the application
         */
        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /**
         * Input box response
         */
        private void PasswordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(passwordBox.Text))
            {
                passwordBlock.Visibility = Visibility.Visible;
            }
            else
            {
                passwordBlock.Visibility = Visibility.Hidden;
            }
        }

        /**
         * Enter button logic
         */
        private void Button_Enter_Click(object sender, RoutedEventArgs e)
        {
            // Parsing the password string
            if (Regex.IsMatch(passwordBox.Text, pattern))
            {
                Encryption.HashNewPassword(passwordBox.Text);                   // Attempt password hash
                Encryption.EncryptFile(new List<string>(), "", "", true);       // Encrypt nothing to file for initialization

                Window menu = new MainWindow();
                menu.Show();

                this.Close();
            }
            else
            {
                // Raise warning
                Disclaimer.Visibility = Visibility.Visible;
                
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(3);
                timer.Tick += (sender, e) =>
                {
                    Disclaimer.Visibility = Visibility.Hidden;
                    timer.Stop();
                };
                timer.Start();
            }
        }
    }
}
