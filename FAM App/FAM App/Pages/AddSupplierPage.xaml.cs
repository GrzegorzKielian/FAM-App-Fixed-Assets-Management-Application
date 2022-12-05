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
                    string PostCode = PostCode_TextBox1.Text +"-"+ PostCode_TextBox2.Text;
                    string Street = Street_TextBox.Text;

                    DataBase dataBase = new DataBase();
                    bool check = dataBase.AddSupplierToBase(Name, City, PostCode, Street);
                    if (check)
                    {
                        MessageBox.Show("Dodano do bazy:\n" + Name + "\n" + City + "\n" + PostCode + "\n" + Street);
                        ClearTextBoxes();
                    }
                    else { MessageBox.Show("Błąd przy wstawianiu danych do bazy!"); }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ClearTextBoxes()
        {
            Name_TextBox.Clear();
            City_TextBox.Clear();
            PostCode_TextBox1.Clear();
            PostCode_TextBox2.Clear();
            Street_TextBox.Clear();
        }
    }
}
