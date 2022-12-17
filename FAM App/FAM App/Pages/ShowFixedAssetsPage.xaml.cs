using FAM_App.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Xml.Linq;

namespace FAM_App
{
    /// <summary>
    /// Logika interakcji dla klasy ShowFixedAssetsPage.xaml
    /// </summary>
    public partial class ShowFixedAssetsPage : Page
    {
        public ShowFixedAssetsPage()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            DataBase dataBase = new DataBase();
            DataTable FixedAssets = new DataTable("emp");
            FixedAssets = dataBase.DataBaseShowFixedAssets(FixedAssets);
            FixedAssetsDataGrid.ItemsSource = FixedAssets.DefaultView;
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (FixedAssetsDataGrid.SelectedItem == null)
                return;
            DataRowView dr = FixedAssetsDataGrid.SelectedItem as DataRowView;
            DataRow dr1 = dr.Row;
            int id_of_the_edited_asset = Convert.ToInt32(dr1.ItemArray[0]);
            EditFixedAssetWindow editFixedAsset = new EditFixedAssetWindow(id_of_the_edited_asset);
            editFixedAsset.Show();

        }

    }

}

