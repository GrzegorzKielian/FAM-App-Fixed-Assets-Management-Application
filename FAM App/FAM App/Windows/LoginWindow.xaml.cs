using System;
using System.Collections.Generic;
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

namespace FAM_App
{
    /// <summary>
    /// Logika interakcji dla klasy LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            LoginTxtBox.Text = String.Empty;
            PasswdTxtBox.Clear();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = LoginTxtBox.Text;
                string passwd = PasswdTxtBox.Password.ToString();

                DataBase dataBase = new DataBase();

                if (dataBase.Login(login))
                {
                    bool checkpassword = Checkpassword(login, passwd);
                    if (checkpassword)
                    {
                        MainMenu mainMenu = new MainMenu();
                        mainMenu.Show();
                        this.Close();
                    }
                    else
                    {
                        PasswdTxtBox.BorderBrush = Brushes.Red;
                    }

                }
                else
                {
                    LoginTxtBox.BorderBrush = Brushes.Red;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("db error");
                MessageBox.Show(ex.ToString());
            }
        }

        private void LoginTxtBox_MouseEnter(object sender, MouseEventArgs e)
        {
            LoginTxtBox.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 221, 221, 221));
        }

        private void PasswdTxtBox_MouseEnter(object sender, MouseEventArgs e)
        {
            PasswdTxtBox.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 221, 221, 221));
        }

        private bool Checkpassword(string login, string passwd)
        {
            DataBase dataBase = new DataBase();
            string[] strings = new string[3];
            strings = dataBase.GetPasswordData(login, strings);
            string hashPasswdFromDataBase = strings[1];
            string saltFromDataBase = strings[2];

            byte[] salt = Convert.FromBase64String(saltFromDataBase);
            byte[] hashPasswd = MakeHash(passwd, salt);
            string hashPasswdString = Convert.ToBase64String(hashPasswd);
            string idEmployee = strings[0];

            if(hashPasswdFromDataBase == hashPasswdString)
            {
                EmployeeINFO.ID_EmployeeINFO = Convert.ToInt32(idEmployee);
                return true;
            }
            else { return false; }


        }
        private byte[] MakeHash(string password, byte[] salt)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, 100)) return deriveBytes.GetBytes(100);
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter) { LoginButton_Click(sender, e); }
        }
    }
}
