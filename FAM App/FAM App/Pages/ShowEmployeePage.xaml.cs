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
    }
}
