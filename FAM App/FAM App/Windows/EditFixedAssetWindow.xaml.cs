using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
        int supplier, product, adress, user, depreciation, guarantee;
        string comments, revision_date, status, date_of_aquisition, gros_orig_value, net_orig_value, descritpion, invoice;
        bool haveUser;
        DataTable FixedAsset;

        public EditFixedAssetWindow(int fixedAsset_ID)
        {
            InitializeComponent();
            assetID = fixedAsset_ID;
            GetDataBaseData(fixedAsset_ID);
            SetProperties();

        }

        private void SetProperties()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString();
            Uri iconUri = new Uri(path + "../../../icon.ico", UriKind.RelativeOrAbsolute);

            this.Icon = BitmapFrame.Create(iconUri);
        }

        private void GetDataBaseData(int fixedAsset_ID)
        {
            DataBase dataBase = new DataBase();
            FixedAsset = new DataTable("emp");
            if (dataBase.DataBaseHaveUser(fixedAsset_ID))
            {
                FixedAsset = dataBase.GetAssetDataToEditWithUser(FixedAsset, fixedAsset_ID);
                LoadDataToBoxes_WithUser(FixedAsset);
                haveUser= true;
            }
            else
            {
                FixedAsset = dataBase.GetAssetDataToEditWithoutUser(FixedAsset, fixedAsset_ID);
                LoadDataToBoxes_WithoutUser(FixedAsset);
                haveUser = false;
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
                    String query = "SELECT ID_Produktu, CONCAT(Nazwa, ' ', Marka, ' ', Model, ' ', Rok_Produkcji) AS Produkt FROM dbo.Produkt;";
                    products = dataBase.DataBaseShowSelectedData(products, query);

                    Product.ItemsSource = products.DefaultView;
                    Product.DisplayMemberPath = "Produkt";
                    Product.SelectedValuePath = "ID_Produktu";
                }
                if (number == 5)
                {
                    DataBase dataBase = new DataBase();
                    DataTable suppliers = new DataTable();
                    String query = "SELECT ID_Dostawcy, CONCAT(Nazwa, ' ', Miejscowosc, ' ' , Kod_Pocztowy, ' ', Ulica) AS Dostawca FROM dbo.Dostawca;";
                    suppliers = dataBase.DataBaseShowSelectedData(suppliers, query);

                    Supplier.ItemsSource = suppliers.DefaultView;
                    Supplier.DisplayMemberPath = "Dostawca";
                    Supplier.SelectedValuePath = "ID_Dostawcy";
                }
                if (number == 6)
                {
                    DataBase dataBase = new DataBase();
                    DataTable adresses = new DataTable();
                    String query = "SELECT ID_Adresu, CONCAT(Nazwa, ' ', Miejscowosc, ' ', Kod_Pocztowy, ' ', Ulica, ' ', Nr_Budynku, ' ', Nr_Lokalu, ' ', Nr_Pomieszczenia) AS Adres FROM dbo.Adres;";
                    adresses = dataBase.DataBaseShowSelectedData(adresses, query);

                    Adress.ItemsSource = adresses.DefaultView;
                    Adress.DisplayMemberPath = "Adres";
                    Adress.SelectedValuePath = "ID_Adresu";
                }
                if (number == 7)
                {
                    DataBase dataBase = new DataBase();
                    DataTable employee = new DataTable();
                    String query = "SELECT ID_Pracownika, CONCAT(Imie, ' ', Nazwisko, ' ', Email) AS Pracownik FROM dbo.Pracownik;";
                    employee = dataBase.DataBaseShowSelectedData(employee, query);

                    User.ItemsSource = employee.DefaultView;
                    User.DisplayMemberPath = "Pracownik";
                    User.SelectedValuePath = "ID_Pracownika";
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void Group_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateItems(1);
            Group.SelectedValue = FixedAsset.Rows[0][0];
            Group.SelectedItem = Group.Items.GetItemAt(Group.SelectedIndex);
        }

        private void Subgroup_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateItems(2);
            Subgroup.SelectedValue = FixedAsset.Rows[0][1];
            Subgroup.SelectedItem = Subgroup.Items.GetItemAt(Subgroup.SelectedIndex);
        }

        private void Type_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateItems(3);
            Type.SelectedValue = FixedAsset.Rows[0][2];
            Type.SelectedItem = Type.Items.GetItemAt(Type.SelectedIndex);
        }

        private void Group_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Group.SelectedValue != null) { Subgroup.IsEnabled = true; groupID = (int)Group.SelectedValue;  }
        }
        private void Subgroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Subgroup.SelectedValue != null) { Type.IsEnabled = true; subgroupID = (int)Subgroup.SelectedValue;  }
        }

        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Type.SelectedValue != null) { typeID = (int)Type.SelectedValue; }
        }

        private void Product_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateItems(4);
            Product.SelectedValue = FixedAsset.Rows[0][3];
            Product.SelectedItem = Product.Items.GetItemAt(Product.SelectedIndex);

        }

        private void Supplier_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateItems(5);
            Supplier.SelectedValue = FixedAsset.Rows[0][4];
            Supplier.SelectedItem = Product.Items.GetItemAt(Supplier.SelectedIndex);
        }
        private void Adress_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateItems(6);
            Adress.SelectedValue = FixedAsset.Rows[0][5];
            Adress.SelectedItem = Adress.Items.GetItemAt(Adress.SelectedIndex);
        }

        private void User_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateItems(7);
            if(haveUser)
            {
                User.SelectedValue= FixedAsset.Rows[0][6];
                User.SelectedItem = User.Items.GetItemAt(User.SelectedIndex);
            }
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
            if (Adress.SelectedValue == null) { Adress.BorderBrush = Brushes.Red; return 0; }
            else { return (int)Adress.SelectedValue; }
        }

        private int CheckProduct()
        {
            if (Product.SelectedValue == null) { return 0; }
            else { return (int)Product.SelectedValue; }
        }

        private int CheckSupplier()
        {
            if (Supplier.SelectedValue == null) { return 0; }
            else { return (int)Supplier.SelectedValue; }
        }

        private int CheckIsUser()
        {
            if (User.SelectedValue == null) { return 0; }
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
                revision_date = DateTime.Now.ToString("yyyy-MM-dd");
                supplier = CheckSupplier();
                product = CheckProduct();
                adress = CheckAdress();
                user = CheckIsUser();
                status = Status.Text;
                if (Convert.ToInt32(FixedAsset.Rows[0][14]) != 0)
                {
                    depreciation = 0;
                }
                else 
                {
                    depreciation = Convert.ToInt32(Depreciation_rate.Text);
                }
                date_of_aquisition = ChangeFormatDate();
                gros_orig_value = TwoStringToDecimal(Gross_orig_val1_TxtBox.Text, Gross_orig_val2_TxtBox.Text);
                net_orig_value = TwoStringToDecimal(Net_orig_val1_TxtBox.Text, Net_orig_val2_TxtBox.Text);
                descritpion = Description_TxtBox.Text;
                invoice = Invoice.Text;
                guarantee = Convert.ToInt32(Guarantee.Text);

                if (supplier == 0 || product == 0 || adress == 0 || status == String.Empty || Type.SelectedValue == null) { MessageBox.Show("Należy Wprowadzić dane w polach wyboru!"); }
                else
                {
                    HaveComments();
                }
            }
        }

        private void SaveToBase(string comments)
        {
            try
            {
                DataBase dataBase = new DataBase();
                bool check = dataBase.UpdateFixedAsset(assetID, revision_date, supplier, product, adress, status, depreciation, date_of_aquisition, gros_orig_value, net_orig_value, descritpion, invoice, guarantee, groupID, subgroupID, typeID, user, comments);
                if (check)
                {
                    MessageBox.Show("Zapisano zmiany");
                    this.Close();
                }
                else { MessageBox.Show("Błąd przy wstawianiu danych do bazy!"); }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void HaveComments()
        {
            InputBox.Visibility= Visibility.Visible;
        }

        private void OkComment_Button_Click(object sender, RoutedEventArgs e)
        {
            if (CommentInput_TextBox.Text != String.Empty)
            {
                comments = CommentInput_TextBox.Text;
                SaveToBase(comments);
                InputBox.Visibility = Visibility.Hidden;
            }
            else { MessageBox.Show("Pole jest puste"); }
        }

        private void CancelComment_Button_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = Visibility.Hidden;
        }
    }
}
