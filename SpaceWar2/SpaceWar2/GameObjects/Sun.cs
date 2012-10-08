using DEMW.SpaceWar2.Graphics;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.GameObjects
{ 
    class Sun : GameObject
    {
        Circle Model { get; set; }
        
        public Sun(GraphicsDeviceManager graphics, Vector2 position, float radius, Color lineColor, uint lineCount, float mass)
            : base (position, radius, mass)
        {
            Model = new Circle(graphics, radius, lineColor, lineCount);
        }

        public override void Draw()
        {
            Model.Draw();
        }
    }
}
