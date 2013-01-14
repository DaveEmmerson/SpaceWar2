using System.Collections.Generic;
using DEMW.SpaceWar2.Core.GameObjects;
using DEMW.SpaceWar2.Core.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2.Core.Graphics
{
    internal class DrawingManager : IDrawingManager
    {
        private readonly List<IGameObject> _drawableObjects;

        public Camera ActiveCamera { get; private set; }

        public int ObjectCount
        {
            get { return _drawableObjects.Count; }
        }

        internal DrawingManager(IUniverse universe) 
        {
            ResetCamera(universe);
            _drawableObjects = new List<IGameObject>();
        }

        public void Register(IGameObject gameObject) 
        {
            _drawableObjects.Add(gameObject);
        }

        public void Unregister(IGameObject gameObject) 
        {
            _drawableObjects.Remove(gameObject);
        }

        public void DrawGameObjects()
        {
            _drawableObjects.ForEach(Draw);
        }

        private void Draw(IGameObject gameObject)
        {
            var model = gameObject.Model;
            var transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (var modelMesh in model.Meshes)
            {
                foreach (BasicEffect effect in modelMesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
                    effect.DiffuseColor = gameObject.Color.ToVector3();
                    
                    effect.World = transforms[modelMesh.ParentBone.Index] *
                                   Matrix.CreateRotationZ(gameObject.Rotation) *
                                   Matrix.CreateTranslation(new Vector3(gameObject.Position,0));

                    effect.View = ActiveCamera.View;
                    effect.Projection = ActiveCamera.Projection;
                }

                modelMesh.Draw();
            }
        }

        public void ResetCamera(IUniverse universe)
        {
            if (universe == null) return;

            var volumeCopy = universe.Volume.Clone();
            ActiveCamera = new Camera(volumeCopy);
        }
    }
}
