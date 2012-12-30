using System.Collections.Generic;
using DEMW.SpaceWar2.GameObjects;

namespace DEMW.SpaceWar2.Physics
{
    public class Universe : IUniverse
    {
        private readonly IList<IGameObject> _managedObjects;
        
        public Universe(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
            MinZ = minZ;
            MaxZ = maxZ;

            _managedObjects = new List<IGameObject>();
        }

        public static IUniverse GetDefault()
        {
            return new Universe(-400, 400, -240, 240, -1000, 1000);
        }

        public float MinX { get; private set; }
        public float MaxX { get; private set; }
        public float MinY { get; private set; }
        public float MaxY { get; private set; }
        public float MinZ { get; private set; }
        public float MaxZ { get; private set; }

        public float Width
        {
            get { return MaxX - MinX; }
        }

        public float Height
        {
            get { return MaxY - MinY;  }
        }

        public void Expand(float verticalAmount)
        {
            var horizontalAmount = verticalAmount * Width / Height;

            MinX -= horizontalAmount;
            MaxX += horizontalAmount;
            MinY -= verticalAmount;
            MaxY += verticalAmount;
        }

        public void Contract(float verticalAmount)
        {
            Expand(-verticalAmount);
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
            var position = gameObject.Position;

            if (position.X < MinX) { position.X = MaxX - (MinX - position.X) % Width; }
            if (position.X > MaxX) { position.X = MinX + (position.X - MaxX) % Width; }
            if (position.Y < MinY) { position.Y = MaxY - (MinY - position.Y) % Height; }
            if (position.Y > MaxY) { position.Y = MinY + (position.Y - MaxY) % Height; }

            if (position != gameObject.Position)
            {
                gameObject.Teleport(position);
            }
        }
    }
}
