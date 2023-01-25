using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
using System.Security.Cryptography;

namespace FAM_App.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy AddEmployeePage.xaml
    /// </summary>
    public partial class AddEmployeePage : Page
    {
        public AddEmployeePage()
        {
            InitializeComponent();
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Czy na pewno chcesz dodać nowego pracownika do bazy danych?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    string? saltString = "";
                    string? hashPasswdString = "";

                    string name = Name_TextBox.Text;
                    string surname = Surname_TextBox.Text;
                    string pesel = Pesel_TextBox.Text;
                    string phone = Phone_TextBox.Text;
                    string city = City_TextBox.Text;
                    string postCode = PostCode_TextBox1.Text + "-" + PostCode_TextBox2.Text;
                    string street = Street_TextBox.Text;
                    string buildingNumber = BuildingNumber_TextBox.Text;
                    string apartmentNumber = ApartmentNumber_TextBox.Text;
                    string email = Email_TextBox.Text;
                    string login = NewLogin_TextBox.Text;
                    SqlBoolean employee = (SqlBoolean)AsEmployee.IsChecked;
                    SqlBoolean admin = (SqlBoolean)IsAdmin.IsChecked;
                    if (employee)
                    {
                        byte[] salt = MakeSalt();
                        byte[] hashPasswd = MakeHash(NewPasswd_TextBox.Text, salt);
                        saltString = Convert.ToBase64String(salt);
                        hashPasswdString = Convert.ToBase64String(hashPasswd);
                    }

                    DataBase dataBase = new DataBase();
                    bool check = dataBase.AddEmployeeToBase(name, surname, pesel, phone, email, city, postCode, street, buildingNumber, apartmentNumber, admin, employee, login, hashPasswdString, saltString);
                    if (check)
                    {
                        MessageBox.Show("Dodano do bazy: \n"+name+"\n" + surname + "\n" + pesel + "\n" + phone + "\n" + email + "\n" + city + "\n" + postCode + "\n" + street + "\n" + buildingNumber + "\n" + apartmentNumber + "\n" + admin + "\n" + employee + "\n" + login + "\n Długość hasła: "+NewPasswd_TextBox.Text.Length);
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

        private byte[] MakeSalt()
        {
            byte[] bytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider()) rng.GetBytes(bytes);
            return bytes;
        }

        private byte[] MakeHash(string password, byte[] salt)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password,salt,100)) return deriveBytes.GetBytes(100) ;
        }

        private void ClearTextBoxes()
        {
            Name_TextBox.Clear();
            Surname_TextBox.Clear();
            Pesel_TextBox.Clear();
            Phone_TextBox.Clear();
            City_TextBox.Clear();
            PostCode_TextBox1.Clear();
            PostCode_TextBox2.Clear();
            Street_TextBox.Clear();
            BuildingNumber_TextBox.Clear();
            ApartmentNumber_TextBox.Clear();
            Email_TextBox.Clear();
            NewLogin_TextBox.Clear();
            NewPasswd_TextBox.Clear();
            IsAdmin.IsChecked = false;
        }

        private void AsEmployee_Checked(object sender, RoutedEventArgs e)
        {
            NewLogin_TextBox.IsEnabled= true;
            NewPasswd_TextBox.IsEnabled = true;
            IsAdmin.IsEnabled = true;
        }

        private void AsEmployee_Unchecked(object sender, RoutedEventArgs e)
        {
            NewLogin_TextBox.IsEnabled = false;
            NewPasswd_TextBox.IsEnabled = false;
            IsAdmin.IsEnabled = false;
        }
    }
}
