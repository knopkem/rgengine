using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rgEngine.Components;

namespace rgEngine.Systems
{
    /// <summary>
    /// Render all entities that have a sprite component
    /// </summary>
    class RenderSystem : BaseSystem, IDrawable
    {
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var entity in Entities)
            {
                // render all sprites
                var sprite = entity.GetComponent(ComponentType.Sprite) as SpriteComponent;
                var pos = entity.GetComponent(ComponentType.Position) as PositionComponent;
                var spriteComp = entity.GetComponent(ComponentType.SpriteComposition) as SpriteCompositionComponent;

                // try to render a single sprite                
                if (pos != null)
                {
                    RenderSprite(sprite, pos.Position, spriteBatch);

                    // try rendering sprite composition
                    if ( spriteComp != null )
                    {
                        List<SpriteComponent> spriteList = spriteComp.SpriteList;
                        foreach (SpriteComponent mySprite in spriteList)
                        {
                            mySprite.Rotation = pos.Rotation;
                            RenderSprite(mySprite, pos.Position + spriteComp.RelativePosition, spriteBatch);                        
                        }
                    }
                }
            }
        }

        protected void RenderSprite(SpriteComponent sprite, Vector2 position, SpriteBatch spriteBatch)
        {
            if ( IsComponentNull(sprite) )
                return;

            position = position + sprite.RelativePosition;

            var target = new Rectangle(0, 0, sprite.SpriteSize.X, sprite.SpriteSize.Y);
            if (sprite.Visible)
            {
                //spriteBatch.Draw(sprite.SpriteTexture, position, target, sprite.SpriteColor);
                spriteBatch.Draw(sprite.SpriteTexture, position + sprite.Origin, target, sprite.SpriteColor, sprite.Rotation,
                                    sprite.Origin, 1.0f, SpriteEffects.None, 0f);
            }
        }
    }
}
