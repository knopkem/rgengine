using Microsoft.Xna.Framework;

namespace rgEngine.Components
{
    /// <summary>
    /// All objects that can move need this
    /// </summary>
    class MovementComponent : IComponent
    {
        public ComponentType ComponentType { get { return ComponentType.Movement; } }

        public MovementComponent(Vector2 initialDirection, float speed)
        {
            Direction = initialDirection;            
            MoveSpeed = speed;
        }

        /// <summary>
        /// Direction vector not normalized
        /// </summary>
        public Vector2 Direction { get; set; }
      
        /// <summary>
        /// Speed in pixel to move per frame
        /// </summary>
        public float MoveSpeed { get; set; }
    }
}
