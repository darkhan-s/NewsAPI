using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace NewsAPI_beta
{
    /// <summary>
    /// Class to connect and publish articles to SQL database
    /// </summary>
    public class Connector
    {
        
        public Connector()
        {

        }

        /// <summary>
        /// Method to connect and publish data to MS SQL express
        /// </summary>
        /// <param name="rows">Data received from Parser class</param>
        public void Publish(List<SqlRow> rows)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

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

        
    }
}
