using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace rgEngine.Components
{
    /// <summary>
    /// Component that adds 2d texture appearance to objects
    /// </summary>
    class SpriteComponent : IComponent
    {
        public ComponentType ComponentType { get { return ComponentType.Sprite; } }
        

        public SpriteComponent(Texture2D texture)
        {
            SpriteTexture = texture;
            SpriteSize = new Point(SpriteTexture.Width, SpriteTexture.Height);
            Origin = new Vector2( (float)SpriteTexture.Width/2, (float)SpriteTexture.Height/2);
            RelativePosition = Vector2.Zero;
            Visible = true;
            SpriteColor = Color.White;
            Scale = 1.0f;
            Rotation = 0.0f;
        }

        public Texture2D SpriteTexture { get; set; }

        /// <summary>
        /// Target rectangle, see scale for alternative sizing
        /// </summary>
        public Point SpriteSize { get; set;}

        /// <summary>
        /// Center of rotation (default: center of sprite)
        /// </summary>
        public Vector2 Origin { get; set; }

        public bool Visible { get; set; }

        public Color SpriteColor { get; set; }

        /// <summary>
        /// Position relative to position in position component
        /// </summary>
        public Vector2 RelativePosition { get; set; }

        public float Scale { get; set; }

        public float Rotation { get; set; }
    }
}
