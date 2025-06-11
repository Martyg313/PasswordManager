using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace WPFPasswordManager.Views.UserControls
{
    /// <summary>
    /// Interaction logic for RemoveInfoWindow.xaml
    /// </summary>
    public partial class RemoveInfoWindow : Window
    {
        public RemoveInfoWindow(int count)
        {
            InitializeComponent();
            this.range = count;
        }

        private readonly int range;   // range of encrypted entry rows

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
        private void IndexBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(IndexBox.Text))
            {
                IndexBlock.Visibility = Visibility.Visible;
            }
            else
            {
                IndexBlock.Visibility = Visibility.Hidden;
            }
        }

        /**
         * Parse the given row for removal of its entry
         */
        private void Button_Index_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int input = int.Parse(IndexBox.Text);
                if (input >= 1 && input <= range)
                {
                    this.DialogResult = true;
                }
                else
                {
                    throw new Exception("Invalid output");
                }
            }
            catch 
            { 
                // Index out of range or invalid input
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
