using rgEngine.Core;

namespace rgEngine.Components
{
    /// <summary>
    /// The grid component stores information about the current map
    /// Needed for pathfinding
    /// </summary>
    class GridComponent : IComponent
    {

        private readonly int _width;
        private readonly int _height;
        private readonly Tile[,] _tileGrid;
        private readonly IGrid _grid;

        public ComponentType ComponentType { get { return ComponentType.Grid; } }

        public GridComponent(IGrid grid)
        {
            _grid = grid;
            _tileGrid = grid.TileGrid;
            _width = _tileGrid.GetLength(0);
            _height = _tileGrid.GetLength(1);
        }

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public Tile[,] TileGrid
        {
            get { return _tileGrid; }
        }

        public int TileSize
        {
            get { return _tileGrid[0,0].Size; }
        }

        public  IGrid GridInstance
        {
            get { return _grid; }
        }

    }
}
