using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace rgEngine.Entities
{
    internal interface IRenderable
    {
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}