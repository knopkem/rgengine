using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace rgEngine.Systems
{
    /// <summary>
    /// Contains all Systems and calls update + draw on them
    /// </summary>
    static class SystemManager
    {
        private static readonly List<ISystem> Systems = new List<ISystem>();

        public static void RegisterSystem (ISystem system)
        {
            Systems.Add(system);    
        }

        public static void Update(GameTime gameTime)
        {
            foreach (var system in Systems)
            {
                if (system is IUpdateable)
                    (system as IUpdateable).Update(gameTime);
            }
        }

        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var system in Systems)
            {
                spriteBatch.Begin();
            
                if (system is IDrawable)
                    (system as IDrawable).Draw(gameTime, spriteBatch);

                spriteBatch.End();

            }
        }
    }
}
