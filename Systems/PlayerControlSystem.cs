using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using rgEngine.Components;
using rgEngine.Entities;

namespace rgEngine.Systems
{
    /// <summary>
    /// Let the user control the player
    /// </summary>
    class PlayerControlSystem : BaseSystem, IUpdateable
    {

        public void Update(GameTime gameTime)
        {
            // cycle all entities and move all enemies
            foreach (var entity in Entities)
            {
                // only move the player
                if (entity.EntityType() != EntityType.Player)
                    continue;

                // get the positional component and move it randomly
                var mov = entity.GetComponent(ComponentType.Movement) as MovementComponent;

                // safety check
                if (IsComponentNull(mov))
                    continue;


                // First, deal with keyboard input. Get only the direction keys.
                Keys[] validKeys = {Keys.Up, Keys.Down, Keys.Left, Keys.Right};
#if WINDOWS

                Keys[] pressedKeys = (from k in Keyboard.GetState().GetPressedKeys()
                                      where validKeys.Contains(k)
                                      select k).ToArray();

#else
            Keys[] pressedKeys = {Keys.Z};
            pressedKeys[0] = GamePadHelper.currentKey(PlayerIndex.One);
            if (pressedKeys[0] == Keys.Z)
                return;
#endif

                if (pressedKeys.Length == 0)
                {
                    mov.Direction = new Vector2(0, 0);
                    return;
                }
                    

                var movementDirection = new Vector2();
                switch (pressedKeys[0])
                {
                    case Keys.Up:
                        movementDirection.Y = -1;
                        break;
                    case Keys.Down:
                        movementDirection.Y = 1;
                        break;
                    case Keys.Left:
                        movementDirection.X = -1;
                        break;
                    case Keys.Right:
                        movementDirection.X = 1;
                        break;
                }
                mov.Direction = movementDirection;

            }

        }

    }
}
