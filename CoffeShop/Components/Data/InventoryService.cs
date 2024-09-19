using CoffeShop.Components.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeShop.Components.Data
{
    public class InventoryService
    {
        private readonly DatabaseService _databaseService;

        public InventoryService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        // add a new inventory item 
        public async Task AddIngredientAsync(InventoryItem inventory)
        {
            string query = "INSERT INTO InventoryItem (Name, Quantity) VALUES (@Name, @Quantity)";

            using (SqlConnection connection = new SqlConnection(_databaseService.ConnectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", inventory.Name);
                    command.Parameters.AddWithValue("@Quantity",inventory.Quantity);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        // update inventory items
        public async Task UpdateIngredientQuantityAsync(string name, int quantity)
        {
            string query = "UPDATE InventoryItem SET Quantity = Quantity + @Quantity WHERE Name = @Name";

            using (SqlConnection connection = new SqlConnection(_databaseService.ConnectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Quantity", quantity);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        //update inventory according to order
        public async Task UpdateInventoryAsync(string coffeeType, int quantity)
        {
            
            int coffeeBeansUsed = coffeeType switch
            {
                "Cappuccino" => 4,
                "Latte" => 3,
                "Espresso" => 2,
                "Tea" => 0,
                "Black Coffee" => 2,
                _ => 0
            };

            int sugarUsed = coffeeType switch
            {
                "Cappuccino" => 5,
                "Latte" => 2,
                "Espresso" => 10,
                "Tea" => 10,
                "Black Coffee" => 5,
                _ => 0
            };

           
            int totalCoffeeBeansUsed = coffeeBeansUsed * quantity;
            int totalSugarUsed = sugarUsed * quantity;

           
            string query = @"
                UPDATE InventoryItem
                SET Quantity = CASE
                    WHEN Name = 'Coffee Beans' THEN Quantity - @TotalCoffeeBeansUsed
                    WHEN Name = 'Sugar' THEN Quantity - @TotalSugarUsed
                END WHERE Name IN ('Coffee Beans', 'Sugar')";

            using (SqlConnection connection = new SqlConnection(_databaseService.ConnectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TotalCoffeeBeansUsed", totalCoffeeBeansUsed);
                    command.Parameters.AddWithValue("@TotalSugarUsed", totalSugarUsed);

                    
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
