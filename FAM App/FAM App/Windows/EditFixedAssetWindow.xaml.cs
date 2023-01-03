using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace FAM_App.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy EditFixedAssetWindow.xaml
    /// </summary>
    public partial class EditFixedAssetWindow : Window
    {
        private int groupID;
        private int subgroupID;
        private int typeID;
        private int assetID;
        string comments;
        DataTable FixedAsset;

        public EditFixedAssetWindow(int fixedAsset_ID)
        {
            InitializeComponent();
            assetID = fixedAsset_ID;
            GetDataBaseData(fixedAsset_ID);
            
        }

        private void GetDataBaseData(int fixedAsset_ID)
        {
            DataBase dataBase = new DataBase();
            FixedAsset = new DataTable("emp");
            if (dataBase.DataBaseHaveUser(fixedAsset_ID))
            {
                FixedAsset = dataBase.GetAssetDataToEditWithUser(FixedAsset, fixedAsset_ID);
                LoadDataToBoxes_WithUser(FixedAsset);
            }
            else
            {
                FixedAsset = dataBase.GetAssetDataToEditWithoutUser(FixedAsset, fixedAsset_ID);
                LoadDataToBoxes_WithoutUser(FixedAsset);
            }       
        }

        private void LoadDataToBoxes_WithUser(DataTable fixedAsset)
        {
            try
            {
                bool dep = true;
                int iF = Convert.ToInt32(fixedAsset.Rows[0][15]);
                if (iF != 0)
                {
                    dep = false;
                    Depreciation.Visibility = Visibility.Hidden;
                }
                Group.Text = fixedAsset.Rows[0][0].ToString();
                Subgroup.Text = fixedAsset.Rows[0][1].ToString();
                Type.Text = fixedAsset.Rows[0][2].ToString();

                Product.Text = fixedAsset.Rows[0][3].ToString();
                Supplier.Text = fixedAsset.Rows[0][4].ToString();
                Adress.Text = fixedAsset.Rows[0][5].ToString();
                User.Text = fixedAsset.Rows[0][6].ToString();

                if (dep)
                {
                    Depreciation_rate.Text = fixedAsset.Rows[0][14].ToString();
                    Depreciation_method.Text = "Amortyzacja liniowa";
                }
                Date_of_aquisition.Text = fixedAsset.Rows[0][7].ToString();
                string[] grossVal = SplitValue(fixedAsset.Rows[0][8].ToString());
                string[] netVal = SplitValue(fixedAsset.Rows[0][9].ToString());
                Gross_orig_val1_TxtBox.Text = grossVal[0];
                Gross_orig_val2_TxtBox.Text = grossVal[1];
                Net_orig_val1_TxtBox.Text = netVal[0];
                Net_orig_val2_TxtBox.Text = netVal[1];
                Description_TxtBox.Text = fixedAsset.Rows[0][10].ToString();
                Status.Text = fixedAsset.Rows[0][13].ToString();
                Invoice.Text = fixedAsset.Rows[0][11].ToString();
                Guarantee.Text = fixedAsset.Rows[0][12].ToString();
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void LoadDataToBoxes_WithoutUser(DataTable fixedAsset)
        {
            try
            {
                bool dep = true;
                int iF = Convert.ToInt32(fixedAsset.Rows[0][14]);
                if (iF != 0)
                {
                    dep = false;
                    Depreciation.Visibility = Visibility.Hidden;
                }
                Group.Text = fixedAsset.Rows[0][0].ToString();
                Subgroup.Text = fixedAsset.Rows[0][1].ToString();
                Type.Text = fixedAsset.Rows[0][2].ToString();

                Product.Text = fixedAsset.Rows[0][3].ToString();
                Supplier.Text = fixedAsset.Rows[0][4].ToString();
                Adress.Text = fixedAsset.Rows[0][5].ToString();

                if (dep)
                {
                    Depreciation_rate.Text = fixedAsset.Rows[0][13].ToString();
                    Depreciation_method.Text = "Amortyzacja liniowa";
                }
                Date_of_aquisition.Text = fixedAsset.Rows[0][6].ToString();
                string[] grossVal = SplitValue(fixedAsset.Rows[0][7].ToString());
                string[] netVal = SplitValue(fixedAsset.Rows[0][8].ToString());
                Gross_orig_val1_TxtBox.Text = grossVal[0];
                Gross_orig_val2_TxtBox.Text = grossVal[1];
                Net_orig_val1_TxtBox.Text = netVal[0];
                Net_orig_val2_TxtBox.Text = netVal[1];
                Description_TxtBox.Text = fixedAsset.Rows[0][9].ToString();
                Status.Text = fixedAsset.Rows[0][12].ToString();
                Invoice.Text = fixedAsset.Rows[0][10].ToString();
                Guarantee.Text = fixedAsset.Rows[0][11].ToString();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private string[] SplitValue(string v)
        {
            string[] s = v.Split(',',2);
            return s;
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
                if (number == 6)
                {
                    DataBase dataBase = new DataBase();
                    DataTable adresses = new DataTable();
                    adresses = dataBase.DataBaseShowAdresses(adresses);

                    Adress.ItemsSource = adresses.DefaultView;
                    Adress.SelectedValuePath = "ID_Adresu";
                }
                if (number == 7)
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
            string one = natural_number + '.' + fraction;
            return one;
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
            if (Supplier.Text == String.Empty) { return Convert.ToInt32(""); }
            else { return (int)Supplier.SelectedValue; }
        }

        private int CheckIsUser()
        {
            if (User.Text == String.Empty) { return 0; }
            else { return (int)User.SelectedValue; }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz anulować?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void EditFixedAssetButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz zapisać?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                string revision_date = DateTime.Now.ToString("yyyy-MM-dd");
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

                if (supplier == 0 || product == 0 || adress == 0 || status == String.Empty) { }
                else
                {
                    DataBase dataBase = new DataBase();
                    bool check = dataBase.UpdateFixedAsset(assetID, revision_date, supplier, product, adress, status, depreciation, date_of_aquisition, gros_orig_value, net_orig_value, descritpion, invoice, guarantee, groupID, subgroupID, typeID, user);
                    if (check)
                    {
                        MessageBox.Show("Zapisano zmiany");
                    }
                    else { MessageBox.Show("Błąd przy wstawianiu danych do bazy!"); }
                }
            }
        }

    }
}
