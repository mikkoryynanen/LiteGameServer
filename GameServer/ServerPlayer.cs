using System;
using System.Numerics;
using LiteNetLib;
using Shared.Demo;

namespace GameSever
{
    public class ServerPlayer : BasePlayer
    {
        public NetPeer Peer { get; set; }
        
        public ServerPlayer(string name, float speed, Vector2 position, NetPeer peer)
        {
            Name = name;
            speed = speed;
            Id = (byte)peer.Id;
            Peer = peer;
        }

        public override void Update()
        {
            // Console.WriteLine($"Updating server player {DateTime.Now.Second}");
        }
    }
}