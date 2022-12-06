using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Logika interakcji dla klasy AddOtherFixedAssetsPage.xaml
    /// </summary>
    public partial class AddOtherFixedAssetsPage : Page
    {
        private int groupID;
        private int subgroupID;
        private int typeID;

        public AddOtherFixedAssetsPage()
        {
            InitializeComponent();
            SetFixedAssetData();

        }
        private void SetFixedAssetData()
        {
            Current_Date.Text = DateTime.Now.ToString("yyyy.MM.dd");
            FixedAsset_Code.Text = "1";
        }

        private void AddOtherFixedAsset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Czy na pewno chcesz dodać nowy środek trwały do bazy danych?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void openFile_Click(object sender, RoutedEventArgs e)
        {
            //System.Diagnostics.Process.Start(appPath);
        }


        private void UpdateItems(int number)
        {
            try
            {
                if(number == 1) 
                { 
                    DataBase dataBase = new DataBase();
                    DataTable groups = new DataTable();
                    groups = dataBase.DataBaseShowGroup(groups);

                    Group.ItemsSource = groups.DefaultView;
                    Group.DisplayMemberPath = "Grupa";
                    Group.SelectedValuePath = "ID_Grupy";

                }
                if (number == 2)
                {
                    DataBase dataBase = new DataBase();
                    DataTable subgroups = new DataTable();
                    subgroups = dataBase.DataBaseShowSubgroup(subgroups, groupID);

                    Subgroup.ItemsSource = subgroups.DefaultView;
                    Subgroup.DisplayMemberPath = "Podgrupa";
                    Subgroup.SelectedValuePath = "ID_Podgrupy";

                }
                if(number == 3)
                {
                    DataBase dataBase = new DataBase();
                    DataTable types = new DataTable();
                    types = dataBase.DataBaseShowType(types, subgroupID);

                    Type.ItemsSource = types.DefaultView;
                    Type.DisplayMemberPath = "Rodzaj";
                    Type.SelectedValuePath = "ID_Rodzaju";
                }
                if (number == 4)
                {
                    DataBase dataBase = new DataBase();
                    DataTable products = new DataTable();
                    products = dataBase.DataBaseShowProducts(products);

                    Product.ItemsSource = products.DefaultView;
                    Product.SelectedValuePath = "ID_Produktu";
                }
                if (number == 5)
                {
                    DataBase dataBase = new DataBase();
                    DataTable suppliers = new DataTable();
                    suppliers = dataBase.DataBaseShowSuppliers(suppliers);

                    Supplier.ItemsSource = suppliers.DefaultView;
                    Supplier.SelectedValuePath = "ID_Dostawcy";
                }

            }
            catch(Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void Group_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateItems(1);
        }

        private void Group_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Group.SelectedValue != null) { Subgroup.IsEnabled = true; groupID = (int)Group.SelectedValue; UpdateItems(2); }
        }

        private void Subgroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Subgroup.SelectedValue != null) { Type.IsEnabled = true; subgroupID = (int)Subgroup.SelectedValue; UpdateItems(3); }
        }

        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Type.SelectedValue!= null) { typeID= (int)Type.SelectedValue;}
        }

        private void Product_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateItems(4);
        }

        private void Supplier_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateItems(5);
        }

    }
}
