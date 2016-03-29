using System.Collections.Generic;
using Microsoft.Xna.Framework;
using rgEngine.Components;
using rgEngine.Core;

namespace rgEngine.Systems
{
    /// <summary>
    /// Updates the movement component of Entities that have PathComponents based on current Position
    /// </summary>
    class WaypointingSystem : BaseSystem , IUpdateable
    {
        public void Update(GameTime gameTime)
        {
            // cycle all entities and move all enemies
            foreach (var entity in Entities)
            {

                // get the positional component and move it randomly
                var pos = entity.GetComponent(ComponentType.Position) as PositionComponent;
                var path = entity.GetComponent(ComponentType.Path) as PathComponent;
                var mov = entity.GetComponent(ComponentType.Movement) as MovementComponent;

                // safety check
                if ( IsComponentNull(pos) || IsComponentNull(path) || IsComponentNull(mov) )
                    continue;

                // check if we have a path
                if (path.Path == null)
                    continue;

                
                // check if waypoint is reached
                Vector2 wayPoint;
                if (path.Path.Count > 0)
                {
                    wayPoint = path.Path.Peek();
                    if (Algorithms.IsNearTarget(pos.Position, wayPoint, path.TargetTolerance))
                    {
                        Queue<Vector2> temp = path.Path;
                        temp.Dequeue();
                        path.Path = temp;
                    }
                    else
                    {

                        Vector2 direction = (wayPoint - pos.Position);
                        direction.Normalize();
                        mov.Direction = direction;
                        pos.Rotation = Algorithms.Vector2Radians(direction);
                    }
                }
                else
                {
                    path.Target =  pos.Position;
                    continue;
                }
                
                

            }
        }
    }
}
