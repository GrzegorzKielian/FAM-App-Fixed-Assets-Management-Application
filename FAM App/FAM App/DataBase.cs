﻿using System;
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

        public DataTable DataBaseSelect(DataTable dataTable)
        {
            SqlCommand cmd = DataBaseConnection();
            String data = "SELECT * FROM information_schema.tables;";
            cmd.CommandText = data;

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            dataTable = new DataTable("emp");
            sda.Fill(dataTable);
            return dataTable;
        }

    }
}
