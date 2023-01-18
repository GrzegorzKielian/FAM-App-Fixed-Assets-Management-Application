using FAM_App.Windows;
using System;
using System.Collections;
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
    /// Logika interakcji dla klasy ShowEmployeePage.xaml
    /// </summary>
    public partial class ShowEmployeePage : Page
    {
        List<String> whichColumn = new List<String>();
        public ShowEmployeePage()
        {
            InitializeComponent();
            LoadData();
            AddColumsToList();
        }

        private void AddColumsToList()
        {
            whichColumn.Add("Imie");
            whichColumn.Add("Nazwisko");
            whichColumn.Add("Email");
            whichColumn.Add("Miejscowosc");
            whichColumn.Add("Ulica");
        }

        private void LoadData()
        {
            DataBase dataBase = new DataBase();
            DataTable Employee = new DataTable("emp");
            Employee = dataBase.DataBaseShowEmployee(Employee);
            EmployeeDataGrid.ItemsSource = Employee.DefaultView;
            EmployeeDataGrid.CanUserAddRows= false;
            ChooseBox.SelectedIndex = 0;
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(EmployeeDataGrid.SelectedItem == null) return;
            DataRowView dr = EmployeeDataGrid.SelectedItem as DataRowView;
            DataRow dr1 = dr.Row;
            int id_of_the_edited_employee = Convert.ToInt32(dr1.ItemArray[0]);
            EditEmployeeWindow editEmployeeWindow = new EditEmployeeWindow(id_of_the_edited_employee);
            editEmployeeWindow.Show();
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
                    query = "SELECT ID_Pracownika, Imie, Nazwisko, Pesel, Telefon, Email, Miejscowosc, Kod_Pocztowy, Ulica, Nr_Budynku, Nr_Lokalu, Admin, Ewidencja, Login, Hash, Sol_Hasla FROM dbo.Pracownik WHERE (" + whichColumn[index] +" LIKE '"+chooseTxt+"');";
                    DataBase dataBase = new DataBase();
                    DataTable Products = new DataTable("emp");
                    Products = dataBase.DataBaseShowSelectedData(Products, query);
                    EmployeeDataGrid.ItemsSource = Products.DefaultView;
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void ShowAll_Button_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}
