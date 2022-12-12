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
    /// Logika interakcji dla klasy AddAdressesPage.xaml
    /// </summary>
    public partial class AddAdressesPage : Page
    {
        public AddAdressesPage()
        {
            InitializeComponent();
        }

        private void AddAdress_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Czy na pewno chcesz dodać nowego dostawce do bazy danych?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    
                    string city = City_TextBox.Text;
                    string postCode = PostCode_TextBox1.Text + "-" + PostCode_TextBox2.Text;
                    string street = Street_TextBox.Text;
                    string buildingNumber = BuildingNumber_TextBox.Text;
                    string apartmentNumber = ApartmentNumber_TextBox.Text;
                    string roomNumber = RoomNumber_TextBox.Text;
                    string name = Name_TextBox.Text;
                    string additionalInformation = AdditionalInformation_TextBox.Text;

                    DataBase dataBase = new DataBase();
                    bool check = dataBase.AddAdressToBase(city, postCode, street, buildingNumber, apartmentNumber, roomNumber, name, additionalInformation);
                    if (check)
                    {
                        MessageBox.Show("Dodano do bazy:\n" + city + "\n" + postCode + "\n" + street + "\n" + buildingNumber + "\n" + apartmentNumber + "\n" + roomNumber + "\n" + name + "\n" + additionalInformation);
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
            City_TextBox.Clear();
            PostCode_TextBox1.Clear();
            PostCode_TextBox2.Clear();
            Street_TextBox.Clear();
            BuildingNumber_TextBox.Clear();
            ApartmentNumber_TextBox.Clear();
            RoomNumber_TextBox.Clear();
            Name_TextBox.Clear();
            AdditionalInformation_TextBox.Clear();
        }
    }
}
