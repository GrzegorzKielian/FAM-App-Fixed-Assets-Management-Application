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
    /// Logika interakcji dla klasy AddSupplierPage.xaml
    /// </summary>
    public partial class AddSupplierPage : Page
    {
        public AddSupplierPage()
        {
            InitializeComponent();
        }
        private void AddSupplier_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (MessageBox.Show("Czy na pewno chcesz dodać nowego dostawce do bazy danych?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    string Name = Name_TextBox.Text;
                    string City = City_TextBox.Text;
                    string PostCode = PostCode_TextBox.Text;
                    string Street = Street_TextBox.Text;

                    DataBase dataBase = new DataBase();
                    dataBase.AddSupplierToBase(Name, City, PostCode, Street);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
