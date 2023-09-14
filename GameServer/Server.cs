using System;
using System.Collections.Generic;
using System.Threading;
using Code.Shared;
using LiteNetLib;
using LiteNetLib.Utils;
using Shared;
using Shared.Packets;
using Shared.Packets.Chat;
using Shared.Packets.Connection;

namespace GameSever
{
    public class Server
    {
        private readonly NetDataWriter _cachedWriter = new NetDataWriter();
        private PlayerInputPacket _cachedInput = new PlayerInputPacket();
        
        private readonly EventBasedNetListener _eventListener;
        private readonly NetManager _server;
        private readonly NetPacketProcessor _packetProcessor;
        private readonly NetManager _netManager;
        private readonly ServerPlayerManager _playerManager;
        private readonly LogicTimer _logicTimer;
        private bool _running = false;

        public Server()
        {
            _eventListener = new EventBasedNetListener();
            _server = new NetManager(_eventListener);
            _packetProcessor = new NetPacketProcessor();
            _playerManager = new ServerPlayerManager();
            _netManager = new NetManager(_eventListener)
            {
                AutoRecycle = true
            };

            _running = true;
            
            // TODO Wrap this in thread manager
            _logicTimer = new LogicTimer(LogicUpdate);
            var t = new Thread(() => _logicTimer.Start());
            t.Start();

            // _server.SimulateLatency = true;
            // _server.SimulatePacketLoss = true;
            // _server.SimulationPacketLossChance = 100;
            // _server.SimulationMinLatency = 3500;
            _server.Start(80);

            Console.WriteLine("server started");

            _packetProcessor.SubscribeReusable<ClientConnectionPacket, NetPeer>(OnClientConnected);
            _packetProcessor.SubscribeReusable<ChatMessage, NetPeer>(OnChatMessage);
            // _packetProcessor.SubscribeReusable<PlayerInputPacket, NetPeer>(OnClientInput);

            _packetProcessor.RegisterNestedType<PlayerPosition>();
            
            _eventListener.ConnectionRequestEvent += request =>
            {
                if (_server.ConnectedPeersCount < 10 /* max connections */)
                    request.AcceptIfKey("SomeConnectionKey");
                else
                    request.Reject();
            };

            _eventListener.PeerDisconnectedEvent += (peer, info) =>
            {
                _playerManager.RemovePlayer((byte)peer.Id);
                SendToAll(WritePacket(new ClientDisconnected()), DeliveryMethod.ReliableOrdered);
            };
            
            _eventListener.NetworkReceiveEvent += (peer, reader, channel, method) =>
            {
                byte packetType = reader.GetByte();
                
                if (packetType >= Enum.GetValues(typeof(PacketType)).Length)
                    return;
                PacketType pt = (PacketType) packetType;
                switch (pt)
                {
                    case PacketType.Movement:
                        OnInputReceived(reader, peer);
                        break;
                    case PacketType.Serialized:
                        _packetProcessor.ReadAllPackets(reader, peer);
                        break;
                    default:
                        Console.WriteLine("Unhandled packet: " + pt);
                        break;
                }
            };

            while (_running)
            {
                _server.PollEvents();
                Thread.Sleep(15);
            }
            
            _server.Stop();
            _logicTimer.Stop();
        }

        private void LogicUpdate()
        {
            _playerManager.LogicUpdate();

            if (_playerManager.Players.Count > 0)
            {
                var state = _playerManager.GetState();
                var packet = WritePacket(state);
                SendToAll(packet, DeliveryMethod.Unreliable);
            }
        }

        private void OnInputReceived(NetPacketReader reader, NetPeer peer)
        {
            // Console.WriteLine($"Received input packet");
            
            _cachedInput.Deserialize(reader);
            
            // TODO MOVE to server config
            float FramesPerSecond = 30.0f;
            float FixedDelta = 1.0f / FramesPerSecond;
            
            _playerManager.ApplyInput((byte)peer.Id, _cachedInput, FixedDelta);
        }

        private void OnClientConnected(ClientConnectionPacket packet, NetPeer peer)
        {
            var newPlayer = _playerManager.AddPlayer(packet, peer);
            
            SendToAll(WritePacket(new ClientConnectionResponsePacket
            {
                Id = newPlayer.Id
            }), DeliveryMethod.ReliableOrdered);
        }
        
        private void OnChatMessage(ChatMessage packet, NetPeer peer)
        {
            SendToAll(WritePacket(packet), DeliveryMethod.ReliableOrdered);
        }

        private void SendToAll(NetDataWriter writer, DeliveryMethod deliveryMethod)
        {
            var connectedPeers = new List<NetPeer>();
            _server.GetPeersNonAlloc(connectedPeers, ConnectionState.Connected);
            foreach (var peer in connectedPeers)
                peer.Send(writer, deliveryMethod);
        }
        
        private NetDataWriter WritePacket<T>(T packet) where T : class, new()
        {
            _cachedWriter.Reset();
            _cachedWriter.Put((byte) PacketType.Serialized);
            _packetProcessor.Write(_cachedWriter, packet);
            return _cachedWriter;
        }
    }
}