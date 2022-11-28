using FAM_App.Pages;
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
            IsAdmin();
        }

        private void IsAdmin()
        {
            AddEmployee.Visibility = Visibility.Visible;
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void ShowFixedAssets_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new ShowFixedAssetsPage();
        }

        private void AddFixedAssets_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new AddFixedAssetsPage();
        }
        private void AddOtherFixedAssets_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new AddOtherFixedAssetsPage();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new AddProductsPage();
        }
        private void ShowProducts_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new ShowProductsPage();
        }

        private void FixedAssetsStatement_Click(object sender, RoutedEventArgs e)
        {

        }
        private void FixedAssetCard_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new FixedAssetCardPage();
        }
        private void ShowSupplier_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new ShowSupplierPage();
        }
        private void AddSupplier_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new AddSupplierPage();
        }
        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new AddEmployeePage();
        }

        private void LogOut_MouseEnter(object sender, RoutedEventArgs e)
        {
            LogOut.BorderThickness = new Thickness(1);
        }

        private void LogOut_MouseLeave(object sender, MouseEventArgs e)
        {
            LogOut.BorderThickness = new Thickness(0);
        }

    }
}
