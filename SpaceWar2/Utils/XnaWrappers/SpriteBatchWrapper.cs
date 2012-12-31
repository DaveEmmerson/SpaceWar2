using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2.Utils.XnaWrappers
{
    public class SpriteBatchWrapper : ISpriteBatch
    {
        private readonly SpriteBatch _spriteBatch;

        public SpriteBatchWrapper(IGraphicsDevice graphicsDevice)
        {
            _spriteBatch = new SpriteBatch(graphicsDevice.GraphicsDevice);
        }

        public void Begin()
        {
            _spriteBatch.Begin();
        }

        public void DrawString(ISpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            _spriteBatch.DrawString(spriteFont.SpriteFont, text, position, color);
        }

        public void End()
        {
            _spriteBatch.End();
        }
    }
}