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
    /// Logika interakcji dla klasy ShowEmployeePage.xaml
    /// </summary>
    public partial class ShowEmployeePage : Page
    {
        public ShowEmployeePage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            DataBase dataBase = new DataBase();
            DataTable Employee = new DataTable("emp");
            Employee = dataBase.ShowEmployee(Employee);
            EmployeeDataGrid.ItemsSource = Employee.DefaultView;
        }

        private void ByName_Checked(object sender, RoutedEventArgs e)
        {
            ByName_TxtBox.IsEnabled= true;
        }

        private void ByName_Unchecked(object sender, RoutedEventArgs e)
        {
            ByName_TxtBox.IsEnabled = false;
            ByName_TxtBox.Clear();
        }

        private void BySurname_Checked(object sender, RoutedEventArgs e)
        {
            BySurname_TxtBox.IsEnabled = true;
        }

        private void BySurname_Unchecked(object sender, RoutedEventArgs e)
        {
            BySurname_TxtBox.IsEnabled= false;
            BySurname_TxtBox.Clear();
        }

        private void ByEmail_Checked(object sender, RoutedEventArgs e)
        {
            ByEmail_TxtBox.IsEnabled = true;
        }

        private void ByEmail_Unchecked(object sender, RoutedEventArgs e)
        {
            ByEmail_TxtBox.IsEnabled = false;
            ByEmail_TxtBox.Clear();
        }

        private void ByCity_Checked(object sender, RoutedEventArgs e)
        {
            ByCity_TxtBox.IsEnabled = true;
        }

        private void ByCity_Unchecked(object sender, RoutedEventArgs e)
        {
            ByEmail_TxtBox.IsEnabled = false;
            ByCity_TxtBox.Clear();
        }

        private void ByStreet_Checked(object sender, RoutedEventArgs e)
        {
            ByStreet_TxtBox.IsEnabled = true;
        }

        private void ByStreet_Unchecked(object sender, RoutedEventArgs e)
        {
            ByStreet_TxtBox.IsEnabled = false;
            ByStreet_TxtBox.Clear();
        }
    }
}
