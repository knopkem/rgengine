
namespace rgEngine.Core
{
    interface IGrid
    {
        int Width { get; }

        int Height { get; }

        int TileSize { get; }

        bool IsTileOpen(int i, int j);

        Tile[,] TileGrid { get; }
    }
}
