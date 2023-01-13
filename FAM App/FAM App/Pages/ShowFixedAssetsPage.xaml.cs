using FAM_App.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
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
using System.Xml.Linq;

namespace FAM_App
{
    /// <summary>
    /// Logika interakcji dla klasy ShowFixedAssetsPage.xaml
    /// </summary>
    public partial class ShowFixedAssetsPage : Page
    {
        List<String> query = new List<String>();

        private int groupID;
        private int subgroupID;
        private int typeID;
        bool IsChecked = false;

        public ShowFixedAssetsPage()
        {
            InitializeComponent();
            LoadAllFixedAssets();
            UserSearch_TxtBox.Text = String.Empty;
        }

        private void LoadAllFixedAssets()
        {
            DataBase dataBase = new DataBase();
            DataTable FixedAssets = new DataTable("emp");
            FixedAssets = dataBase.DataBaseShowFixedAssets(FixedAssets);
            FixedAssetsDataGrid.ItemsSource = FixedAssets.DefaultView;
            FixedAssetsDataGrid.CanUserAddRows = false;
        }

        private void LoadOnlyFixedAssets()
        {
            DataBase dataBase = new DataBase();
            DataTable OnlyFixedAssets = new DataTable("emp");
            string query = "SELECT dbo.Srodek_Trwaly.ID_Srodka, dbo.Srodek_Trwaly.Kod_Srodka, dbo.Srodek_Trwaly.Nr_Inwentarzowy, dbo.Srodek_Trwaly.Stan_Status, dbo.Grupa.Nazwa AS Grupa, dbo.Podgrupa.Nazwa AS Podgrupa, dbo.Rodzaj.Nazwa AS Rodzaj, dbo.Produkt.Nazwa AS Produkt, dbo.Srodek_Trwaly.Opis, dbo.Srodek_Trwaly.Data_Nabycia, dbo.Srodek_Trwaly.Data_Likwidacji, dbo.Srodek_Trwaly.Data_Wprowadzenia, dbo.Srodek_Trwaly.Wartosc_Poczatkowa_Netto, dbo.Srodek_Trwaly.Wartosc_Poczatkowa_Brutto, dbo.Dostawca.Nazwa AS Dostawca, dbo.Srodek_Trwaly.Faktura, dbo.Srodek_Trwaly.Gwarancja, dbo.Srodek_Trwaly.Stawka_Amortyzacji " +
                "FROM dbo.Produkt " +
                "INNER JOIN dbo.Srodek_Trwaly " +
                "INNER JOIN dbo.Dostawca ON dbo.Srodek_Trwaly.id_dostawcy = dbo.Dostawca.ID_Dostawcy ON dbo.Produkt.ID_Produktu = dbo.Srodek_Trwaly.id_Produktu " +
                "INNER JOIN dbo.Rodzaj ON dbo.Srodek_Trwaly.id_nr_klasyfikacyjny = dbo.Rodzaj.ID_Rodzaju " +
                "INNER JOIN dbo.Podgrupa " +
                "INNER JOIN dbo.Grupa ON dbo.Podgrupa.id_grupy = dbo.Grupa.ID_Grupy ON dbo.Rodzaj.id_podgrupy = dbo.Podgrupa.ID_Podgrupy " +
                "WHERE (dbo.Srodek_Trwaly.Kod_Srodka = 0);";
            OnlyFixedAssets = dataBase.DataBaseShowSelectedData(OnlyFixedAssets, query);
            FixedAssetsDataGrid.ItemsSource = OnlyFixedAssets.DefaultView;
            FixedAssetsDataGrid.CanUserAddRows = false;
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
                if (FixedAssetsDataGrid.SelectedItem == null) return;
                DataRowView dr = FixedAssetsDataGrid.SelectedItem as DataRowView;
                DataRow dr1 = dr.Row;
                int id_of_the_edited_asset = Convert.ToInt32(dr1.ItemArray[0]);
                EditFixedAssetWindow editFixedAsset = new EditFixedAssetWindow(id_of_the_edited_asset);
                editFixedAsset.Show();
        }

        private void DataGridRow_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsChecked)
            {
                ContextMenu = (ContextMenu)Resources["RightClickMenu"];
            }
            else { ContextMenu = null; }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Depreciation_Box.Text = "";
            DataRowView dr = FixedAssetsDataGrid.SelectedItem as DataRowView;
            DataRow dr1 = dr.Row;
            decimal rateInPercents = (Convert.ToDecimal(dr1.ItemArray[17]))/100;
            decimal gross_orig_val = Convert.ToDecimal(dr1.ItemArray[13]);
            decimal[] yearsRates = CalculateDepreciation(gross_orig_val, rateInPercents);
            LoadDataToDepreciationTextBlock(yearsRates);
        }

        private void LoadDataToDepreciationTextBlock(decimal[] yearsRates)
        {
            Depreciation_Box.Text = "Wartość początkowa: " + yearsRates[0] + "\n";
            Depreciation_Box.Text += "Stawka amortyzacji: " + yearsRates[1] + "\n";
            for(int i=2;i<yearsRates.Length;i++) 
            {
                Depreciation_Box.Text += "Rok " + (i - 1) + ": " + yearsRates[i] + "\n";
            }
        }

        private decimal[] CalculateDepreciation(decimal gross_orig_val, decimal rateInPercents)
        {
            int length = (int)(1 / rateInPercents);
            decimal[] decimals= new decimal[length+2];
            decimals[0] = gross_orig_val;
            decimals[1] = gross_orig_val*rateInPercents;
            decimal val = gross_orig_val;
            for(int i=2;i<decimals.Length;i++)
            {
                if (val - decimals[1] > 0)
                {
                    decimals[i] = val - decimals[1];
                    val = decimals[i];
                }
                else 
                {
                    decimals[i] = 0;
                }
            }
            return decimals;
        }

        private void Depreciation_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Depreciation_Border.Visibility = Visibility.Visible;
            Group.Text = "";
            Subgroup.Text = string.Empty;
            Type.Text = string.Empty;
            LoadOnlyFixedAssets();
            IsChecked = true;
        }

        private void Depreciation_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Depreciation_Border.Visibility = Visibility.Hidden;
            Depreciation_Box.Text = string.Empty;
            LoadAllFixedAssets();
            IsChecked = false;
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
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
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

        private void FiltrSearch_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String query = SwitchQuery();

                DataBase dataBase = new DataBase();
                DataTable filterFixedAssets = new DataTable();
                filterFixedAssets = dataBase.DataBaseShowSelectedData(filterFixedAssets, query);
                FixedAssetsDataGrid.ItemsSource = filterFixedAssets.DefaultView;
                FixedAssetsDataGrid.CanUserAddRows = false;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private string SwitchQuery()
        {
            String query;
            if (IsChecked)
            {
                if(UserSearch_TxtBox.Text != String.Empty)
                {
                    if (Type.SelectedValue != null)
                    {
                        query = "SELECT dbo.Srodek_Trwaly.ID_Srodka, dbo.Srodek_Trwaly.Kod_Srodka, dbo.Srodek_Trwaly.Nr_Inwentarzowy, dbo.Srodek_Trwaly.Stan_Status, dbo.Grupa.Nazwa AS Grupa, dbo.Podgrupa.Nazwa AS Podgrupa, dbo.Rodzaj.Nazwa AS Rodzaj, dbo.Produkt.Nazwa AS Produkt, dbo.Srodek_Trwaly.Opis, dbo.Srodek_Trwaly.Data_Nabycia, dbo.Srodek_Trwaly.Data_Likwidacji, dbo.Srodek_Trwaly.Data_Wprowadzenia, dbo.Srodek_Trwaly.Wartosc_Poczatkowa_Netto, dbo.Srodek_Trwaly.Wartosc_Poczatkowa_Brutto, dbo.Dostawca.Nazwa AS Dostawca, dbo.Srodek_Trwaly.Faktura, dbo.Srodek_Trwaly.Gwarancja, dbo.Srodek_Trwaly.Stawka_Amortyzacji " +
                                "FROM dbo.Dostawca " +
                                "INNER JOIN dbo.Pracownik " +
                                "INNER JOIN dbo.Historia_Srodka ON dbo.Pracownik.ID_Pracownika = dbo.Historia_Srodka.id_uzytkownika " +
                                "INNER JOIN dbo.Rodzaj " +
                                "INNER JOIN dbo.Podgrupa " +
                                "INNER JOIN dbo.Grupa ON dbo.Podgrupa.id_grupy = dbo.Grupa.ID_Grupy ON dbo.Rodzaj.id_podgrupy = dbo.Podgrupa.ID_Podgrupy " +
                                "INNER JOIN dbo.Srodek_Trwaly ON dbo.Rodzaj.ID_Rodzaju = dbo.Srodek_Trwaly.id_nr_klasyfikacyjny " +
                                "INNER JOIN dbo.Produkt ON dbo.Srodek_Trwaly.id_produktu = dbo.Produkt.ID_Produktu ON dbo.Historia_Srodka.id_srodka = dbo.Srodek_Trwaly.ID_Srodka ON dbo.Dostawca.ID_Dostawcy = dbo.Srodek_Trwaly.id_dostawcy " +
                                "WHERE (dbo.Srodek_Trwaly.id_nr_klasyfikacyjny = " + typeID + " AND dbo.Srodek_Trwaly.Kod_Srodka = 0 AND dbo.Pracownik.ID_Pracownika = (SELECT dbo.Pracownik.ID_Pracownika FROM dbo.Pracownik WHERE dbo.Pracownik.Nazwisko LIKE '" + UserSearch_TxtBox.Text + "'));";
                        return query;
                    }
                    else
                    {
                        query = "SELECT dbo.Srodek_Trwaly.ID_Srodka, dbo.Srodek_Trwaly.Kod_Srodka, dbo.Srodek_Trwaly.Nr_Inwentarzowy, dbo.Srodek_Trwaly.Stan_Status, dbo.Grupa.Nazwa AS Grupa, dbo.Podgrupa.Nazwa AS Podgrupa, dbo.Rodzaj.Nazwa AS Rodzaj, dbo.Produkt.Nazwa AS Produkt, dbo.Srodek_Trwaly.Opis, dbo.Srodek_Trwaly.Data_Nabycia, dbo.Srodek_Trwaly.Data_Likwidacji, dbo.Srodek_Trwaly.Data_Wprowadzenia, dbo.Srodek_Trwaly.Wartosc_Poczatkowa_Netto, dbo.Srodek_Trwaly.Wartosc_Poczatkowa_Brutto, dbo.Dostawca.Nazwa AS Dostawca, dbo.Srodek_Trwaly.Faktura, dbo.Srodek_Trwaly.Gwarancja, dbo.Srodek_Trwaly.Stawka_Amortyzacji " +
                                "FROM dbo.Dostawca " +
                                "INNER JOIN dbo.Pracownik " +
                                "INNER JOIN dbo.Historia_Srodka ON dbo.Pracownik.ID_Pracownika = dbo.Historia_Srodka.id_uzytkownika " +
                                "INNER JOIN dbo.Rodzaj " +
                                "INNER JOIN dbo.Podgrupa " +
                                "INNER JOIN dbo.Grupa ON dbo.Podgrupa.id_grupy = dbo.Grupa.ID_Grupy ON dbo.Rodzaj.id_podgrupy = dbo.Podgrupa.ID_Podgrupy " +
                                "INNER JOIN dbo.Srodek_Trwaly ON dbo.Rodzaj.ID_Rodzaju = dbo.Srodek_Trwaly.id_nr_klasyfikacyjny " +
                                "INNER JOIN dbo.Produkt ON dbo.Srodek_Trwaly.id_produktu = dbo.Produkt.ID_Produktu ON dbo.Historia_Srodka.id_srodka = dbo.Srodek_Trwaly.ID_Srodka ON dbo.Dostawca.ID_Dostawcy = dbo.Srodek_Trwaly.id_dostawcy " +
                                "WHERE (dbo.Srodek_Trwaly.Kod_Srodka = 0 AND dbo.Pracownik.ID_Pracownika = (SELECT dbo.Pracownik.ID_Pracownika FROM dbo.Pracownik WHERE dbo.Pracownik.Nazwisko LIKE '" + UserSearch_TxtBox.Text + "'));";
                        return query;
                    }

                }
                else
                {
                    query = "SELECT dbo.Srodek_Trwaly.ID_Srodka, dbo.Srodek_Trwaly.Kod_Srodka, dbo.Srodek_Trwaly.Nr_Inwentarzowy, dbo.Srodek_Trwaly.Stan_Status, dbo.Grupa.Nazwa AS Grupa, dbo.Podgrupa.Nazwa AS Podgrupa, dbo.Rodzaj.Nazwa AS Rodzaj, dbo.Produkt.Nazwa AS Produkt, dbo.Srodek_Trwaly.Opis, dbo.Srodek_Trwaly.Data_Nabycia, dbo.Srodek_Trwaly.Data_Likwidacji, dbo.Srodek_Trwaly.Data_Wprowadzenia, dbo.Srodek_Trwaly.Wartosc_Poczatkowa_Netto, dbo.Srodek_Trwaly.Wartosc_Poczatkowa_Brutto, dbo.Dostawca.Nazwa AS Dostawca, dbo.Srodek_Trwaly.Faktura, dbo.Srodek_Trwaly.Gwarancja, dbo.Srodek_Trwaly.Stawka_Amortyzacji " +
                            "FROM dbo.Dostawca " +
                            "INNER JOIN dbo.Pracownik " +
                            "INNER JOIN dbo.Historia_Srodka ON dbo.Pracownik.ID_Pracownika = dbo.Historia_Srodka.id_uzytkownika " +
                            "INNER JOIN dbo.Rodzaj " +
                            "INNER JOIN dbo.Podgrupa " +
                            "INNER JOIN dbo.Grupa ON dbo.Podgrupa.id_grupy = dbo.Grupa.ID_Grupy ON dbo.Rodzaj.id_podgrupy = dbo.Podgrupa.ID_Podgrupy " +
                            "INNER JOIN dbo.Srodek_Trwaly ON dbo.Rodzaj.ID_Rodzaju = dbo.Srodek_Trwaly.id_nr_klasyfikacyjny " +
                            "INNER JOIN dbo.Produkt ON dbo.Srodek_Trwaly.id_produktu = dbo.Produkt.ID_Produktu ON dbo.Historia_Srodka.id_srodka = dbo.Srodek_Trwaly.ID_Srodka ON dbo.Dostawca.ID_Dostawcy = dbo.Srodek_Trwaly.id_dostawcy " +
                            "WHERE (dbo.Srodek_Trwaly.id_nr_klasyfikacyjny = " + typeID + " AND dbo.Srodek_Trwaly.Kod_Srodka = 0);";
                    return query;
                }

            }
            else
            {
                if (UserSearch_TxtBox.Text != String.Empty)
                {
                    if (Type.SelectedValue != null)
                    {
                        query = "SELECT dbo.Srodek_Trwaly.ID_Srodka, dbo.Srodek_Trwaly.Kod_Srodka, dbo.Srodek_Trwaly.Nr_Inwentarzowy, dbo.Srodek_Trwaly.Stan_Status, dbo.Grupa.Nazwa AS Grupa, dbo.Podgrupa.Nazwa AS Podgrupa, dbo.Rodzaj.Nazwa AS Rodzaj, dbo.Produkt.Nazwa AS Produkt, dbo.Srodek_Trwaly.Opis, dbo.Srodek_Trwaly.Data_Nabycia, dbo.Srodek_Trwaly.Data_Likwidacji, dbo.Srodek_Trwaly.Data_Wprowadzenia, dbo.Srodek_Trwaly.Wartosc_Poczatkowa_Netto, dbo.Srodek_Trwaly.Wartosc_Poczatkowa_Brutto, dbo.Dostawca.Nazwa AS Dostawca, dbo.Srodek_Trwaly.Faktura, dbo.Srodek_Trwaly.Gwarancja, dbo.Srodek_Trwaly.Stawka_Amortyzacji " +
                                "FROM dbo.Dostawca " +
                                "INNER JOIN dbo.Pracownik " +
                                "INNER JOIN dbo.Historia_Srodka ON dbo.Pracownik.ID_Pracownika = dbo.Historia_Srodka.id_uzytkownika " +
                                "INNER JOIN dbo.Rodzaj " +
                                "INNER JOIN dbo.Podgrupa " +
                                "INNER JOIN dbo.Grupa ON dbo.Podgrupa.id_grupy = dbo.Grupa.ID_Grupy ON dbo.Rodzaj.id_podgrupy = dbo.Podgrupa.ID_Podgrupy " +
                                "INNER JOIN dbo.Srodek_Trwaly ON dbo.Rodzaj.ID_Rodzaju = dbo.Srodek_Trwaly.id_nr_klasyfikacyjny " +
                                "INNER JOIN dbo.Produkt ON dbo.Srodek_Trwaly.id_produktu = dbo.Produkt.ID_Produktu ON dbo.Historia_Srodka.id_srodka = dbo.Srodek_Trwaly.ID_Srodka ON dbo.Dostawca.ID_Dostawcy = dbo.Srodek_Trwaly.id_dostawcy " +
                                "WHERE (dbo.Srodek_Trwaly.id_nr_klasyfikacyjny = " + typeID + " AND dbo.Pracownik.ID_Pracownika = (SELECT dbo.Pracownik.ID_Pracownika FROM dbo.Pracownik WHERE dbo.Pracownik.Nazwisko LIKE '" + UserSearch_TxtBox.Text + "'));";
                        return query;
                    }
                    else 
                    {
                        query = "SELECT dbo.Srodek_Trwaly.ID_Srodka, dbo.Srodek_Trwaly.Kod_Srodka, dbo.Srodek_Trwaly.Nr_Inwentarzowy, dbo.Srodek_Trwaly.Stan_Status, dbo.Grupa.Nazwa AS Grupa, dbo.Podgrupa.Nazwa AS Podgrupa, dbo.Rodzaj.Nazwa AS Rodzaj, dbo.Produkt.Nazwa AS Produkt, dbo.Srodek_Trwaly.Opis, dbo.Srodek_Trwaly.Data_Nabycia, dbo.Srodek_Trwaly.Data_Likwidacji, dbo.Srodek_Trwaly.Data_Wprowadzenia, dbo.Srodek_Trwaly.Wartosc_Poczatkowa_Netto, dbo.Srodek_Trwaly.Wartosc_Poczatkowa_Brutto, dbo.Dostawca.Nazwa AS Dostawca, dbo.Srodek_Trwaly.Faktura, dbo.Srodek_Trwaly.Gwarancja, dbo.Srodek_Trwaly.Stawka_Amortyzacji " +
                                "FROM dbo.Dostawca " +
                                "INNER JOIN dbo.Pracownik " +
                                "INNER JOIN dbo.Historia_Srodka ON dbo.Pracownik.ID_Pracownika = dbo.Historia_Srodka.id_uzytkownika " +
                                "INNER JOIN dbo.Rodzaj " +
                                "INNER JOIN dbo.Podgrupa " +
                                "INNER JOIN dbo.Grupa ON dbo.Podgrupa.id_grupy = dbo.Grupa.ID_Grupy ON dbo.Rodzaj.id_podgrupy = dbo.Podgrupa.ID_Podgrupy " +
                                "INNER JOIN dbo.Srodek_Trwaly ON dbo.Rodzaj.ID_Rodzaju = dbo.Srodek_Trwaly.id_nr_klasyfikacyjny " +
                                "INNER JOIN dbo.Produkt ON dbo.Srodek_Trwaly.id_produktu = dbo.Produkt.ID_Produktu ON dbo.Historia_Srodka.id_srodka = dbo.Srodek_Trwaly.ID_Srodka ON dbo.Dostawca.ID_Dostawcy = dbo.Srodek_Trwaly.id_dostawcy " +
                                "WHERE (dbo.Pracownik.ID_Pracownika = (SELECT dbo.Pracownik.ID_Pracownika FROM dbo.Pracownik WHERE dbo.Pracownik.Nazwisko LIKE '" + UserSearch_TxtBox.Text + "'));";
                        return query;
                    }

                }
                else
                {
                    query = "SELECT dbo.Srodek_Trwaly.ID_Srodka, dbo.Srodek_Trwaly.Kod_Srodka, dbo.Srodek_Trwaly.Nr_Inwentarzowy, dbo.Srodek_Trwaly.Stan_Status, dbo.Grupa.Nazwa AS Grupa, dbo.Podgrupa.Nazwa AS Podgrupa, dbo.Rodzaj.Nazwa AS Rodzaj, dbo.Produkt.Nazwa AS Produkt, dbo.Srodek_Trwaly.Opis, dbo.Srodek_Trwaly.Data_Nabycia, dbo.Srodek_Trwaly.Data_Likwidacji, dbo.Srodek_Trwaly.Data_Wprowadzenia, dbo.Srodek_Trwaly.Wartosc_Poczatkowa_Netto, dbo.Srodek_Trwaly.Wartosc_Poczatkowa_Brutto, dbo.Dostawca.Nazwa AS Dostawca, dbo.Srodek_Trwaly.Faktura, dbo.Srodek_Trwaly.Gwarancja, dbo.Srodek_Trwaly.Stawka_Amortyzacji " +
                        "FROM dbo.Dostawca " +
                        "INNER JOIN dbo.Pracownik " +
                        "INNER JOIN dbo.Historia_Srodka ON dbo.Pracownik.ID_Pracownika = dbo.Historia_Srodka.id_uzytkownika " +
                        "INNER JOIN dbo.Rodzaj " +
                        "INNER JOIN dbo.Podgrupa " +
                        "INNER JOIN dbo.Grupa ON dbo.Podgrupa.id_grupy = dbo.Grupa.ID_Grupy ON dbo.Rodzaj.id_podgrupy = dbo.Podgrupa.ID_Podgrupy " +
                        "INNER JOIN dbo.Srodek_Trwaly ON dbo.Rodzaj.ID_Rodzaju = dbo.Srodek_Trwaly.id_nr_klasyfikacyjny " +
                        "INNER JOIN dbo.Produkt ON dbo.Srodek_Trwaly.id_produktu = dbo.Produkt.ID_Produktu ON dbo.Historia_Srodka.id_srodka = dbo.Srodek_Trwaly.ID_Srodka ON dbo.Dostawca.ID_Dostawcy = dbo.Srodek_Trwaly.id_dostawcy " +
                        "WHERE (dbo.Srodek_Trwaly.id_nr_klasyfikacyjny = " + typeID + ");";
                    return query;
                }
            }
        }

        private void ClearUserSearch_TxtBox_Click(object sender, RoutedEventArgs e)
        {
            UserSearch_TxtBox.Text = String.Empty;
            ClearUserSearch_TxtBox.Visibility = Visibility.Hidden;
        }

        private void UserSearch_TxtBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (UserSearch_TxtBox.Text != String.Empty)
            { 
                ClearUserSearch_TxtBox.Visibility = Visibility.Visible;
            }
        }
    }

}

