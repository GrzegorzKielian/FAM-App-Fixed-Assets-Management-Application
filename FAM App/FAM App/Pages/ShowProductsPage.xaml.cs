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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FAM_App.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy ShowProductsPage.xaml
    /// </summary>
    public partial class ShowProductsPage : Page
    {
        public ShowProductsPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            DataBase dataBase = new DataBase();
            DataTable Products = new DataTable("emp");
            Products = dataBase.DataBaseShowProducts(Products);
            ProductsDataGrid.ItemsSource = Products.DefaultView;
        }

        private void ByName_Checked(object sender, RoutedEventArgs e)
        {
            ByName_TxtBox.IsEnabled = true;
        }

        private void ByBrand_Checked(object sender, RoutedEventArgs e)
        {
            ByBrand_TxtBox.IsEnabled = true;
        }

        private void ByModel_Checked(object sender, RoutedEventArgs e)
        {
            ByModel_TxtBox.IsEnabled = true;
        }

        private void ByYear_Checked(object sender, RoutedEventArgs e)
        {
            ByYear_TxtBox.IsEnabled = true;
        }

        private void ByName_Unchecked(object sender, RoutedEventArgs e)
        {
            ByName_TxtBox.IsEnabled = false;
        }

        private void ByBrand_Unchecked(object sender, RoutedEventArgs e)
        {
            ByBrand_TxtBox.IsEnabled = false;
        }

        private void ByModel_Unchecked(object sender, RoutedEventArgs e)
        {
            ByModel_TxtBox.IsEnabled = false;
        }

        private void ByYear_Unchecked(object sender, RoutedEventArgs e)
        {
            ByYear_TxtBox.IsEnabled = false;
        }

        private void Search_Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

    }
}
