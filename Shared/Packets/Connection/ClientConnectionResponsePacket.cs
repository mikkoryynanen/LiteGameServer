using System;

namespace Shared.Packets.Connection
{
    public class ClientConnectionResponsePacket
    {
        public byte Id { get; set; }
        public float InitialPositionX { get; set; }
        public float InitialPositionY { get; set; }
    }
}