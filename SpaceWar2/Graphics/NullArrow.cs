using DEMW.SpaceWar2.Utils.XnaWrappers;
using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2.Graphics
{
    public class NullArrow : IArrow
    {
        public void Draw(IGraphicsDevice graphicsDevice) { }
        
        public VertexPositionColor[] Verticies
        {
            get { return new VertexPositionColor[0]; }
        }
    }
}
