using System;
using System.Collections.Generic;
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
    /// Logika interakcji dla klasy AddFixedAssetsPage.xaml
    /// </summary>
    public partial class AddFixedAssetsPage : Page
    {

        public AddFixedAssetsPage()
        {
            InitializeComponent();
            SetFixedAssetData();
        }
        private void SetFixedAssetData()
        {
            Current_Date.Text = DateTime.Now.ToString("yyyy.MM.dd");
            FixedAsset_Code.Text = "0";
            Depreciation_method.Text = "Amortyzacja liniowa";
        }

        private void AddFixedAsset_Click(object sender, RoutedEventArgs e)
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

    }
}
