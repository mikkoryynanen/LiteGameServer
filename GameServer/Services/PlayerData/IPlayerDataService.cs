using Shared.Models;

namespace GameSever.Services.PlayerData
{
    public interface IPlayerDataService
    {
        Item[] GetItems(int playerId);
    }
}