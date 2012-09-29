using Microsoft.Xna.Framework;

namespace SpaceWar2
{
    public class GameObjectFactory : Game
    {
        private readonly GraphicsDeviceManager _graphics;

        public GameObjectFactory(GraphicsDeviceManager graphics)
        {
            _graphics = graphics;
        }

        internal Sun CreateSun(Vector2 position, Color color, float mass)
        {
            return new Sun(_graphics, position, 25, color, 32, mass);
        }

        internal Ship CreateShip(string name, Vector2 position, Vector2 velocity, Color color, IShipController controller)
        {
            var ship = new Ship(name, _graphics, position, 16, color, 16)
            {
                Velocity = velocity,
                Controller = controller,
                DrawArrows = true
            };

            return ship;
        }
    }
}
