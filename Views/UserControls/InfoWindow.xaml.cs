using System.Windows;
using System.Windows.Input;

namespace WPFPasswordManager.Views.UserControls
{
    /// <summary>
    /// Interaction logic for InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        public InfoWindow()
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
    }
}
