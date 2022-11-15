using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAM_App
{
    internal class DataBase
    {
        private SqlCommand DataBaseConnection()
        {
            SqlConnection sqlConnection = new SqlConnection(@"Server=(local);Database=Inzynierka;Trusted_Connection=Yes;"); //DESKTOP-JLN71CE
            sqlConnection.Open();
            SqlCommand cmd = sqlConnection.CreateCommand();
            return cmd;
        }

        public void AddFixedAssetsToBase()
        {
            //SqlCommand cmd = DataBaseConnection();
        }

        public DataTable DataBaseShowFixedAssets(DataTable dataTable)
        {
            SqlCommand cmd = DataBaseConnection();
            String data = "SELECT * FROM Srodki_Trwale;";
            cmd.CommandText = data;

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dataTable = new DataTable("emp");
            sda.Fill(dataTable);
            return dataTable;
        }

        public void AddProductToBase(String Name, String Brand, String Model, String Descritpion, String Year)
        {
            //SqlCommand cmd = DataBaseConnection();
        }

        public DataTable DataBaseShowProducts(DataTable dataTable)
        {
            SqlCommand cmd = DataBaseConnection();
            String data = "SELECT * FROM Produkty;";
            cmd.CommandText = data;

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dataTable = new DataTable("emp");
            sda.Fill(dataTable);
            return dataTable;
        }

        public void AddSupplierToBase()
        {
            //SqlCommand cmd = DataBaseConnection();
        }

        public DataTable DataBaseShowSuppliers(DataTable dataTable)
        {
            SqlCommand cmd = DataBaseConnection();
            String data = "SELECT * FROM Dostawcy;";
            cmd.CommandText = data;

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dataTable = new DataTable("emp");
            sda.Fill(dataTable);
            return dataTable;
        }
    }
}
