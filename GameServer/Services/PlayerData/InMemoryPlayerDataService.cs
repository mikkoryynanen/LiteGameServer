using System.Collections.Generic;
using Shared.Models;

namespace GameSever.Services.PlayerData
{
    public class InMemoryPlayerDataService : IPlayerDataService
    {
        private List<Item> _items = new List<Item>();
        
        
        public Item[] GetItems(int playerId)
        {
            return _items.ToArray();
        }
    }
}