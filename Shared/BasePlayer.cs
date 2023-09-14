using System;
using System.Numerics;
using Shared.Packets;

namespace Shared.Demo
{
    public abstract class BasePlayer
    {
        public string Name { get; protected set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float Speed { get; protected set; } = 1f;
        public byte Id { get; protected set; }

        public virtual void ApplyInput(PlayerInputPacket inputPacket, float delta)
        {
            var velocity = Vector2.Zero;
            
            if ((inputPacket.Keys & MovementKeys.Up) != 0)
                velocity.Y = -1f;
            if ((inputPacket.Keys & MovementKeys.Down) != 0)
                velocity.Y = 1f;
            
            if ((inputPacket.Keys & MovementKeys.Left) != 0)
                velocity.X = -1f;
            if ((inputPacket.Keys & MovementKeys.Right) != 0)
                velocity.X = 1f;     
            
            PositionX += velocity.X * Speed;
            PositionY += velocity.Y * Speed;
            
            if (inputPacket.Keys != 0)
                Console.WriteLine($"Applying input for player {Id} x {PositionX} | y {PositionY}");
            
            // _rotation = command.Rotation;

            // if ((command.Keys & MovementKeys.Fire) != 0)
            // {
            //     if (_shootTimer.IsTimeElapsed)
            //     {
            //         _shootTimer.Reset();
            //         Shoot();
            //     }
            // }
        }

        public virtual void Update()
        {
            
        }
    }
}