using System.Collections.Generic;

namespace Shared.Demo
{
    public abstract class BasePlayerManager
    {
        public Dictionary<byte, BasePlayer> Players = new Dictionary<byte, BasePlayer>();
        
        public abstract void LogicUpdate();
    }
}