using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;

public class MySqlHelper
{
    private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

    public DataTable GetUsers()
    {
        DataTable dataTable = new DataTable();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM Users";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            adapter.Fill(dataTable);
        }
        return dataTable;
    }
}
