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
        List<String> whichColumn = new List<String>();
        public ShowProductsPage()
        {
            InitializeComponent();
            LoadData();
            AddColumsToList();
        }

        private void AddColumsToList()
        {
            whichColumn.Add("Nazwa");
            whichColumn.Add("Marka");
            whichColumn.Add("Model");
            whichColumn.Add("Rok_Produkcji");
        }

        private void LoadData()
        {
            DataBase dataBase = new DataBase();
            DataTable Products = new DataTable("emp");
            Products = dataBase.DataBaseShowProducts(Products);
            ProductsDataGrid.ItemsSource = Products.DefaultView;
            ProductsDataGrid.CanUserAddRows= false;
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
                    query = "SELECT ID_Produktu, Nazwa, Marka, Model, Opis, Rok_Produkcji FROM dbo.Produkt WHERE (" + whichColumn[index] +" LIKE '" + chooseTxt + "');";
                    DataBase dataBase = new DataBase();
                    DataTable Products = new DataTable("emp");
                    Products = dataBase.DataBaseShowSelectedData(Products, query);
                    ProductsDataGrid.ItemsSource = Products.DefaultView;
                    ProductsDataGrid.CanUserAddRows = false;
                }
            }
            catch(Exception ex)
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
