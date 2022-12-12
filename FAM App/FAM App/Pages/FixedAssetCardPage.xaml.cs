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
    /// Logika interakcji dla klasy FixedAssetCardPage.xaml
    /// </summary>
    public partial class FixedAssetCardPage : Page
    {
        public FixedAssetCardPage()
        {
            InitializeComponent();
        }

        private void ShowHistory_Button_Click(object sender, RoutedEventArgs e)
        {
            string inventoryNumber = InventoryNumber_TxtBox.Text;
            DataBase dataBase = new DataBase();
            DataTable history = new DataTable("emp");
            history = dataBase.DataBaseShowFixedAssetHistory(history,inventoryNumber);
            HistoryDataGrid.ItemsSource = history.DefaultView;
            ClearTextBoxes();

        }

        private void ClearTextBoxes()
        {
            InventoryNumber_TxtBox.Clear();
        }
    }
}
