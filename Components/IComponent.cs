

namespace rgEngine.Components
{
    public enum ComponentType
    {
        Position,
        Movement,
        Sprite,
        SpriteComposition,
        SpriteAnimationComponent,
        Collision,
        Grid,
        Path
    }

    interface IComponent
    {
        ComponentType ComponentType { get; }
    }
}
