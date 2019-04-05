using System;
using System.Data.SqlClient;

namespace DEVPRO
{
    public static class DB
    {
        private static SqlConnection Connection;
        private static SqlCommand Command;
        private static SqlDataReader reader;

        public static void ConnectDB()
        {
            Connection =
                new SqlConnection(
                    @"Data Source=(localdb)\ProjectsV13;Initial Catalog=DB_1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            Connection.Open();
            if (Connection == null) throw new Exception("DataBase Connection Failed");
            Command = Connection.CreateCommand();
        }

        public static void Close_connection()
        {
            if (Connection != null) Connection.Close();
        }

        public static void PutData(string command)
        {
            try
            {
                ConnectDB();
                Command = new SqlCommand(command, Connection);
                Command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Close_connection();
            }
        }

        public static SqlDataReader GetData(SqlCommand command)
        {
            ConnectDB();
            command.Connection = Connection;
            try
            {
                reader = command.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return reader;
        }

        public static int update(SqlCommand command)
        {
            ConnectDB();
            command.Connection = Connection;
            var flag = command.ExecuteNonQuery();
            Close_connection();
            return flag;
        }
    }
}