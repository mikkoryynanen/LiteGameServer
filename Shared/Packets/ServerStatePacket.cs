using LiteNetLib.Utils;

namespace Shared.Packets;

public struct PlayerPosition : INetSerializable
{
    public byte PlayerId { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
        
    public void Serialize(NetDataWriter writer)
    {
        writer.Put(PlayerId);
        writer.Put(X);
        writer.Put(Y);
    }

    void INetSerializable.Deserialize(NetDataReader reader)
    {
        PlayerId = reader.GetByte();
        X = reader.GetFloat();
        Y = reader.GetFloat();
    }
}

public class ServerStatePacket
{
    public PlayerPosition[] PlayerPositions { get; set; }
    // TODO This should be another data type?
    // public long TimeStamp { get; set; }
}