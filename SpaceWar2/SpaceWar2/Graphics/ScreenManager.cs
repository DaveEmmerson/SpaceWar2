using System.Collections.Generic;
using DEMW.SpaceWar2.GameObjects;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Graphics
{
    internal class ScreenManager
    {
        public const float DefaultMinX = -400;
        public const float DefaultMaxX = 400;
        public const float DefaultMinY = -240;
        public const float DefaultMaxY = 240;

        public float MinX { get; set; }
        public float MaxX { get; set; }
        public float MinY { get; set; }
        public float MaxY { get; set; }
        public float ScreenWidth { get { return MaxX - MinX; } }
        public float ScreenHeight { get { return MaxY - MinY; } }

        private readonly IList<IGameObject> _managedObjects;

        internal ScreenManager() 
        {
            MinX = DefaultMinX;
            MaxX = DefaultMaxX;
            MinY = DefaultMinY;
            MaxY = DefaultMaxY;

            _managedObjects = new List<IGameObject>();
        }

        public void Register(IGameObject managedObject)
        {
            _managedObjects.Add(managedObject);
        }

        public void UnRegister(IGameObject managedObject)
        {
            _managedObjects.Remove(managedObject);
        }

        public void Update()
        {
            foreach (var managedObject in _managedObjects)
            {
                Constrain(managedObject);
            }
        }

        private void Constrain(IGameObject gameObject) 
        {
            Vector2 position = gameObject.Position;

            if (position.X < MinX) { position.X  = MaxX - (MinX - position.X) % ScreenWidth; }
            if (position.X > MaxX) { position.X  = MinX + (position.X - MaxX) % ScreenWidth; }
            if (position.Y < MinY) { position.Y = MaxY - (MinY - position.Y) % ScreenHeight; }
            if (position.Y > MaxY) { position.Y = MinY + (position.Y - MaxY) % ScreenHeight; }

            gameObject.Teleport(position);
        }
    }
}
