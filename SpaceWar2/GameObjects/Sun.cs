using DEMW.SpaceWar2.Utils.XnaWrappers;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.GameObjects
{ 
    internal class Sun : GameObject
    {      
        internal Sun(Vector2 position, float radius, Color color, float mass)
            : base (position, radius, mass)
        {
            Color = color;
        }

        protected override void UpdateInternal(float deltaT)
        {
            //Todo do anything that the sun might do. 
            //e.g. spin, emit solar storms, grow, explode
        }

        public override void Draw(IGraphicsDevice graphicsDevice = null)
        {
            // TODO: if Matt implements flares or storms, draw them here!
        }
    }
}
