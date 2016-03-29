using Microsoft.Xna.Framework;
using rgEngine.Components;

namespace rgEngine.Systems
{
    /// <summary>
    /// Moves all entities that have movement components
    /// </summary>
    class MovementSystem : BaseSystem, IUpdateable
    {

        public void Update(GameTime gameTime)
        {
            // cycle all entities and move all enemies
            foreach (var entity in Entities)
            {
                // get the positional component and move it randomly
                var pos = entity.GetComponent(ComponentType.Position) as PositionComponent;
                var mov = entity.GetComponent(ComponentType.Movement) as MovementComponent;
                
                // safety check
                if (IsComponentNull(pos) || IsComponentNull(mov))
                    continue;

                // move based on direction move-speed and time
                float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                pos.Position = pos.Position + (mov.Direction * mov.MoveSpeed * elapsedTime);

            }
        }

    }
}
