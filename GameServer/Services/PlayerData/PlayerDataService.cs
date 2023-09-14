using System.Collections.Generic;
using GameSever.Utils;
using MySqlConnector;
using Shared.Models;

namespace GameSever.Services.PlayerData
{
    public class PlayerDataService : IPlayerDataService
    {
        public Item[] GetItems(int playerId)
        {
            // Open connection
            using var connection = new MySqlConnection(MySqlConnectionString.Get());
            var items = new List<Item>();
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Items"; // TODO Currently getting all items from items db, not player owned items
            
                // Execute command and read data
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32("id");
                        var name = reader.GetString("name");
                        var description = reader.GetString("description");

                        items.Add(new Item { Id = id, Name = name, Description = description });
                        // Console.WriteLine($"{id} | {name} | {description}");
                    }
                }
            }
            
            connection.Close();

            return items.ToArray();
        }
    }
}