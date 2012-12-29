using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2.Utils.XnaWrappers
{
    public interface IGraphicsDevice
    {
        void DrawUserPrimitives(PrimitiveType primitiveType, VertexPositionColor[] vertices, int vertexOffset, int primitiveCount);
    }
}