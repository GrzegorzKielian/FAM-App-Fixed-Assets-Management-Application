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
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Xml.Linq;

namespace FAM_App
{
    internal class DataBase
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
            String data = "SELECT Login FROM Pracownik WHERE Login='"+login+"';";
            cmd.CommandText = data;
            if(cmd != null)
            {
                return true;
            }
            else { return false; }
        }

        public bool AddFixedAssetsToBase(SqlDateTime introduction_date, string fixedAsset_Code, int supplier, int product, int adress, string status, int depreciation, SqlDateTime date_of_aquisition, decimal gros_orig_value, decimal net_orig_value, string descritpion, string invoice, int guarantee, int GroupID, int SubgroupID, int TypeID)
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
                "VALUES (" + ID + ",'" + fixedAsset_Code + "', '"+ inventoryNumber + "','"+status+"', " + TypeID + ", " + product + ", '" + descritpion + "', '" + date_of_aquisition + "', '', '" + introduction_date + "', " + net_orig_value + ", " + gros_orig_value + ", " + supplier + ", '" + invoice + "', " + guarantee + ", " + depreciation + ");";
            cmd.CommandText = data;
            int result2 = cmd.ExecuteNonQuery();
            //AddToHistoryAsset(introduction_date, ,adress, ID, ,"Dodanie środka trwałego do bazy danych");
            sqlConnection.Close();

            // Check Error
            if (result2 < 0) { return false;  }
            else { return true; };
        }

        public bool AddOtherFixedAssetsToBase(SqlDateTime introduction_date, string fixedAsset_Code, int supplier, int product, int adress, string status, SqlDateTime date_of_aquisition, decimal gros_orig_value, decimal net_orig_value, string descritpion, string invoice, int guarantee, int GroupID, int SubgroupID, int TypeID)
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
                "VALUES (" + ID + ",'" + fixedAsset_Code + "', '" + inventoryNumber + "','" + status + "', " + TypeID + ", " + product + ", '" + descritpion + "', '" + date_of_aquisition + "', '', '" + introduction_date + "', " + net_orig_value + ", " + gros_orig_value + ", " + supplier + ", '" + invoice + "', " + guarantee + ", '');";
            cmd.CommandText = data;
            int result2 = cmd.ExecuteNonQuery();

            sqlConnection.Close();

            // Check Error
            if (result2 < 0) { return false; }
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
                "WHERE Nr_Inwentarzowy ='" + inventoryNumber+"';";
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
                "VALUES (" + ID + ", '" + name + "', '" + surname + "', '" + pesel + "', '" + phone + "', '" + email + "', '" + city + "', '" + postCode + "', '" + street + "', '" + buildingNumber + "', '" + apartmentNumber + "', " + admin + ", " + employee + ", '" + newLogin + "', '"+ newPassword +"', '"+ salt +"');";
            cmd.CommandText = data;
            int result2 = cmd.ExecuteNonQuery();

            sqlConnection.Close();

            // Check Error
            if (result2 < 0) { return false; }
            else { return true; };
        }

        public DataTable ShowEmployee(DataTable dataTable)
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

        private void AddToHistoryAsset(SqlDateTime introduction_date, int userID, int adressID, int fixedAssetID, int introducerID, string comments)
        {
            int ID;
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

            String dataInsert = "INSERT INTO dbo.Historia_Srodka (ID_Historii, Data, id_uzytkownika, id_adresu, id_srodka, id_wprowadzajacego, Uwagi)  VALUES ( " + ID + ",'" + introduction_date + "', " + userID + ", " + adressID + ", " + fixedAssetID + "," + introducerID + ",'" + comments + "');";
            cmd.CommandText = dataInsert;
            sqlConnection.Close();

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
    }
}
