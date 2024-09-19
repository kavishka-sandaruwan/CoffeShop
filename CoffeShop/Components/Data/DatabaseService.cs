using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeShop.Components.Data
{
    public class DatabaseService
    {
        /*        private readonly string _connectionString= "Server=LAPTOP-21I8K0UF;Database=coffeeShop;User Id=kavishka;Password=12345;TrustServerCertificate=True;";
        */

        public string ConnectionString { get; }
        public DatabaseService(string connectionString)
        {
            ConnectionString = connectionString;
        }
        // Asynchronously executes the given SQL query and returns the result as a DataTable
        public async Task<DataTable> GetDataAsync(string query)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
        }
    }

}

