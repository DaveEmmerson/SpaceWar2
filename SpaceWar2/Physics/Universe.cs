using System.Collections.Generic;
using DEMW.SpaceWar2.GameObjects;

namespace DEMW.SpaceWar2.Physics
{
    public class Universe : IUniverse
    {
        private readonly Volume _volume;
        private readonly IList<IGameObject> _managedObjects;

        public Universe(Volume volume)
        {
            _volume = volume;
            _managedObjects = new List<IGameObject>();
        }

        public static IUniverse CreateDefault()
        {
            var volume = new Volume(-400, 400, -240, 240, -1000, 1000);
            return new Universe(volume);
        }

        public Volume Volume { get { return _volume; } }

        public void Register(IGameObject managedObject)
        {
            _managedObjects.Add(managedObject);
        }

        public void Unregister(IGameObject managedObject)
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

            if (position.X < _volume.MinX) { position.X = _volume.MaxX - (_volume.MinX - position.X) % _volume.Width; }
            if (position.X > _volume.MaxX) { position.X = _volume.MinX + (position.X - _volume.MaxX) % _volume.Width; }
            if (position.Y < _volume.MinY) { position.Y = _volume.MaxY - (_volume.MinY - position.Y) % _volume.Height; }
            if (position.Y > _volume.MaxY) { position.Y = _volume.MinY + (position.Y - _volume.MaxY) % _volume.Height; }

            if (position != gameObject.Position)
            {
                gameObject.Teleport(position);
            }
        }
    }
}
