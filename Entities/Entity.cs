using System.Collections.Generic;
using rgEngine.Components;

namespace rgEngine.Entities
{
    /// <summary>
    /// Some base entities that can be enhanced
    /// </summary>
    public enum EntityType
    {
        Enemy,
        Player
    }


    /// <summary>
    /// Defines basic properties common to all entities
    /// </summary>
    class Entity : IEntity
    {
        private readonly EntityType _entityType;

        public Entity(EntityType type)
        {
            _entityType = type;
        }

        private readonly Dictionary<ComponentType, IComponent> _components = new Dictionary<ComponentType, IComponent>();

        public void AddComponent(IComponent component)
        {
            _components.Add(component.ComponentType, component);
        }

        public Dictionary<ComponentType, IComponent> Components()
        {
            return _components;
        }

        public EntityType EntityType()
        {
            return _entityType;
        }

        public IComponent GetComponent(ComponentType type)
        {
            IComponent component;
            return _components.TryGetValue(type, out component) ? component : null;
        }
    }

    internal interface IEntity
    {
        void AddComponent(IComponent component);

        Dictionary<ComponentType, IComponent> Components();

        EntityType EntityType();

        IComponent GetComponent(ComponentType type);
    }

}
