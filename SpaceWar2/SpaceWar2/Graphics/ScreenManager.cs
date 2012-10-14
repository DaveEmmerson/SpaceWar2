using System.Collections.Generic;
using DEMW.SpaceWar2.GameObjects;
using Microsoft.Xna.Framework;
using DEMW.SpaceWar2.Physics;

namespace DEMW.SpaceWar2.Graphics
{
    internal class ScreenManager
    {
        private readonly IList<IGameObject> _managedObjects;
        public Universe Universe { get; set; }
        
        internal ScreenManager(Universe universe = null) 
        {
            Universe = universe ?? Universe.GetDefault();
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
            var position = gameObject.Position;

            if (position.X < Universe.MinX) { position.X  = Universe.MaxX - (Universe.MinX - position.X) % Universe.Width; }
            if (position.X > Universe.MaxX) { position.X  = Universe.MinX + (position.X - Universe.MaxX) % Universe.Width; }
            if (position.Y < Universe.MinY) { position.Y = Universe.MaxY - (Universe.MinY - position.Y) % Universe.Height; }
            if (position.Y > Universe.MaxY) { position.Y = Universe.MinY + (position.Y - Universe.MaxY) % Universe.Height; }

            if (position != gameObject.Position)
            {
                gameObject.Teleport(position);
            }
        }
    }
}
