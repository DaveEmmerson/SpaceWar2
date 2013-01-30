using System;
using System.Collections.Generic;
using DEMW.SpaceWar2.Core.Controls;
using DEMW.SpaceWar2.Core.GameObjects;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Core
{
    public interface IGameObjectFactory
    {
        IList<IGameObject> GameObjects { get; }
        IGameObject CreateSun(Vector2 position, Color color, float mass);
        IGameObject CreateShip(string name, Vector2 position, Vector2 velocity, Color color, IShipController controller);
        void DestroyAll(Predicate<IGameObject> match);
    }
}