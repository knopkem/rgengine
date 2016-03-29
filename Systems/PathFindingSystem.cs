using System;
using Microsoft.Xna.Framework;
using rgEngine.Components;
using rgEngine.Core;

namespace rgEngine.Systems
{
    /// <summary>
    /// Uses a pathfinder to update the path of all entities that have PathComponents
    /// </summary>
    class PathFindingSystem : BaseSystem , IUpdateable
    {
        private IPathProvider _provider;

        private readonly Random _rand = new Random();

        public IPathProvider PathProvider
        {
            set
            {
                _provider = value;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_provider == null)
                return;

            // cycle all entities and move all enemies
            foreach (var entity in Entities)
            {
      
                // get the positional component and move it randomly
                var pos = entity.GetComponent(ComponentType.Position) as PositionComponent;
                var path = entity.GetComponent(ComponentType.Path) as PathComponent;

                // safety check
                if (IsComponentNull(pos) || IsComponentNull(path))
                    continue;

                /*
                // check if target is reached
                if (Algorithms.IsNearTarget(pos.Position, path.Target, path.TargetTolerance) )
                {
                    GenerateNewTarget(path);
                }
                */

                // check if path is empty
                if (path.Path !=null && path.Path.Count == 0)
                {                    
                    GenerateNewTarget(path);                   
                }

                // check if we need to compute a path (target different from queue endpoint)
                if (IsUpdateRequired(pos.Position, path))
                {                    
                    path.Path = _provider.FindPath(pos.Position, path.Target);
                }
            }
        }

        private static bool IsUpdateRequired( Vector2 position, PathComponent path)
        {
            
                if (path.Path == null)
                    return true;

                if (path.Path.Count == 0)
                    return true;               
            /*
                Vector2 targetInQueue = path.Path.ToArray()[path.Path.Count - 1];
                // if target is the last item in queue no update is needed 
                if (Algorithms.IsNearTarget(targetInQueue, path.Target, path.TargetTolerance))
                    return false;
            */
                return false;
            
        }

        private void GenerateNewTarget(PathComponent path)
        {
            // create a new random target
            Vector2 maxWorld = Algorithms.Grid2World(new Point(27, 30), 16);

            int X, Y;
            Point tilePos;
            do
            {
                X = _rand.Next(0, (int)maxWorld.X);
                Y = _rand.Next(0, (int)maxWorld.Y);
                tilePos = Algorithms.World2Grid(new Vector2(X, Y), 16);

            } while (!_provider.Grid.IsTileOpen(tilePos.X, tilePos.Y));

            path.Target = new Vector2(X, Y);
        }
  
    }
}
