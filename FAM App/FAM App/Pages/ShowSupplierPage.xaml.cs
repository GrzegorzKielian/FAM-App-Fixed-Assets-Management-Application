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
        List<String> whichColumn = new List<String>();
        public ShowSupplierPage()
        {
            InitializeComponent();
            LoadData();
            AddColumsToList();
        }

        private void AddColumsToList()
        {
            whichColumn.Add("Nazwa");
            whichColumn.Add("Miejscowosc");
            whichColumn.Add("Kod_Pocztowy");
            whichColumn.Add("Ulica");
        }

        private void LoadData()
        {
            DataBase dataBase = new DataBase();
            DataTable Suppliers = new DataTable("emp");
            Suppliers = dataBase.DataBaseShowSuppliers(Suppliers);
            SuppliersDataGrid.ItemsSource = Suppliers.DefaultView;
            SuppliersDataGrid.CanUserAddRows= false;
        }

        
        private void Search_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = ChooseBox.SelectedIndex;
                string chooseTxt = Choose_TxtBox.Text;
                String query = "";
                if (index == -1) { MessageBox.Show("Nie wybrano opcji wyszukiwania"); }
                else
                {
                    query = "SELECT ID_Dostawcy, Nazwa, Miejscowosc, Kod_Pocztowy, Ulica FROM dbo.Dostawca WHERE (" + whichColumn[index] +" LIKE '" + chooseTxt + "');";
                    DataBase dataBase = new DataBase();
                    DataTable Suppliers = new DataTable("emp");
                    Suppliers = dataBase.DataBaseShowSelectedData(Suppliers, query);
                    SuppliersDataGrid.ItemsSource = Suppliers.DefaultView;
                    SuppliersDataGrid.CanUserAddRows = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ShowAll_Button_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}
