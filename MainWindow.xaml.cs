using System.Windows;
using System.Windows.Input;
using WPFPasswordManager.Views.UserControls;

namespace WPFPasswordManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Update ui from data
            UpdateDisplay();
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
         * Adds a new encrypted entry
         */
        private void Button_Plus_Click(object sender, RoutedEventArgs e)
        {
            AddInfoWindow aif = new AddInfoWindow();
            bool? windowResult = aif.ShowDialog();

            if (windowResult == true)
            {
                // Encrypt new data
                Encryption.EncryptFile(Encryption.DecryptFile(), aif.subjectBox.Text, aif.infoBox.Text, false);

                // Display on main window
                InfoBox newInfo = new InfoBox();
                newInfo.SubjectBoxText.Content = aif.subjectBox.Text;
                newInfo.infoBoxText.Text = aif.infoBox.Text;
            
                infoScreen.Children.Add(newInfo);
            }
        }

        /**
         * Removes the selected entry given its row
         */
        private void Button_Minus_Click(object sender, RoutedEventArgs e)
        {
            RemoveInfoWindow rif = new RemoveInfoWindow(infoScreen.Children.Count);
            bool? windowResult = rif.ShowDialog();

            if (windowResult == true)
            {
                // Removes an the specified entry from the encrypted data
                Encryption.EncryptFile(Encryption.RemoveEntryDecryptFile(int.Parse(rif.IndexBox.Text) - 1), "", "", true);

                infoScreen.Children.RemoveAt(int.Parse(rif.IndexBox.Text) - 1);
            }
        }

        /**
         * Displays the instruction/info window
         */
        private void Button_Info_Click(object sender, RoutedEventArgs e)
        {
            InfoWindow iw = new InfoWindow();
            iw.ShowDialog();
        }

        /**
        * Updates the gui of the main window for all entries
        */
        private void UpdateDisplay()
        {
            List<string> myData = Encryption.DecryptFile();
            for (int i = 0; i < myData.Count / 2; i++)
            {
                InfoBox newInfo = new InfoBox();
                newInfo.SubjectBoxText.Content = myData[i * 2];     // Applies title
                newInfo.infoBoxText.Text = myData[i * 2 + 1];       // Applies info

                infoScreen.Children.Add(newInfo);
            }
        }
    }
}