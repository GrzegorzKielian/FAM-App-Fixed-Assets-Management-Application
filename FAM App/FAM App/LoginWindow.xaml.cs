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

namespace FAM_App
{
    /// <summary>
    /// Logika interakcji dla klasy LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        string login;
        string password;
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            LoginTxtBox.Text = String.Empty;
            PasswdTxtBox.Clear();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                MainMenu mainMenu = new MainMenu();
                mainMenu.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("db error");
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
