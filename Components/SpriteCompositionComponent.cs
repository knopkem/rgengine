using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace rgEngine.Components
{
    /// <summary>
    /// Collection component for sprites
    /// </summary>
    class SpriteCompositionComponent : IComponent
    {
        public ComponentType ComponentType { get { return ComponentType.SpriteComposition; } }

        private readonly List<SpriteComponent> _sprites = new List<SpriteComponent>();

        public void AddSprite(SpriteComponent sprite)
        {
            _sprites.Add(sprite);
        }

        public List<SpriteComponent> SpriteList { get { return _sprites; } }

        /// <summary>
        /// Relative position of all sprites in collection to position in position component
        /// </summary>
        public Vector2 RelativePosition { get; set; }
    }
}
