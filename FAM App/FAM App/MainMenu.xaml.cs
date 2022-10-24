using System;
using System.Collections.Generic;
using System.Data;
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
    /// Logika interakcji dla klasy MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void ShowFixedAssets_Click(object sender, RoutedEventArgs e)
        {
            //DataBase dataBase = new DataBase();
            DataTable FixedAssets = new DataTable("emp");
            //FixedAssets = dataBase.DataBaseConnection(FixedAssets, 1);
            datagrid1.ItemsSource = FixedAssets.DefaultView;
        }

        private void AddFixedAssets_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditFixedAssets_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
