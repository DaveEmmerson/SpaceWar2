using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.GameObjects
{ 
    public class Sun : GameObject
    {      
        public Sun(Vector2 position, float radius, Color color, float mass)
            : base (position, radius, mass)
        {
            Color = color;
        }

        protected override void UpdateInternal(GameTime gameTime)
        {
            //Todo do anything that the sun might do. 
            //e.g. spin, emit solar storms, grow, explode
        }

        public override void Draw()
        {
            // TODO: if Matt implements flares or storms, draw them here!
        }
    }
}
