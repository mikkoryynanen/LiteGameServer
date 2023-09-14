using System;
using LiteNetLib.Utils;

namespace Shared.Packets
{
    [Flags]
    public enum MovementKeys : byte
    {
        None = 0,
        Left = 1 << 1,
        Right = 1 << 2,
        Up = 1 << 3,
        Down = 1 << 4,
        // Fire = 1 << 5
    }
    
    public class PlayerInputPacket : INetSerializable
    {
        public byte Id;
        public MovementKeys Keys;

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(Id);
            writer.Put((byte)Keys);
        }

        public void Deserialize(NetDataReader reader)
        {
            Id = reader.GetByte();
            Keys = (MovementKeys)reader.GetByte();
        }
    }
}