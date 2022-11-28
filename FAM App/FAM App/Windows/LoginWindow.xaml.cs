﻿using System;
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
                string passwd = PasswdTxtBox.ToString();

                DataBase dataBase = new DataBase();

                if(dataBase.Login(login))
                {
                    MainMenu mainMenu = new MainMenu();
                    mainMenu.Show();
                    this.Close();
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
    }
}
