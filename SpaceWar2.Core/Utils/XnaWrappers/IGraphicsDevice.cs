using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2.Core.Utils.XnaWrappers
{
    public interface IGraphicsDevice
    {
        GraphicsDevice GraphicsDevice { get; }
        void DrawUserPrimitives(PrimitiveType primitiveType, VertexPositionColor[] vertices, int vertexOffset, int primitiveCount);
    }
}