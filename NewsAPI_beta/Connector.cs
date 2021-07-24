using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NewsAPI_beta
{
    public class Connector
    {
        //constructor for standard connection with username
        public Connector(string server, string database, string username, string password)
        {
            Server = server;
            Database = database;
            Username = username;
            Password = password;
        }

        // if windows authentication is used
        public Connector(string server, string database)
        {
            Server = server;
            Database = database;
        }

        public void Publish(List<SqlRow> rows)
        {
            string connectionString = "";

            if (Username == null && Password == null)
            {
                connectionString = string.Format("Server= {0}; Database={1}; Integrated Security=True;", Server, Database);
            }
            else
            {
                connectionString = string.Format("Server= {0}; Database={1}; User id = {2}, Password = {3};", Server, Database, Username, Password);
            }
            string databaseQuery = @"CREATE DATABASE TestDB";
            string tableQuery = @"CREATE TABLE News(Id int IDENTITY(1,1) PRIMARY KEY, Title nvarchar(256), Date DateTime, Content nvarchar(256) )";

            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(databaseQuery, cnn);
                cnn.Open();

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    Console.WriteLine("Database already exists");
                }


                command.CommandText = tableQuery;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    Console.WriteLine("Table already exists");
                }


                command.CommandText = string.Format(@"TRUNCATE TABLE News");
                command.ExecuteNonQuery();

                command.CommandText = string.Format(@"Insert INTO News (Title, Date, Content) Values ");
                int counter = 0;
                foreach (var item in rows)
                {
                    counter++;

                    //apparently adding N before insert is important for encoding, otherwise returns "???"
                    command.CommandText += string.Format(" (N'{0}', N'{1}', N'{2}') ", item.Title, item.Date, item.Content);


                    //insert maximum is 1000 values at the time
                    if (counter == 1000 || counter == rows.Count)
                    {
                        break;
                    }
                    else
                    {
                        command.CommandText += ",";
                    }
                }

                command.ExecuteNonQuery();
            }


        }


        private string Server { get; set; } = "DARKHAN\\SQLEXPRESS";
        private string Database { get; set; } = "TestDB";
        private string Username { get; set; }
        private string Password { get; set; }
    }
}
