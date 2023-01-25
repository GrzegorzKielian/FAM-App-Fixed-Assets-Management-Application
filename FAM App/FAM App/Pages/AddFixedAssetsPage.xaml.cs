using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Reflection;
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

namespace FAM_App
{
    /// <summary>
    /// Logika interakcji dla klasy AddFixedAssetsPage.xaml
    /// </summary>
    public partial class AddFixedAssetsPage : Page
    {
        private int groupID;
        private int subgroupID;
        private int typeID;

        public AddFixedAssetsPage()
        {
            InitializeComponent();
            SetFixedAssetData();
        }
        private void SetFixedAssetData()
        {
            Current_Date.Text = DateTime.Now.ToString("yyyy-MM-dd");
            FixedAsset_Code.Text = "0";
            Depreciation_method.Text = "Amortyzacja liniowa";
        }

        private void AddFixedAsset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Czy na pewno chcesz dodać nowy środek trwały do bazy danych?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    string introduction_date = Current_Date.Text;
                    string fixedAsset_Code = FixedAsset_Code.Text;
                    int supplier = CheckSupplier();
                    int product = CheckProduct(); 
                    int adress = CheckAdress(); 
                    int user = CheckIsUser();
                    string status = Status.Text;
                    int depreciation = Convert.ToInt32(Depreciation_rate.Text); 
                    string date_of_aquisition = ChangeFormatDate();
                    string gros_orig_value = TwoStringToDecimal(Gross_orig_val1_TxtBox.Text, Gross_orig_val2_TxtBox.Text);
                    string net_orig_value = TwoStringToDecimal(Net_orig_val1_TxtBox.Text, Net_orig_val2_TxtBox.Text);
                    string descritpion = Description_TxtBox.Text;
                    string invoice = Invoice.Text;
                    int guarantee = Convert.ToInt32(Guarantee.Text);

                    if (supplier == 0 || product == 0 || adress == 0 || status == String.Empty ) { MessageBox.Show("Wymagane pola są puste"); }
                    else
                    {
                        DataBase dataBase = new DataBase();
                        bool check = dataBase.AddFixedAssetsToBase(introduction_date, fixedAsset_Code, supplier, product, adress, status, depreciation, date_of_aquisition, gros_orig_value, net_orig_value, descritpion, invoice, guarantee, groupID, subgroupID, typeID, user);
                        if (check)
                        {
                            MessageBox.Show("Dodano do bazy."); //:\n" + introduction_date + "\n" + fixedAsset_Code + "\n" + typeID + "\n" + Supplier.Text + "\n" + Product.Text + "\n" + Adress.Text + "\n" + status + "\n" + depreciation + "\n" + date_of_aquisition + "\n" + gros_orig_value + "\n" + net_orig_value + "\n" + descritpion + "\n" + invoice + "\n" + guarantee + "\n");
                            ClearTextBoxes();
                        }
                        else { MessageBox.Show("Błąd przy wstawianiu danych do bazy!"); }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wymagane pola są puste!");
                MessageBox.Show(ex.ToString());
            }
        }

        private string ChangeFormatDate()
        {
            string changedDate = this.Date_of_aquisition.SelectedDate.Value.Date.ToString("yyyy-MM-dd");
            return changedDate;

        }

        private int CheckAdress()
        {
            if (Adress.Text == String.Empty) { return 0; }
            else { return (int)Adress.SelectedValue; }
        }

        private int CheckProduct()
        {
            if (Product.Text == String.Empty) { return 0; }
            else { return (int)Product.SelectedValue; }
        }

        private int CheckSupplier()
        {
            if (Supplier.Text == String.Empty) { return 0; }
            else { return (int)Supplier.SelectedValue; }
        }

        private int CheckIsUser()
        {
            if (User.Text == String.Empty) { return 0; }
            else { return (int)User.SelectedValue; }

        }

        private void AddInvoiveBox_Click(object sender, RoutedEventArgs e)
        {
            OpenFile();
            if (Invoice.Text != String.Empty)
            {
                ClearInvoiveBox.Visibility = Visibility.Visible;
            }
        }

        private void ClearInvoiveBox_Click(object sender, RoutedEventArgs e)
        {
            Invoice.Inlines.Clear();
            ClearInvoiveBox.Visibility = Visibility.Hidden;
        }

        private void OpenFile()
        {
            try
            {
                OpenFileDialog choofdlog = new OpenFileDialog();
                choofdlog.Filter = "All Files (*.*)|*.*";
                choofdlog.FilterIndex = 1;
                choofdlog.Multiselect = true;

                if (choofdlog.ShowDialog() == true)
                {
                    string sFileName = choofdlog.FileName;
                    Invoice.Text = sFileName;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void UpdateItems(int number)
        {
            try
            {
                if (number == 1)
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
                if (number == 3)
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
                if(number == 6)
                {
                    DataBase dataBase = new DataBase();
                    DataTable adresses = new DataTable();
                    adresses = dataBase.DataBaseShowAdresses(adresses);

                    Adress.ItemsSource = adresses.DefaultView;
                    Adress.SelectedValuePath = "ID_Adresu";
                }
                if(number == 7)
                {
                    DataBase dataBase = new DataBase();
                    DataTable employee = new DataTable();
                    employee = dataBase.DataBaseShowEmployee(employee);

                    User.ItemsSource = employee.DefaultView;
                    User.SelectedValuePath = "ID_Pracownika";
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
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
            if (Type.SelectedValue != null) { typeID = (int)Type.SelectedValue; }
        }

        private void Product_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateItems(4);
        }

        private void Supplier_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateItems(5);
        }
        private void Adress_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateItems(6);
        }

        private void User_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateItems(7);
        }

        private string TwoStringToDecimal(string natural_number, string fraction)
        {
            string one = natural_number+'.'+fraction;
            return one;
        }

        private void ClearTextBoxes()
        {
            Current_Date.Text = DateTime.Now.ToString("yyyy.MM.dd");
            FixedAsset_Code.Text = "0";
            Depreciation_method.Text = "Amortyzacja liniowa";
            UpdateItems(1);
            UpdateItems(2);
            UpdateItems(3);
            UpdateItems(4);
            UpdateItems(5);
            UpdateItems(6);
            UpdateItems(7);
            Status.Items.Clear();
            Depreciation_rate.Clear();
            Date_of_aquisition.Text = String.Empty;
            Gross_orig_val1_TxtBox.Clear();
            Gross_orig_val2_TxtBox.Clear();
            Net_orig_val1_TxtBox.Clear();
            Net_orig_val2_TxtBox.Clear();
            Description_TxtBox.Clear();
            Invoice.Inlines.Clear();
            Guarantee.Clear();
        }

        private void Status_Loaded(object sender, RoutedEventArgs e)
        {
            Status.Items.Add("W użyciu");
            Status.Items.Add("Zlikwidowany");
        }

        private void Status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Status.Items.Count == 0)
            {
                Status.Items.Add("W użyciu");
                Status.Items.Add("Zlikwidowany");
            }
        }
    }
}
