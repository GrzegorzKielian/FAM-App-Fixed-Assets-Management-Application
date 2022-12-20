using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FAM_App.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy EditEmployeeWindow.xaml
    /// </summary>
    public partial class EditEmployeeWindow : Window
    {
        int employee_ID;
        string oldValue;
        string revision_date;
        public EditEmployeeWindow(int employee_ID)
        {
            InitializeComponent();
            this.employee_ID = employee_ID;
            GetDataBaseData(employee_ID);
        }

        private void GetDataBaseData(int employee_ID)
        {
            DataBase dataBase = new DataBase();
            DataTable Employee = new DataTable("emp");
            Employee = dataBase.GetEmployeeDataToEdit(Employee, employee_ID);
            LoadDataToBoxes(Employee);
        }

        private void LoadDataToBoxes(DataTable employee)
        {
            Name_TextBox.Text = employee.Rows[0][0].ToString();
            Surname_TextBox.Text = employee.Rows[0][1].ToString();
            Pesel_TextBox.Text = employee.Rows[0][2].ToString();
            Phone_TextBox.Text = employee.Rows[0][3].ToString();
            Email_TextBox.Text = employee.Rows[0][4].ToString();
            City_TextBox.Text = employee.Rows[0][5].ToString();
            string[] psCode = SplitPostCode(employee.Rows[0][6].ToString());
            PostCode_TextBox1.Text = psCode[0];
            PostCode_TextBox2.Text = psCode[1];
            Street_TextBox.Text = employee.Rows[0][7].ToString();
            BuildingNumber_TextBox.Text = employee.Rows[0][8].ToString();
            ApartmentNumber_TextBox.Text = employee.Rows[0][9].ToString();
            NewLogin_TextBox.Text = employee.Rows[0][12].ToString();
            int iF = Convert.ToInt32(employee.Rows[0][10]);
            if(iF == 1)
            {
                IsAdmin.IsChecked = true;
            }
        }

        private string[] SplitPostCode(string v)
        {
            string[] s = v.Split('-', 2);
            return s;
        }

        private byte[] MakeSalt()
        {
            byte[] bytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider()) rng.GetBytes(bytes);
            return bytes;
        }

        private byte[] MakeHash(string password, byte[] salt)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, 100)) return deriveBytes.GetBytes(100);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz anulować?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void EditEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz zapisać zmiany?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
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
                SqlBoolean employee = true;
                SqlBoolean admin = (SqlBoolean)IsAdmin.IsChecked;
                byte[] salt = MakeSalt();
                byte[] hashPasswd = MakeHash(NewPasswd_TextBox.Text, salt);
                string saltString = Convert.ToBase64String(salt);
                string hashPasswdString = Convert.ToBase64String(hashPasswd);
                

                DataBase dataBase = new DataBase();
                bool check = dataBase.UpdateEmployee(employee_ID, name, surname, pesel, phone, email, city, postCode, street, buildingNumber, apartmentNumber, admin, employee, login, hashPasswdString, saltString);
                if (check)
                {
                    MessageBox.Show("Zapisano zmiany");
                }
                else { MessageBox.Show("Błąd przy wstawianiu danych do bazy!"); }
            }
        }
    }
}
