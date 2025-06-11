using System.Windows;

namespace WPFPasswordManager
{
    /// <summary>
    /// Interaction logic for PasswordLogInWindow.xaml
    /// </summary>
    public partial class App : Application
    {
        protected Window windowToShow = null!;

        /**
         * Determines starting window based on vault file existence
         */
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);  // Ensures the framework's initialization

            if (KeyFileManager.CheckFile())
            {
                // Proceed to password login window
                windowToShow = new PasswordLogInWindow();
            }
            else
            {
                // Proceed to Password creation window
                windowToShow = new PasswordCreationWindow();
            }
            windowToShow.Show();
        }
    }
}
