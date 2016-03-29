using System.Collections.Generic;
using Microsoft.Xna.Framework;
using rgEngine.Core.PathFinder;

namespace rgEngine.Core
{
    /// <summary>
    /// Uses a grid interface compliant object to generate waypoint paths
    /// </summary>
    class PathProvider : IPathProvider
    {
        private readonly PathFinderFast _finder;
        private readonly byte[,] _byteArr;
        private readonly int _tileSize;
        private readonly IGrid _grid;

        public PathProvider(IGrid grid)
        {
            _grid = grid;
            _tileSize = grid.TileSize;
            _byteArr = new byte[grid.Width, grid.Height];

            // convert grid to byte array
            for (int j = 0; j < grid.Height; j++)
                for (int i = 0; i < grid.Width; i++)
                {
                    if ( grid.IsTileOpen(i, j) )
                        _byteArr[i, j] = 1; // open
                    else
                        _byteArr[i, j] = 0; // closed
                }

            // our pathfinder algorithm gets initialized here
            _finder = new PathFinderFast(_byteArr);
            _finder.Diagonals = false;
            _finder.HeavyDiagonals = false;
            _finder.PunishChangeDirection = false;
            _finder.TieBreaker = true;
            _finder.SearchLimit = grid.Width * grid.Height;
        }

        public IGrid Grid
        {
            get
            {
                return _grid;
            }
        }

        /// <summary>
        /// Compute the path between begin and end, returns null if nothing was found
        /// </summary>
        /// <param name="begin">Tileposition to start</param>
        /// <param name="end">Tileposition to end</param>
        /// <returns>valid path or null</returns>
        public Queue<Vector2> FindPath(Vector2 begin, Vector2 end) 
        {
            var res = new Queue<Vector2>();
            Point start = Algorithms.World2Grid(begin, _tileSize);
            Point stop = Algorithms.World2Grid(end, _tileSize);
            List<PathFinderNode> path = null;
            if (_grid.IsTileOpen(start.X, start.Y) )
                if (_grid.IsTileOpen(stop.X, stop.Y))
                    path = _finder.FindPath(start, stop);
            
            if (path == null)
                return res;

            path.Reverse();
            foreach (PathFinderNode node in path){
                Vector2 waypoint;
                if (node.X == stop.X && node.Y == stop.Y)
                    waypoint = Algorithms.Grid2World(new Point(node.X, node.Y), _tileSize);
                waypoint = Algorithms.Grid2World(new Point(node.X, node.Y), _tileSize);
                if (waypoint != begin)
                    res.Enqueue(waypoint);
            }

            return res;
        }

     
    }
}
