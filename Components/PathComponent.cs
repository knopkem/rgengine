using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace rgEngine.Components
{
    /// <summary>
    /// All objects that need pathfinding use this component to store the path and the current target
    /// </summary>
    class PathComponent : IComponent
    {
        public ComponentType ComponentType { get { return ComponentType.Path; } }

        public PathComponent()
        {
            TargetTolerance = 10;
        }

        /// <summary>
        /// Queue of waypoints build a path, peek (first element) is the current waypoint
        /// </summary>
        public Queue<Vector2> Path { get; set; }

        /// <summary>
        /// This is the current target, not necessary the same as in the path
        /// </summary>
        public Vector2 Target { get; set; }

        /// <summary>
        /// Tolerance that should be taken into account when checking if waypoint is reached
        /// </summary>
        public float TargetTolerance { get; set; }

        
    }
}
