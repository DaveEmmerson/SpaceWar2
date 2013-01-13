using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2.Utils.XnaWrappers
{
    internal class GraphicsDeviceWrapper : IGraphicsDevice
    {
        private readonly IGraphicsDeviceManager _graphicsDeviceManager;

        internal GraphicsDeviceWrapper(IGraphicsDeviceManager graphicsDeviceManager)
        {
            _graphicsDeviceManager = graphicsDeviceManager;
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return ((GraphicsDeviceManager) _graphicsDeviceManager).GraphicsDevice; }
        }

        public void DrawUserPrimitives(PrimitiveType primitiveType, VertexPositionColor[] vertices, int vertexOffset, int primitiveCount)
        {
            GraphicsDevice.DrawUserPrimitives(primitiveType, vertices, vertexOffset, primitiveCount);
        }
    }
}