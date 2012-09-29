using Microsoft.Xna.Framework;

namespace SpaceWar2
{
    public class GameObjectFactory : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly GravitySimulator _gravitySimulator;

        public GameObjectFactory(GraphicsDeviceManager graphics, GravitySimulator gravitySimulator)
        {
            _graphics = graphics;
            _gravitySimulator = gravitySimulator;
        }

        internal Sun CreateSun(Vector2 position, Color color, float mass)
        {
            var sun = new Sun(_graphics, position, 10, color, 32, mass);
            _gravitySimulator.RegisterSource(sun);
            return sun;
        }

        internal Ship CreateShip(string name, Vector2 position, Vector2 velocity, Color color, IShipController controller)
        {
            var ship = new Ship(name, _graphics, position, 16, color, 16)
            {
                Velocity = velocity,
                Controller = controller,
                DrawArrows = true
            };

            _gravitySimulator.RegisterParticipant(ship);

            return ship;
        }
    }
}
