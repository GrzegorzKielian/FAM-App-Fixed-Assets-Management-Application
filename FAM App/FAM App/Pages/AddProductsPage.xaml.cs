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

namespace FAM_App.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy AddProductsPage.xaml
    /// </summary>
    public partial class AddProductsPage : Page
    {
        public AddProductsPage()
        {
            InitializeComponent();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Czy na pewno chcesz dodać nowy produkt do bazy danych?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    string Name = NameTextBox.Text;
                    string Brand = BrandTextBox.Text;
                    string Model = ModelTextBox.Text;
                    string Description = DescriptionTextBox.Text;
                    string Year = YearTextBox.Text;

                    DataBase dataBase = new DataBase();
                    dataBase.AddProductToBase(Name, Brand, Model, Description, Year);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
