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
                    string Name = Name_TextBox.Text;
                    string Brand = Brand_TextBox.Text;
                    string Model = Model_TextBox.Text;
                    string Description = Description_TextBox.Text;
                    string Year = Year_TextBox.Text;

                    DataBase dataBase = new DataBase();
                    bool check = dataBase.AddProductToBase(Name, Brand, Model, Description, Year);
                    if (check)
                    {
                        MessageBox.Show("Dodano do bazy:\n" + Name_TextBox.Text+"\n" + Brand_TextBox.Text + "\n" + Model_TextBox.Text + "\n" + Year_TextBox.Text + "\n" + Description_TextBox.Text + "\n");
                        ClearTextBoxes();
                    }
                    else { MessageBox.Show("Error inserting data into Database!"); }
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
            Brand_TextBox.Clear();
            Model_TextBox.Clear();
            Description_TextBox.Clear();
            Year_TextBox.Clear();
        }
    }
}
