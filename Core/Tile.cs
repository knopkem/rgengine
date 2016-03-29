using Microsoft.Xna.Framework;

namespace rgEngine.Core
{
    /// <summary>
    /// By who the tile can be traversed
    /// </summary>
    public enum TileTypes
    {
        /// <summary>
        /// Everyone can go through
        /// </summary>
        Open,
        /// <summary>
        /// No one can go through
        /// </summary>
        Closed,
        /// <summary>
        /// Under special circumstances ghosts can go there
        /// </summary>
        Home
    }

    /// <summary>
    /// A square of the maze
    /// </summary>
    public class Tile
    {
        private readonly int _size;

        /// <summary>
        /// Sets the different attributes
        /// </summary>
        /// <param name="type">The type of tile</param>
        /// <param name="hasCrump">Whether the tile has a crump</param>
        /// <param name="hasPowerPill">Whether the tile has a power pill</param>
        /// <param name="position">Position in grid</param>
        public Tile(TileTypes type, bool hasCrump, bool hasPowerPill, Point position, int size)
        {
            Type = type;
            HasCrump = hasCrump;
            HasPowerPill = hasPowerPill;
            Position = position;
            _size = size;
        }

        /// <summary>
        /// The type of the tile
        /// </summary>
        public TileTypes Type { get; set; }

        /// <summary>
        /// Whether the tile has a crump
        /// </summary>
        public bool HasCrump { get; set; }

        /// <summary>
        /// Whether the tile has a power pill
        /// </summary>
        public bool HasPowerPill { get; set; }

        public bool IsOpen
        {
            get { return Type == TileTypes.Open; }
        }

        public Point Position { get; set; }

        public int Size { get { return _size; } }

    }
}
