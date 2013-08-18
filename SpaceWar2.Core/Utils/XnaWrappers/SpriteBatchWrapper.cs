using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DEMW.SpaceWar2.Core.Utils.XnaWrappers
{
    internal class SpriteBatchWrapper : ISpriteBatch, IDisposable
    {
        private SpriteBatch _spriteBatch;

        internal SpriteBatchWrapper(IGraphicsDevice graphicsDevice)
        {
            _spriteBatch = new SpriteBatch(graphicsDevice.GraphicsDevice);
        }

        public void BeginBatch()
        {
            _spriteBatch.Begin();
        }

        public void DrawString(ISpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            if (spriteFont != null)
            {
                _spriteBatch.DrawString(spriteFont.SpriteFont, text, position, color);
            }
        }

        public void EndBatch()
        {
            _spriteBatch.End();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_spriteBatch == null) return;

            _spriteBatch.Dispose();
            _spriteBatch = null;
        }
    }
}