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
    }
}
