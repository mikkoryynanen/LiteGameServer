using System;

namespace Shared.Packets.Connection
{
    public class ClientConnectionPacket
    {
        public string Name { get; set; }
        public float InitialPositionX { get; set; }
        public float InitialPositionY { get; set; }
    }
}