using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeShop.Components.Models;

namespace CoffeShop.Components.Data
{
    public class OrderService
    {

        // hold references to the DatabaseService and InventoryService
        private readonly DatabaseService _databaseService;
        private readonly InventoryService _inventoryService;

        public OrderService(DatabaseService databaseService, InventoryService inventoryService)
        {
            _databaseService = databaseService;
            _inventoryService = inventoryService;
        }

        // submit a new coffee order
        public async Task SubmitOrderAsync(CoffeeOrder order)
        {
            // Calculate the total price for the order
            order.Price = CalculateOrderPrice(order);

            // Insert order 
            string query = "INSERT INTO Orders (CustomerName, CoffeeType, Quantity, Size, HasExtraShot, HasWhippedCream, Price) VALUES (@CustomerName, @CoffeeType, @Quantity, @Size, @HasExtraShot, @HasWhippedCream, @Price)";

            
            using (SqlConnection connection = new SqlConnection(_databaseService.ConnectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerName", order.CustomerName);
                    command.Parameters.AddWithValue("@CoffeeType", order.CoffeeType);
                    command.Parameters.AddWithValue("@Quantity", order.Quantity);
                    command.Parameters.AddWithValue("@Size", order.Size);
                    command.Parameters.AddWithValue("@HasExtraShot", order.HasExtraShot);
                    command.Parameters.AddWithValue("@HasWhippedCream", order.HasWhippedCream);
                    command.Parameters.AddWithValue("@Price", order.Price);

                    await command.ExecuteNonQueryAsync();
                }
            }
            // update the inventory base on  coffee order
            await _inventoryService.UpdateInventoryAsync(order.CoffeeType, order.Quantity);

        }

        // calculate the total price of the coffee order
        private decimal CalculateOrderPrice(CoffeeOrder order)
        {
            decimal basePrice = order.CoffeeType switch
            {
                "Cappuccino" => 2.20m,
                "Latte" => 2.30m,
                "Espresso" => 2.50m,
                "Tea" => 2.40m,
                "Black Coffee" => 3.00m,
                _ => 0m
            };

            decimal sizePrice = order.Size switch
            {
                
                "Medium" => 0.50m,
                "Large" => 1.00m,
                _ => 0m
            };

            decimal extrasPrice = (order.HasExtraShot ? 0.75m : 0) + (order.HasWhippedCream ? 0.50m : 0);

            return (basePrice + sizePrice + extrasPrice) * order.Quantity;
        }

    }
}
