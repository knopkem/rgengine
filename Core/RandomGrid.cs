using System.Collections.Generic;
using Microsoft.Xna.Framework;
using rgEngine.Core.RandomLevel;

namespace rgEngine.Core
{
    class RandomGrid : IGrid
    {
        private readonly List<string> _levelStringList = new List<string>();
        private readonly int _width;
        private readonly int _height;

        private readonly Tile[,] _tileGrid = new Tile[64,64];
        private const int _tileSize = 16;

        private readonly CellularAutomata _cellAut = new CellularAutomata();

        /// <summary>
        /// Creates a new Grid object
        /// </summary>
        public RandomGrid()
        {
            _width = _tileGrid.GetLength(0);
            _height = _tileGrid.GetLength(1);
            _cellAut.Size = new Size(_width, _height);
     
            InitGrid();
            GenerateRandomGrid();
        }

        private void ConvertToLevel(GridType [,] grid, Size size)
        {
            for (int j = 0; j < size.Height; j++)
            {
                for (int i = 0; i < size.Width; i++)
                {
                    if ( grid[i,j] == GridType.Empty )
                        _tileGrid[i, j] = new Tile(TileTypes.Open, true, false, new Point(i, j), _tileSize);
                }
            }
        }

        private void GenerateRandomGrid()
        {
            GridType[,] grid = _cellAut.Generate(true);
            ConvertToLevel(grid, _cellAut.Size);
        }

        public Tile[,] TileGrid
        {
            get { return _tileGrid; }
        }

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public int TileSize
        {
            get { return _tileSize;}
        }

        public bool IsTileOpen(int i, int j)
        {
            if (i >= Width || i < 0)
                return false;
            if (j >= Height || j < 0)
                return false;

            if (TileGrid[i, j].Type == TileTypes.Open)
                return true;
            return false;
        }

        private void InitGrid()
        {
            for (int j = 0; j < Height; j++)
                for (int i = 0; i < Width; i++)
                {
                    TileGrid[i, j] = new Tile(TileTypes.Closed, false, false, new Point(i, j), _tileSize);
                }

            _cellAut.Density = 40;
            _cellAut.IterationsMutationFill = 5;
            _cellAut.IterationsMutationOnly = 2;
            _cellAut.FillSearchArea = 2;
            _cellAut.CountForValidWalls = 5;
            _cellAut.CountForValidWallsForFill = 2;
        }
    }
}
