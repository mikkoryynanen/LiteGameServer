using Shared.Models;

namespace Shared.Packets.PlayerData
{
    public class PlayerInventoryResponse
    {
        public Item[] Items { get; set; }
    }
}