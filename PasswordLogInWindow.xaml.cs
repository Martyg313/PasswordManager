using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace WPFPasswordManager
{
    /// <summary>
    /// Interaction logic for PasswordLogInWindow.xaml
    /// </summary>
    public partial class PasswordLogInWindow : Window
    {
        public PasswordLogInWindow()
        {
            InitializeComponent();
        }

        /**
         * Closes the application
         */
        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
        private void passwordBox_TextChanged(object sender, TextChangedEventArgs e)
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
            bool Errors = true;

            try
            {
                KeyFileManager.ReadFromFile();                  // Updates file info to program   
                Encryption.HashPassword(passwordBox.Text);      // Hash password 
                Encryption.DecryptFile();                       // Attempt decryption

                Errors = false;
            }
            catch 
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

            if (!Errors) // If no error occurs, then load main window
            {
                Window menu = new MainWindow();
                menu.Show();
                this.Close();
            }
        }
    }
}
