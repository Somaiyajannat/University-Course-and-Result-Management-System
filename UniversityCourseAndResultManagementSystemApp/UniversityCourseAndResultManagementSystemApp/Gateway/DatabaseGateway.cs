﻿using System.Data.SqlClient;

namespace UniversityCourseAndResultManagementSystemApp.Gateway
{
    public class DatabaseGateway
    {
        private string ConnectionString = "Data Source=PANDA-PC\\SQLEXPRESS;Initial Catalog=UniversityDB;Integrated Security=True";
        public SqlConnection Connection = new SqlConnection();
        public SqlCommand Command = new SqlCommand();

        public DatabaseGateway()
        {
            Connection.ConnectionString = ConnectionString;
            Command.Connection = Connection;
        }
    }
}