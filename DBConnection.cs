using System;
using System.Configuration;
using System.Data.SqlClient;

namespace LostAndFound.DL
{
    public class DBConnection
    {
        // This holds the connection string to our database
        private static string connectionString = ConfigurationManager.ConnectionStrings["LostAndFoundDB"].ConnectionString;

        // This method returns a new connection to the database
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        // Test method to check if connection works
        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    return true; // Connection successful
                }
            }
            catch (Exception ex)
            {
                // Connection failed
                Console.WriteLine("Connection Error: " + ex.Message);
                return false;
            }
        }
    }
}