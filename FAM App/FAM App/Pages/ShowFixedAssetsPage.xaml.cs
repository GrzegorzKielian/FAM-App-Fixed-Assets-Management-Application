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
            datagrid1.ItemsSource = FixedAssets.DefaultView;
        }
    }
}
