using Microsoft.Xna.Framework;

namespace rgEngine.Components
{
    /// <summary>
    /// Properties of all components that have a visual appearance
    /// </summary>
    class PositionComponent : IComponent
    {
        
        public ComponentType ComponentType { get { return ComponentType.Position; } }

        public PositionComponent(Vector2 initialPosition)
        {
            Position = initialPosition;
            Rotation = 0;
        }

        /// <summary>
        /// Position of object
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Rotation property of object (will be applied to all sprites)
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Amount of pixel the object is off the center of a tile
        /// </summary>
        public Point DeltaPixel { get; set; }

        /// <summary>
        /// The tile the entity is on.
        /// </summary>
        public Point Tile;

    }
}
