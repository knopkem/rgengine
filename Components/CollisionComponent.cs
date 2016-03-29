using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace rgEngine.Components
{
    /// <summary>
    /// Collision information needed for collision checks
    /// </summary>
    class CollisionComponent : IComponent
    {

        public ComponentType ComponentType { get { return ComponentType.Collision; } }

        public CollisionComponent(Rectangle frame)
        {
            CollisionFrame = frame;
            CollisionDirections = new List<Vector2>();
        }

        /// <summary>
        /// Frame to test collision against
        /// </summary>
        public Rectangle CollisionFrame { get; set; }

        /// <summary>
        /// Experimental, needed for collision warning system
        /// </summary>
        public List<Vector2> CollisionDirections { get; set; }

        public bool HasCollided { get; set; }

    }
}
