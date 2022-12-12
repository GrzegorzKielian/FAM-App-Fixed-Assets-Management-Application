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
    /// Logika interakcji dla klasy ShowSupplierPage.xaml
    /// </summary>
    public partial class ShowSupplierPage : Page
    {
        public ShowSupplierPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            DataBase dataBase = new DataBase();
            DataTable Suppliers = new DataTable("emp");
            Suppliers = dataBase.DataBaseShowSuppliers(Suppliers);
            SuppliersDataGrid.ItemsSource = Suppliers.DefaultView;
        }

        private void ByName_Checked(object sender, RoutedEventArgs e)
        {
            ByName_TxtBox.IsEnabled = true;
        }

        private void ByCity_Checked(object sender, RoutedEventArgs e)
        {
            ByCity_TxtBox.IsEnabled = true;
        }

        private void ByPostCode_Checked(object sender, RoutedEventArgs e)
        {
            ByPostCode_TxtBox.IsEnabled = true;
        }

        private void ByStreet_Checked(object sender, RoutedEventArgs e)
        {
            ByStreet_TxtBox.IsEnabled = true;
        }


        private void ByName_Unchecked(object sender, RoutedEventArgs e)
        {
            ByName_TxtBox.IsEnabled = false;
            ByName_TxtBox.Clear();
        }

        private void ByCity_Unchecked(object sender, RoutedEventArgs e)
        {
            ByCity_TxtBox.IsEnabled = false;
            ByCity_TxtBox.Clear();
        }

        private void ByPostCode_Unchecked(object sender, RoutedEventArgs e)
        {
            ByPostCode_TxtBox.IsEnabled = false;
            ByPostCode_TxtBox.Clear();
        }

        private void ByStreet_Unchecked(object sender, RoutedEventArgs e)
        {
            ByStreet_TxtBox.IsEnabled = false;
            ByStreet_TxtBox.Clear();
        }
        private void Search_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
