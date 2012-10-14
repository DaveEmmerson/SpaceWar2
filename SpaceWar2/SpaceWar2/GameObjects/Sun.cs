using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.GameObjects
{ 
    class Sun : GameObject
    {
        // This is all that is needed for the sun to be drawn.
        // It is registered with the DrawingManager by the factory.
        public override string ModelPath { get { return "Models/Sun"; } }
        
        public Sun(GraphicsDeviceManager graphics, Vector2 position, float radius, Color color, float mass)
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
