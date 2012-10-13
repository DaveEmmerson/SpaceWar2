﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEMW.SpaceWar2.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2.Graphics
{
    internal class DrawingManager
    {
        private List<IGameObject> drawableObjects;

        public Camera ActiveCamera { get; set; }

        internal DrawingManager() {

            drawableObjects = new List<IGameObject>();

        }

        internal void Register(IGameObject gameObject) {

            drawableObjects.Add(gameObject);

        }

        internal void UnRegister(IGameObject gameObject) {

            drawableObjects.Remove(gameObject);

        }


        internal void DrawGameObjects()
        {

            if (ActiveCamera == null)
            {

                throw new NullReferenceException("No ActiveCamera set on DrawingManager.");

            }

            drawableObjects.ForEach(draw);

        }

        private void draw(IGameObject gameObject)
        {
            var model = gameObject.Model;
            var transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh modelMesh in model.Meshes)
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
    }
}
