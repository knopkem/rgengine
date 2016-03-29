using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace rgEngine.Systems
{
    interface IDrawable
    {
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
