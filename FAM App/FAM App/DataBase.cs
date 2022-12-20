using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml.Linq;

namespace FAM_App
{
    internal class DataBase : EmployeeINFO
    {

        SqlConnection sqlConnection;
        private SqlCommand DataBaseConnection()
        {
            sqlConnection = new SqlConnection(@"Server=(local);Database=FAMDataBase;Trusted_Connection=Yes;"); //DESKTOP-JLN71CE
            sqlConnection.Open();
            SqlCommand cmd = sqlConnection.CreateCommand();
            return cmd;
        }

        public bool Login(string login)
        {
            SqlCommand cmd = DataBaseConnection();
            String data = "SELECT CASE WHEN EXISTS (SELECT ID_Pracownika FROM dbo.Pracownik WHERE Login='" + login+ "') THEN CAST(1 AS INT) ELSE CAST(0 AS INT) END;";
            cmd.CommandText = data;
            int result = (int)cmd.ExecuteScalar();
            if(result == 1)
            {
                sqlConnection.Close();
                return true;
            }
            else { return false; }
        }

        public string[] GetPasswordData(string login, string[] strings)
        {
            SqlCommand cmd = DataBaseConnection();
            String data = "SELECT ID_Pracownika, Haslo, Sol_Hasla FROM Pracownik WHERE Login='" + login + "';";
            cmd.CommandText = data;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    strings[0] = dr["ID_Pracownika"].ToString();
                    strings[1] = dr["Haslo"].ToString();
                    strings[2] = dr["Sol_Hasla"].ToString();
                }
            }
            else { MessageBox.Show("Brak danych"); }

            return strings;
        }

        public bool AddFixedAssetsToBase(string introduction_date, string fixedAsset_Code, int supplier, int product, int adress, string status, int depreciation, string date_of_aquisition, string gros_orig_value, string net_orig_value, string descritpion, string invoice, int guarantee, int GroupID, int SubgroupID, int TypeID, int UserID)
        {
            int ID;
            SqlCommand cmd = DataBaseConnection();
            String dataSelect = "SELECT MAX(ID_Srodka) FROM dbo.Srodek_Trwaly;";
            cmd.CommandText = dataSelect;
            var result = cmd.ExecuteScalar(); 
            if (result == DBNull.Value)
            { ID = 1; }
            else
            {
                ID = (int)cmd.ExecuteScalar();
                ID++;
            }
            string inventoryNumber = CreateAnInventoryNumber(GroupID, SubgroupID, TypeID, ID);

            String data = "INSERT INTO dbo.Srodek_Trwaly (ID_Srodka, Kod_Srodka, Nr_Inwentarzowy, Stan_Status, id_nr_klasyfikacyjny, id_produktu, Opis, Data_Nabycia, Data_Likwidacji, Data_Wprowadzenia, Wartosc_Poczatkowa_Netto, Wartosc_Poczatkowa_Brutto, id_dostawcy, Faktura, Gwarancja, Stawka_Amortyzacji) " +
                "VALUES (" + ID + ",'" + fixedAsset_Code + "', '"+ inventoryNumber + "','"+status+"', " + TypeID + ", " + product + ", '" + descritpion + "', '" + date_of_aquisition + "', NULL, '" + introduction_date + "', " + net_orig_value + ", " + gros_orig_value + ", " + supplier + ", '" + invoice + "', " + guarantee + ", " + depreciation + ");";
            cmd.CommandText = data;
            int result2 = cmd.ExecuteNonQuery();
            bool result3 = AddToHistoryAsset(introduction_date, UserID, adress, ID, ID_EmployeeINFO, "Dodanie środka trwałego do bazy danych");
            sqlConnection.Close();

            // Check Error
            if (result2 < 0 & !result3) { return false; }
            else { return true; };
        }

        public bool AddOtherFixedAssetsToBase(string introduction_date, string fixedAsset_Code, int supplier, int product, int adress, string status, string date_of_aquisition, string gros_orig_value, string net_orig_value, string descritpion, string invoice, int guarantee, int GroupID, int SubgroupID, int TypeID, int UserID)
        {
            int ID;
            SqlCommand cmd = DataBaseConnection();
            String dataSelect = "SELECT MAX(ID_Srodka) FROM dbo.Srodek_Trwaly;";
            cmd.CommandText = dataSelect;
            var result = cmd.ExecuteScalar();
            if (result == DBNull.Value)
            { ID = 1; }
            else
            {
                ID = (int)cmd.ExecuteScalar();
                ID++;
            }
            string inventoryNumber = CreateAnInventoryNumber(GroupID, SubgroupID, TypeID, ID);


            String data = "INSERT INTO dbo.Srodek_Trwaly (ID_Srodka, Kod_Srodka, Nr_Inwentarzowy, Stan_Status, id_nr_klasyfikacyjny, id_produktu, Opis, Data_Nabycia, Data_Likwidacji, Data_Wprowadzenia, Wartosc_Poczatkowa_Netto, Wartosc_Poczatkowa_Brutto, id_dostawcy, Faktura, Gwarancja, Stawka_Amortyzacji) " +
                "VALUES (" + ID + ",'" + fixedAsset_Code + "', '" + inventoryNumber + "','" + status + "', " + TypeID + ", " + product + ", '" + descritpion + "', '" + date_of_aquisition + "', NULL, '" + introduction_date + "', " + net_orig_value + ", " + gros_orig_value + ", " + supplier + ", '" + invoice + "', " + guarantee + ", '');";
            cmd.CommandText = data;
            int result2 = cmd.ExecuteNonQuery();
            bool result3 = AddToHistoryAsset(introduction_date, UserID, adress, ID, ID_EmployeeINFO, "Dodanie środka trwałego do bazy danych");
            sqlConnection.Close();

            // Check Error
            if (result2 < 0 & !result3) { return false; }
            else { return true; };
        }

        public DataTable DataBaseShowFixedAssets(DataTable dataTable)
        {
            SqlCommand cmd = DataBaseConnection();
            String data = "SELECT dbo.Srodek_Trwaly.ID_Srodka, dbo.Srodek_Trwaly.Kod_Srodka, dbo.Srodek_Trwaly.Nr_Inwentarzowy, dbo.Srodek_Trwaly.Stan_Status, dbo.Grupa.Nazwa AS Grupa, dbo.Podgrupa.Nazwa AS Podgrupa, dbo.Rodzaj.Nazwa AS Rodzaj, dbo.Produkt.Nazwa AS Produkt, dbo.Srodek_Trwaly.Opis, dbo.Srodek_Trwaly.Data_Nabycia, dbo.Srodek_Trwaly.Data_Likwidacji, dbo.Srodek_Trwaly.Data_Wprowadzenia, dbo.Srodek_Trwaly.Wartosc_Poczatkowa_Netto, dbo.Srodek_Trwaly.Wartosc_Poczatkowa_Brutto, dbo.Dostawca.Nazwa AS Dostawca, dbo.Srodek_Trwaly.Faktura, dbo.Srodek_Trwaly.Gwarancja "+
                          "FROM dbo.Produkt "+
                          "INNER JOIN dbo.Srodek_Trwaly "+
                          "INNER JOIN dbo.Dostawca ON dbo.Srodek_Trwaly.id_dostawcy = dbo.Dostawca.ID_Dostawcy ON dbo.Produkt.ID_Produktu = dbo.Srodek_Trwaly.id_Produktu "+
                          "INNER JOIN dbo.Rodzaj ON dbo.Srodek_Trwaly.id_nr_klasyfikacyjny = dbo.Rodzaj.ID_Rodzaju "+
                          "INNER JOIN dbo.Podgrupa "+
                          "INNER JOIN dbo.Grupa ON dbo.Podgrupa.id_grupy = dbo.Grupa.ID_Grupy ON dbo.Rodzaj.id_podgrupy = dbo.Podgrupa.ID_Podgrupy;";

            cmd.CommandText = data;

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dataTable = new DataTable("emp");
            sda.Fill(dataTable);
            sqlConnection.Close();
            return dataTable;
        }

        public bool AddProductToBase(String Name, String Brand, String Model, String Descritpion, String Year)
        {
            int ID;
            SqlCommand cmd = DataBaseConnection();
            String dataSelect = "SELECT MAX(ID_Produktu) FROM dbo.Produkt;";
            cmd.CommandText = dataSelect;
            var result = cmd.ExecuteScalar();
            if (result == DBNull.Value)
            { ID = 1; }
            else
            {
                ID = (int)cmd.ExecuteScalar();
                ID++;
            }

            String dataInsert = "INSERT INTO dbo.Produkt (ID_Produktu, Nazwa, Marka, Model, Opis, Rok_Produkcji)  VALUES ( "+ID+",'"+Name+"', '"+Brand+"', '"+Model+"', '"+Descritpion+"', '"+Year+"');";
            cmd.CommandText = dataInsert;
            int result2 = cmd.ExecuteNonQuery();
            sqlConnection.Close();
            // Check Error
            if (result2 < 0) { return false; }
            else { return true; };
        }

        public DataTable DataBaseShowProducts(DataTable dataTable)
        {
            SqlCommand cmd = DataBaseConnection();
            String data = "SELECT ID_Produktu, Nazwa, Marka, Model, Opis, Rok_Produkcji FROM dbo.Produkt;";
            cmd.CommandText = data;

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dataTable = new DataTable("emp");
            sda.Fill(dataTable);
            sqlConnection.Close();
            return dataTable;
        }

        public bool AddSupplierToBase(String Name, String City, String PostCode, String Street)
        {
            int ID;
            SqlCommand cmd = DataBaseConnection();
            String dataSelect = "SELECT MAX(ID_Dostawcy) FROM dbo.Dostawca;";
            cmd.CommandText = dataSelect;
            var result = cmd.ExecuteScalar();
            if (result == DBNull.Value)
            { ID = 1; }
            else
            {
                ID = (int)cmd.ExecuteScalar();
                ID++;
            }

            String dataInsert = "INSERT INTO dbo.Dostawca (ID_Dostawcy, Nazwa, Miejscowosc, Kod_Pocztowy, Ulica)  VALUES ( " + ID + ",'" + Name + "', '" + City + "', '" + PostCode + "', '" + Street + "');";
            cmd.CommandText = dataInsert;
            int result2 = cmd.ExecuteNonQuery();
            sqlConnection.Close();
            // Check Error
            if (result2 < 0) { return false; }
            else { return true; };
        }

        public DataTable DataBaseShowSuppliers(DataTable dataTable)
        {
            SqlCommand cmd = DataBaseConnection();
            String data = "SELECT ID_Dostawcy, Nazwa, Miejscowosc, Kod_Pocztowy, Ulica FROM dbo.Dostawca;";
            cmd.CommandText = data;

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dataTable = new DataTable("emp");
            sda.Fill(dataTable);
            sqlConnection.Close();
            return dataTable;
        }

        public DataTable DataBaseShowFixedAssetHistory(DataTable dataTable, string inventoryNumber)
        {
            SqlCommand cmd = DataBaseConnection();
            String data = "SELECT dbo.Historia_Srodka.ID_Historii AS Lp, dbo.Srodek_Trwaly.Nr_Inwentarzowy, dbo.Srodek_Trwaly.Kod_Srodka, dbo.Srodek_Trwaly.Stan_Status, dbo.Srodek_Trwaly.Data_Nabycia, dbo.Srodek_Trwaly.Data_Likwidacji, dbo.Srodek_Trwaly.Data_Wprowadzenia, dbo.Srodek_Trwaly.Wartosc_Poczatkowa_Netto, dbo.Srodek_Trwaly.Wartosc_Poczatkowa_Brutto, dbo.Srodek_Trwaly.Faktura, dbo.Srodek_Trwaly.Gwarancja, dbo.Historia_Srodka.Data, CONCAT(dbo.Adres.Miejscowosc, dbo.Adres.Kod_Pocztowy, dbo.Adres.Ulica, dbo.Adres.Nr_Budynku, dbo.Adres.Nr_Lokalu, dbo.Adres.Nr_Pomieszczenia) AS Adres, dbo.Historia_Srodka.id_uzytkownika AS 'Dokonal zmiany', dbo.Historia_Srodka.id_wprowadzajacego AS Wprowadzil, dbo.Historia_Srodka.Uwagi " +
                "FROM dbo.Historia_Srodka " +
                "INNER JOIN dbo.Srodek_Trwaly ON dbo.Historia_Srodka.id_srodka = dbo.Srodek_Trwaly.ID_Srodka " +
                "INNER JOIN dbo.Adres ON dbo.Historia_Srodka.id_adresu = dbo.Adres.ID_Adresu " +
                "INNER JOIN dbo.Pracownik ON dbo.Historia_Srodka.id_uzytkownika = dbo.Pracownik.ID_Pracownika AND dbo.Historia_Srodka.id_wprowadzajacego = dbo.Pracownik.ID_Pracownika " +
                "WHERE Nr_Inwentarzowy ='"+inventoryNumber+"';";
            cmd.CommandText = data;

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dataTable = new DataTable("emp");
            sda.Fill(dataTable);
            sqlConnection.Close();
            return dataTable;
        }

        public DataTable DataBaseShowGroup(DataTable dataTable)
        {
            SqlCommand cmd = DataBaseConnection();
            String data = "SELECT ID_Grupy, CONCAT(Symbol,' - ',Nazwa) AS 'Grupa' FROM dbo.Grupa;";
            cmd.CommandText = data;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dataTable = new DataTable("emp");
            sda.Fill(dataTable);
            cmd.Dispose();
            sqlConnection.Close();
            return dataTable;
        }

        public DataTable DataBaseShowSubgroup(DataTable dataTable, int groupID)
        {
            SqlCommand cmd = DataBaseConnection();
            String data = "SELECT ID_Podgrupy, CONCAT(Symbol,' - ',Nazwa) AS 'Podgrupa' FROM dbo.Podgrupa WHERE id_grupy=" + groupID + ";";
            cmd.CommandText = data;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dataTable = new DataTable("emp");
            sda.Fill(dataTable);
            cmd.Dispose();
            sqlConnection.Close();
            return dataTable;
        }

        public DataTable DataBaseShowType(DataTable dataTable, int subgroupID)
        {
            SqlCommand cmd = DataBaseConnection();
            String data = "SELECT ID_Rodzaju, CONCAT(Symbol,' - ',Nazwa) AS 'Rodzaj' FROM dbo.Rodzaj WHERE id_podgrupy=" + subgroupID + ";";
            cmd.CommandText = data;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dataTable = new DataTable("emp");
            sda.Fill(dataTable);
            cmd.Dispose();
            sqlConnection.Close();
            return dataTable;
        }

        public bool AddEmployeeToBase(string name, string surname, string pesel, string phone, string email, string city, string postCode, string street, string buildingNumber, string apartmentNumber, SqlBoolean admin, SqlBoolean employee, string newLogin, string newPassword, string salt)
        {
            int ID;
            SqlCommand cmd = DataBaseConnection();
            String dataSelect = "SELECT MAX(ID_Pracownika) FROM dbo.Pracownik;";
            cmd.CommandText = dataSelect;
            var result = cmd.ExecuteScalar();
            if (result == DBNull.Value)
            { ID = 1; }
            else
            {
                ID = (int)cmd.ExecuteScalar();
                ID++;
            }

            String data = "INSERT INTO dbo.Pracownik (ID_Pracownika, Imie, Nazwisko, Pesel, Telefon, Email, Miejscowosc, Kod_Pocztowy, Ulica, Nr_Budynku, Nr_Lokalu, Admin, Ewidencja, Login, Haslo, Sol_Hasla) " +
                "VALUES (" + ID + ", '" + name + "', '" + surname + "', '" + pesel + "', '" + phone + "', '" + email + "', '" + city + "', '" + postCode + "', '" + street + "', '" + buildingNumber + "', '" + apartmentNumber + "', '" + admin + "', '" + employee + "', '" + newLogin + "', '"+ newPassword +"', '"+ salt +"');";
            cmd.CommandText = data;
            int result2 = cmd.ExecuteNonQuery();

            sqlConnection.Close();

            // Check Error
            if (result2 < 0) { return false; }
            else { return true; };
        }

        public DataTable DataBaseShowEmployee(DataTable dataTable)
        {
            SqlCommand cmd = DataBaseConnection();
            String data = "SELECT ID_Pracownika, Imie, Nazwisko, Pesel, Telefon, Email, Miejscowosc, Kod_Pocztowy, Ulica, Nr_Budynku, Nr_Lokalu, Admin, Ewidencja, Login, Haslo, Sol_Hasla FROM dbo.Pracownik";
            cmd.CommandText = data;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dataTable = new DataTable("emp");
            sda.Fill(dataTable);
            cmd.Dispose();
            sqlConnection.Close();
            return dataTable;
        }

        private string CreateAnInventoryNumber(int GroupID, int SubgroupID, int TypeID, int ID)
        {
            string inventoryNumber;
            string groupID = GroupID.ToString();
            string subgroupID = SubgroupID.ToString();
            string typeID = TypeID.ToString();
            string iD = ID.ToString();
            inventoryNumber = groupID+subgroupID+typeID+iD;
            return inventoryNumber;
        }

        private bool AddToHistoryAsset(string introduction_date, int userID, int adressID, int fixedAssetID, int introducerID, string comments)
        {
            int ID;
            int result2;
            SqlCommand cmd = DataBaseConnection();
            String dataSelect = "SELECT MAX(ID_Historii) FROM dbo.Historia_Srodka;";
            cmd.CommandText = dataSelect;
            var result = cmd.ExecuteScalar();
            if (result == DBNull.Value)
            { ID = 1; }
            else
            {
                ID = (int)cmd.ExecuteScalar();
                ID++;
            }

            if(userID == 0) 
            {
                String dataInsert1 = "INSERT INTO dbo.Historia_Srodka (ID_Historii, Data, id_uzytkownika, id_adresu, id_srodka, id_wprowadzajacego, Uwagi)  VALUES ( " + ID + ",'" + introduction_date + "', , " + adressID + ", " + fixedAssetID + "," + introducerID + ",'" + comments + "');";
                cmd.CommandText = dataInsert1;
                result2 = cmd.ExecuteNonQuery();
            }
            else
            {
                String dataInsert2 = "INSERT INTO dbo.Historia_Srodka (ID_Historii, Data, id_uzytkownika, id_adresu, id_srodka, id_wprowadzajacego, Uwagi)  VALUES ( " + ID + ",'" + introduction_date + "', " + userID + ", " + adressID + ", " + fixedAssetID + "," + introducerID + ",'" + comments + "');";
                cmd.CommandText = dataInsert2;
                result2 = cmd.ExecuteNonQuery();
            }

            sqlConnection.Close();
            // Check Error
            if (result2 < 0) { return false; }
            else { return true; };
        }

        public bool AddAdressToBase(string City, string PostCode, string Street, string BuildingNumber, string ApartmentNumber, string RoomNumber, string Name, string AdditionalInformation)
        {
            int ID;
            SqlCommand cmd = DataBaseConnection();
            String dataSelect = "SELECT MAX(ID_Adresu) FROM dbo.Adres;";
            cmd.CommandText = dataSelect;
            var result = cmd.ExecuteScalar();
            if (result == DBNull.Value)
            { ID = 1; }
            else
            {
                ID = (int)cmd.ExecuteScalar();
                ID++;
            }

            String dataInsert = "INSERT INTO dbo.Adres (ID_Adresu, Miejscowosc, Kod_Pocztowy, Ulica, Nr_Budynku, Nr_Lokalu, Nr_Pomieszczenia, Dodatkowe_Dane, Nazwa)  VALUES ( " + ID + ",'" + City + "', '" + PostCode + "', '" + Street + "', '" + BuildingNumber + "', '" + ApartmentNumber + "', '" + RoomNumber + "', '" + AdditionalInformation + "', '" + Name + "');";
            cmd.CommandText = dataInsert;
            int result2 = cmd.ExecuteNonQuery();
            sqlConnection.Close();
            // Check Error
            if (result2 < 0) { return false; }
            else { return true; };
        }

        public DataTable DataBaseShowAdresses(DataTable dataTable)
        {
            SqlCommand cmd = DataBaseConnection();
            String data = "SELECT ID_Adresu, Miejscowosc, Kod_Pocztowy, Ulica, Nr_Budynku, Nr_Lokalu, Nr_Pomieszczenia, Dodatkowe_Dane, Nazwa FROM dbo.Adres;";
            cmd.CommandText = data;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dataTable = new DataTable("emp");
            sda.Fill(dataTable);
            cmd.Dispose();
            sqlConnection.Close();
            return dataTable;
        }

        public bool IsAdmin()
        {
            bool isAdmin=false;
            SqlCommand cmd = DataBaseConnection();
            String data = "SELECT Admin FROM dbo.Pracownik WHERE ID_Pracownika=" + ID_EmployeeINFO + ";";
            cmd.CommandText = data;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    isAdmin = (bool)dr["Admin"];
                }
            }
            else { MessageBox.Show("Brak danych"); }

            if (isAdmin) { return true; }
            else { return false; }
        }

        public DataTable GetAssetDataToEdit(DataTable dataTable, int assetID)
        {
            SqlCommand cmd = DataBaseConnection();
            String data = "SELECT CONCAT(dbo.Grupa.Symbol, ' - ', dbo.Grupa.Nazwa) AS GRUPA, CONCAT(dbo.Podgrupa.Symbol, ' - ', dbo.Podgrupa.Nazwa) AS PODGRUPA, CONCAT(dbo.Rodzaj.Symbol, ' - ', dbo.Rodzaj.Nazwa) AS RODZAJ, " +
                "CONCAT(dbo.Produkt.Nazwa, ' ', dbo.Produkt.Marka, ' ', dbo.Produkt.Model, ' ', dbo.Produkt.Rok_Produkcji) AS PRODUKT, " +
                "CONCAT(dbo.Dostawca.Nazwa, ' ', dbo.Dostawca.Miejscowosc, ' ', dbo.Dostawca.Kod_Pocztowy, ' ', dbo.Dostawca.Ulica) AS DOSTAWCA, " +
                "CONCAT(dbo.Adres.Nazwa, ' ', dbo.Adres.Miejscowosc, ' ', dbo.Adres.Kod_Pocztowy, ' ', dbo.Adres.Ulica, ' ', dbo.Adres.Nr_Budynku, ' ', dbo.Adres.Nr_Lokalu, ' ', dbo.Adres.Nr_Pomieszczenia) AS ADRES, " +
                "CONCAT(dbo.Pracownik.Imie, ' ', dbo.Pracownik.Nazwisko, ' ', dbo.Pracownik.Email) AS PRACOWNIK, " +
                "dbo.Srodek_Trwaly.Data_Nabycia, dbo.Srodek_Trwaly.Wartosc_Poczatkowa_Brutto, dbo.Srodek_Trwaly.Wartosc_Poczatkowa_Netto, dbo.Srodek_Trwaly.Opis, dbo.Srodek_Trwaly.Faktura, dbo.Srodek_Trwaly.Gwarancja, dbo.Srodek_Trwaly.Stan_Status, dbo.Srodek_Trwaly.Stawka_Amortyzacji, dbo.Srodek_Trwaly.Kod_Srodka " +
                "FROM dbo.Pracownik INNER JOIN dbo.Adres INNER JOIN dbo.Historia_Srodka ON dbo.Adres.ID_Adresu = dbo.Historia_Srodka.id_adresu ON dbo.Pracownik.ID_Pracownika = dbo.Historia_Srodka.id_uzytkownika AND dbo.Pracownik.ID_Pracownika = dbo.Historia_Srodka.id_wprowadzajacego INNER JOIN dbo.Srodek_Trwaly ON dbo.Historia_Srodka.id_srodka = dbo.Srodek_Trwaly.ID_Srodka INNER JOIN dbo.Podgrupa INNER JOIN dbo.Grupa ON dbo.Podgrupa.id_grupy = dbo.Grupa.ID_Grupy INNER JOIN dbo.Rodzaj ON dbo.Podgrupa.ID_Podgrupy = dbo.Rodzaj.id_podgrupy ON dbo.Srodek_Trwaly.id_nr_klasyfikacyjny = dbo.Rodzaj.ID_Rodzaju INNER JOIN dbo.Produkt ON dbo.Srodek_Trwaly.id_produktu = dbo.Produkt.ID_Produktu INNER JOIN dbo.Dostawca ON dbo.Srodek_Trwaly.id_dostawcy = dbo.Dostawca.ID_Dostawcy " +
                "WHERE (dbo.Srodek_Trwaly.ID_Srodka = "+assetID+")";
            cmd.CommandText = data;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dataTable = new DataTable("emp");
            sda.Fill(dataTable);
            cmd.Dispose();
            sqlConnection.Close();
            return dataTable;
        }

        public DataTable GetEmployeeDataToEdit(DataTable dataTable, int employee_ID)
        {
            SqlCommand cmd = DataBaseConnection();
            String data = "SELECT Imie, Nazwisko, Pesel, Telefon, Email, Miejscowosc, Kod_Pocztowy, Ulica, Nr_Budynku, Nr_Lokalu, Admin, Ewidencja, Login " +
                "FROM dbo.Pracownik " +
                "WHERE (dbo.Pracownik.ID_Pracownika = "+employee_ID+");";
            cmd.CommandText = data;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dataTable = new DataTable("emp");
            sda.Fill(dataTable);
            cmd.Dispose();
            sqlConnection.Close();
            return dataTable;
        }

        public bool UpdateEmployee(int employeeID, string name, string surname, string pesel, string phone, string email, string city, string postCode, string street, string buildingNumber, string apartmentNumber, SqlBoolean admin, SqlBoolean employee, string newLogin, string newPassword, string salt)
        {
            SqlCommand cmd = DataBaseConnection();
            String data = "UPDATE dbo.Pracownik " +
                "SET dbo.Pracownik.Imie = '"+name+"', dbo.Pracownik.Nazwisko = '"+surname+"', dbo.Pracownik.Pesel = '"+pesel+"', dbo.Pracownik.Telefon = '"+phone+ "', dbo.Pracownik.Email = '"+email+"', dbo.Pracownik.Miejscowosc = '"+city+ "', dbo.Pracownik.Kod_Pocztowy = '"+postCode+"', dbo.Pracownik.Ulica = '"+street+"', dbo.Pracownik.Nr_Budynku = '"+buildingNumber+ "', dbo.Pracownik.Nr_Lokalu = '"+apartmentNumber+ "', dbo.Pracownik.Admin = "+admin+", dbo.Pracownik.Login = '"+newLogin+"', dbo.Pracownik.Haslo = '"+newPassword+ "', dbo.Pracownik.Sol_Hasla = '"+salt+"' " +
                "WHERE (dbo.Pracownik.ID_Pracownika = "+employeeID+");";
            cmd.CommandText = data;
            int updateResult = cmd.ExecuteNonQuery();
            sqlConnection.Close();

            // Check Error
            if (updateResult < 0) { return false; }
            else { return true; };
        }

        public bool AddToChanges()
        {
            SqlCommand cmd = DataBaseConnection();
            String data = "";
            cmd.CommandText = data;
            int result = cmd.ExecuteNonQuery();

            sqlConnection.Close();
            if (result < 0) { return false; }
            else { return true; };
        }

        public bool UpdateFixedAsset(int asset_ID, string revision_date, int supplier, int product, int adress, string status, int depreciation, string date_of_aquisition, string gros_orig_value, string net_orig_value, string descritpion, string invoice, int guarantee, int GroupID, int SubgroupID, int TypeID, int UserID)
        {
            SqlCommand cmd = DataBaseConnection();
            String data = "UPDATE dbo.Srodek_Trwaly " +
                "SET dbo.Srodek_Trwaly.Stan_Status = '"+status+"', dbo.Srodek_Trwaly.id_nr_klasyfikacyjny = "+TypeID+", dbo.Srodek_Trwaly.id_produktu = "+product+", dbo.Srodek_Trwaly.Opis = '"+descritpion+"', dbo.Srodek_Trwaly.Data_Nabycia = '"+date_of_aquisition+"', dbo.Srodek_Trwaly.Wartosc_Poczatkowa_Brutto = "+gros_orig_value+", dbo.Srodek_Trwaly.Wartosc_Poczatkowa_Netto = "+net_orig_value+", dbo.Srodek_Trwaly.id_dostawcy = "+supplier+", dbo.Srodek_Trwaly.Faktura = '"+invoice+"', dbo.Srodek_Trwaly.Gwarancja = "+guarantee+", dbo.Srodek_Trwaly.Stawka_Amortyzacji = "+depreciation+" " +
                "WHERE (dbo.Srodek_Trwaly.ID_Srodka = "+asset_ID+");";
            cmd.CommandText = data;
            int updateResult = cmd.ExecuteNonQuery();
            bool result = AddToHistoryAsset(revision_date, UserID, adress, asset_ID, ID_EmployeeINFO, "Zmiana danych środka trwałego");
            sqlConnection.Close();

            // Check Error
            if (updateResult < 0 & !result) { return false; }
            else { return true; };
        }
    }
}
