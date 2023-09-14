using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using Code.Shared;
using LiteNetLib;
using Shared.Demo;
using Shared.Packets;
using Shared.Packets.Connection;

namespace GameSever
{
    public class ServerPlayerManager : BasePlayerManager
    {
        public ServerPlayerManager()
        {
        }
        
        public override void LogicUpdate()
        {
            foreach (var player in Players.Values)
            {
                var serverPlayer = (ServerPlayer)player;
                serverPlayer.Update();
            }
        }

        public void ApplyInput(byte id, PlayerInputPacket inputPacket, float delta)
        {
            if (Players.TryGetValue(id, out var player))
            {
                player.ApplyInput(inputPacket, delta);
            }
            else
            {
                Console.WriteLine($"Failed to apply input to player {id}");
            }
        }

        public ServerStatePacket GetState()
        {
            var statePlayerPositions = new List<PlayerPosition>();
            foreach (var player in Players.Values)
            {
                statePlayerPositions.Add(new PlayerPosition
                {
                    PlayerId = player.Id,
                    X = player.PositionX,
                    Y = player.PositionY
                });
            }

            return new ServerStatePacket
            {
                PlayerPositions = statePlayerPositions.ToArray()
            };
        }
        
        public BasePlayer AddPlayer(ClientConnectionPacket packet, NetPeer peer)
        {
            if (!Players.TryAdd((byte)peer.Id, new ServerPlayer(
                    packet.Name,
                    5,
                    new Vector2(packet.InitialPositionX, packet.InitialPositionY),
                    peer)))
            {
                Console.WriteLine($"Failed to add player with peer {peer.Id}");
                return null;
            }

            // Console.WriteLine($"Added new player with id {(byte)peer.Id}");
            return Players[(byte)peer.Id];
        }

        public void RemovePlayer(byte id)
        {
            if (!Players.Remove(id))
                Console.WriteLine($"Failed to remove player with id {id}");
            else
                Console.WriteLine($"Removed player with id {id}");
        }

        public BasePlayer GetPlayer(byte id)
        {
            return Players.TryGetValue(id, out var player) ? player : null;
        }
    }
}