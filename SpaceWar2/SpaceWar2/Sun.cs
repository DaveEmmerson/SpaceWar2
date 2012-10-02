using Microsoft.Xna.Framework;

namespace SpaceWar2
{ 
    class Sun : IGameObject
    {
        Circle Model { get; set; }
        
        public Sun(GraphicsDeviceManager graphics, Vector2 position, float radius, Color lineColor, uint lineCount, float mass)
        {
            Mass = mass;
            Position = position;
            Radius = radius;
            
            Model = new Circle(graphics, radius, lineColor, lineCount);
        }

        public bool Expired { get; private set; }
        public Vector2 Position { get; set; }
        public Vector2 Acceleration { get; set; }
        public float Mass { get; set; }
        public float Radius { get; set; }

        public void Draw()
        {
            Model.Draw();
        }
    }
}
