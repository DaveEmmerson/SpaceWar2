using DEMW.SpaceWar2.Core.Physics;
using DEMW.SpaceWar2.Core.Utils.XnaWrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2.Core.GameObjects
{
    public interface IGameObject
    {
        bool Expired { get; }

        float Mass { get; }
        Vector2 Position { get; }
        
        float Rotation { get; }

        float Radius { get; }
        
        Model Model { get; }
        Color Color { get; }

        void ApplyExternalForce(Force force);
        void ApplyInternalForce(Force force);
        
        void Teleport(Vector2 destination);

        void Draw(IGraphicsDevice graphicsDevice);
    }
}
