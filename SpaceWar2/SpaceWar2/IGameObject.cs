﻿using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2
{
    interface IGameObject
    {
        bool Expired { get; }
        Vector2 Position { get; set; }
        Vector2 Acceleration { get; set; }
        float Mass { get; set; }
        float Radius { get; set; }

        void ApplyForce(Vector2 force);
        Vector2 ResolveForces();
        
        void Draw();
    }
}
