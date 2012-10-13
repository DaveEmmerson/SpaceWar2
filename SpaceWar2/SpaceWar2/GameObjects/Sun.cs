using DEMW.SpaceWar2.Graphics;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.GameObjects
{ 
    class Sun : GameObject
    {
        public override string ModelPath { get { return "Models/Sun"; } }
        
        Circle LineModel { get; set; }
        
        public Sun(GraphicsDeviceManager graphics, Vector2 position, float radius, Color lineColor, uint lineCount, float mass)
            : base (position, radius, mass)
        {
            LineModel = new Circle(graphics, radius, lineColor, lineCount);
            Color = lineColor;
        }

        protected override void UpdateInternal(GameTime gameTime)
        {
            //Todo do anything that the sun might do. 
            //e.g. spin, emit solar storms, grow, explode
        }

        public override void Draw()
        {
            LineModel.Draw();
        }
    }
}
