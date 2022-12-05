using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace FAM_App
{
    internal class DataBase
    {
        private SqlCommand DataBaseConnection()
        {
            SqlConnection sqlConnection = new SqlConnection(@"Server=(local);Database=FAMDataBase;Trusted_Connection=Yes;"); //DESKTOP-JLN71CE
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

        public void AddFixedAssetsToBase()
        {
            //SqlCommand cmd = DataBaseConnection();
        }

        public void AddOtherFixedAssetsToBase()
        {
            //SqlCommand cmd = DataBaseConnection();
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
            return dataTable;
        }

        public bool AddProductToBase(String Name, String Brand, String Model, String Descritpion, String Year)
        {
            SqlCommand cmd = DataBaseConnection();
            String dataSelect = "SELECT MAX(ID_Produktu) FROM dbo.Produkt;";
            cmd.CommandText = dataSelect;
            int ID = (int)cmd.ExecuteScalar();
            ID++;
            String dataInsert = "INSERT INTO dbo.Produkt (ID_Produktu, Nazwa, Marka, Model, Opis, Rok_Produkcji)  VALUES ( "+ID+",'"+Name+"', '"+Brand+"', '"+Model+"', '"+Descritpion+"', '"+Year+"');";
            cmd.CommandText = dataInsert;
            int result = cmd.ExecuteNonQuery();

            // Check Error
            if (result < 0) { return false; }
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
            return dataTable;
        }

        public bool AddSupplierToBase(String Name, String City, String PostCode, String Street)
        {
            SqlCommand cmd = DataBaseConnection();
            String dataSelect = "SELECT MAX(ID_Dostawcy) FROM dbo.Dostawca;";
            cmd.CommandText = dataSelect;
            int ID = (int)cmd.ExecuteScalar();
            ID++;
            String dataInsert = "INSERT INTO dbo.Dostawca (ID_Dostawcy, Nazwa, Miejscowosc, Kod_Pocztowy, Ulica)  VALUES ( " + ID + ",'" + Name + "', '" + City + "', '" + PostCode + "', '" + Street + "');";
            cmd.CommandText = dataInsert;
            int result = cmd.ExecuteNonQuery();

            // Check Error
            if (result < 0) { return false; }
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
            return dataTable;
        }

        public DataTable DataBaseFixedAssetHistory(DataTable dataTable, string inventoryNumber)
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
            return dataTable;
        }
    }
}
