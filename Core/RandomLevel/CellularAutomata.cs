using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using rgEngine.Core.PathFinder;

namespace rgEngine.Core.RandomLevel 
{

    /// <summary>
    /// Stores an ordered pair of integers (System.Drawing clone)
    /// </summary>
    public class Size
    {
        private readonly int _width;
        private readonly int _height;

        public Size(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public int Height
        {
            get { return _height; }
        }

        public int Width
        {
            get { return _width; }
        }
    }


    /// <summary>
    /// Built-in algorithm: Cellular Automata.
    /// </summary>
    public class CellularAutomata
    {

        #region private members

        /// <summary>
        /// Grid to which the algorithm will build on.
        /// </summary>
        private GridType[,] _grid;

        /// <summary>
        /// Density of initial placement of filled tiles.
        /// </summary>
        private int _density;

        /// <summary>
        /// List of locations that need to be left empty.
        /// </summary>
        private SkipListCollection _skipList;

        /// <summary>
        /// Size of the level.
        /// </summary>
        private Size _size;

        /// <summary>
        /// Width of the level.
        /// </summary>
        private int _width;

        /// <summary>
        /// Height of the level.
        /// </summary>
        private int _height;

        /// <summary>
        /// Random number generation seed.
        /// </summary>
        private int _seed;

        /// <summary>
        /// Level bounds.
        /// </summary>
        private Rectangle _bounds;

        /// <summary>
        /// Random number generator.
        /// </summary>
        private Random _rand;

        /// <summary>
        /// Number of open tiles in level.
        /// </summary>
        private int _openCount;

        #endregion private members

        #region public properties

        /// <summary>
        /// Gets the bounds of the level.
        /// </summary>
        /// <value>
        /// The bounds of the level.
        /// </value>
        public Rectangle Bounds {
            get {
                return _bounds;
            }
        }

        /// <summary>
        /// Gets a list of locations to be skipped (left empty) during generation.
        /// </summary>
        /// <value>
        /// A list of locations to be skipped (left empty) during generation.
        /// </value>
        public SkipListCollection SkipList {
            get {
                if (_skipList != null) 
                    return _skipList;

                _skipList = new SkipListCollection();
                return _skipList;
            }
        }

        /// <summary>
        /// Gets or sets the size of the level.
        /// </summary>
        /// <value>
        /// The size of the level.
        /// </value>
        public Size Size {
            get {
                return _size;
            }

            set {
                _size = value;
                _width = value.Width;
                _height = value.Height;
                _bounds = new Rectangle(0, 0, value.Width, value.Height);
            }
        }

        /// <summary>
        /// Gets or sets the random number generation seed.
        /// </summary>
        /// <value>
        /// The random number generation seed.
        /// </value>
        public int Seed {
            get {
                return _seed;
            }

            set {
                _seed = value;
            }
        }

        /// <summary>
        /// Gets the width of the level.
        /// </summary>
        /// <value>
        /// The width of the level.
        /// </value>
        public int Width {
            get {
                return _width;
            }
        }

        /// <summary>
        /// Gets the height of the level.
        /// </summary>
        /// <value>
        /// The height of the level.
        /// </value>
        public int Height {
            get {
                return _height;
            }
        }

        /// <summary>
        /// Gets or sets a value between 0 - 100 that represents the density of of the level. The higher the number, the
        /// more dense it will be. A value of 100 equals 100 percent density.
        /// </summary>
        /// <value>
        /// A value between 0 - 100 that represents the density of of the level.
        /// </value>
        public int Density {
            get {
                return _density;
            }

            set {
                if (value < 0 || value > 100) {
                    throw new ArgumentOutOfRangeException();
                }

                _density = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of iterations for mutation and fill in.
        /// </summary>
        /// <value>
        /// The number of iterations for mutation and fill in.</value>
        public int IterationsMutationFill { get; set; }

        /// <summary>
        /// Gets or sets the number iterations for cellular mutation only.
        /// </summary>
        /// <value>
        /// The number iterations for cellular mutation only.
        /// </value>
        public int IterationsMutationOnly { get; set; }

        /// <summary>
        /// Gets or sets the distance from center point to check for filling.
        /// </summary>
        /// <value>
        /// The distance from center point to check for filling.
        /// </value>
        public int FillSearchArea { get; set; }

        /// <summary>
        /// Gets or sets the number of filled tiles needed to connect to the center point for
        /// it to be classified as a filled tile.
        /// </summary>
        /// <value>
        /// The number of filled tiles needed to connect to the center point for
        /// it to be classified as a filled tile.
        /// </value>
        public int CountForValidWalls { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of filled tiles needed to fill an area.
        /// </summary>
        /// <value>
        /// The minimum number of filled tiles needed to fill an area.
        /// </value>
        public int CountForValidWallsForFill { get; set; }

        #endregion public properties
        
        #region public methods

        /// <summary>
        /// Starts the cellular automata algorithm.
        /// </summary>
        /// <param name="connectSeperated">Should the seperated sections be connected by a passage.</param>
        /// <returns>Returns the grid array.</returns>
        public GridType[,] Generate(bool connectSeperated) {
            Initialize();

            // Set the buffer to be exactly that same as the grid.
            var gridBuffer = _grid.Clone() as GridType[,];
            int totalIter = IterationsMutationFill + IterationsMutationOnly;
            bool imf = IterationsMutationFill != 0;
            NeighborAnaylsis neighbors;

            // Iterate through the entire level for the set number of iterations.
            for (int iter = 0; iter < totalIter; iter++) {
                // Iterate through the entire level...
                if (_grid != null)
                {
                    for (int y = _grid.GetLowerBound(1) + 1; y <= _grid.GetUpperBound(1) - 1; y++) {
                        for (int x = _grid.GetLowerBound(0) + 1; x <= _grid.GetUpperBound(0) - 1; x++) {
                            // Get the location.
                            var location = new Point(x, y);
                        
                            // If the location is a part of the passage out structure, then bypass it...
                            if (_skipList != null && _skipList.Contains(location)) {
                                continue;
                            }
                        
                            // Check to see if this location is buildable.
                            // Reset flags...
                            bool check1 = false;
                            bool check2 = false;
                            bool doBoth = false;
                        
                            // Check to see if we are filling in during this iteration.
                            if (imf && iter < IterationsMutationFill) {
                                neighbors = AreaGridAnalyze(location.X - FillSearchArea, location.Y - FillSearchArea, (FillSearchArea << 1) + 1, (FillSearchArea << 1) + 1);
                                doBoth = true;
                            } else {
                                neighbors = AreaGridAnalyze(location.X - 1, location.Y - 1, 3, 3);
                            }
                        
                            // Check to see if this location has enough neighbors to stay/made into a wall.
                            if (neighbors.Neighbors >= CountForValidWalls) {
                                check1 = true;
                            }
                        
                            // Check to see if this location is within a sparse area and requires filling...
                            if (neighbors.Region <= CountForValidWallsForFill) {
                                check2 = true;
                            }
                        
                            // If this is a valid build location.....
                            // ....And passes all check to be made into a wall....
                            if (check1 || (check2 && doBoth)) {
                                // ....Set this location to a wall.
                                if (gridBuffer != null) 
                                    gridBuffer[x, y] = GridType.Filled;
                            } else {
                                // ...no, so make it a floor....
                                if (gridBuffer != null) 
                                    gridBuffer[x, y] = GridType.Empty;
                            }
                        }
                    }
                
                    // Now, copy the changes made from the iteration back to the original grid.
                    // It is important that the generation process not alter the level until the entire level
                    // is processed.
                    if (gridBuffer != null) 
                        _grid = gridBuffer.Clone() as GridType[,];
                }
                if (connectSeperated) {
                    AnalyzeSeperation();
                }
            }
            
            return _grid;
        }

        #endregion public methods

        #region private methods

        /// <summary>
        /// Count all of the floor tiles within the level. Gives a baseline for the seperation scanner.
        /// </summary>
        private void CountOpen() {
            _openCount = 0;
            for (int y = 1; y < _grid.GetLength(1) - 1; y++) {
                for (int x = 1; x < _grid.GetLength(0) - 1; x++) {
                    GridType tile = _grid[x, y];
                    if (tile == GridType.Empty) {
                        _openCount++;
                    }
                }
            }
        }

        /// <summary>
        /// Generates a random point within the bounds of the level.
        /// </summary>
        /// <returns>A random point.</returns>
        private Point GenerateRandomPoint() {
            var temp = new Point();
            do {
                temp.X = _rand.Next(0, Width);
                temp.Y = _rand.Next(0, Height);
            } while (!IsLocationWithinBounds(temp));
            return temp;
        }

        /// <summary>
        /// Checks to see if a point is within the level's boundries.
        /// </summary>
        /// <param name="location">Point to check.</param>
        /// <returns>True if the point is within bounds, false otherwise.</returns>
        private bool IsLocationWithinBounds(Point location) {
            return Bounds.Contains(location);
        }

        /// <summary>
        /// Checks to see if the location has a wall or not.
        /// </summary>
        /// <param name="location">Location to check.</param>
        /// <returns>True if there is a wall at the point, otherwise false.</returns>
        private bool IsLocationFilled(Point location)
        {
            if (IsLocationWithinBounds(location)) {
                return _grid[location.X, location.Y] == GridType.Filled;
            }
            return false;
        }

        /// <summary>
        /// This method will anaylze the structure looking for seperated sections. When found, it will call the pathfinder
        /// to connect the sections from the largest(main) section to the smaller sections.
        /// </summary>
        private void AnalyzeSeperation() {
            // Some local variables...
            Point start;
            Point largestPoint = Point.Zero;
            int largestCount = 0;
            var startPoints = new List<Point>();
            var buffer = new HashSet<Point>();
            
            // Count the total number of floor tiles within level, use this for comparison.
            CountOpen();
            int count = 0;
            
            // Loop to find each disconnected section...
            do {
                // Loop to find a random point that is withing bounds, not a wall, and not already tagged by the scanner.
                do {
                    start = GenerateRandomPoint();
                } while (IsLocationFilled(start) ||
                         buffer.Contains(start) ||
                         (start.X == 0 ||
                          start.Y == 0 ||
                          start.X == _grid.GetLength(0) - 1 ||
                          start.Y == _grid.GetLength(1) - 1));
                
                // Once a legal starting point has been found, add it to a list.
                startPoints.Add(start);
                
                // Use flood fill from this found point to find all floor tiles connected to it.
                FloodFill(start, buffer);
                
                // Find out how many floor tiles were counted this trip through...
                count = (count != buffer.Count ? Math.Abs(buffer.Count - count) : buffer.Count);
                
                // Since we want to connect the smaller sections to the largest(main) section, find out which one
                // is the largest.
                if (count > largestCount) {
                    largestCount = count;
                    largestPoint = start;
                }
                
                // Keep looping until the entire level has been scanned.
            } while (buffer.Count != _openCount);

            // If there was more than one section detected, then we need to connect them.
            if (startPoints.Count != 1) {
                // Iterate through all of the sections.
                foreach (Point t in startPoints)
                {
                    // Since we are connected them to the largest, skip if the next point is from the largest.
                    if (t == largestPoint) {
                        continue;
                    }
                    
                    // Set up the pathfinder to find the shortest path from the random point found in the largest section,
                    // to the point in the smaller section.
                    IPathFinder connect;
                    
                    // If its a small cavern, i.e. from a cavern imbedded in a dungeon level, then use a more acurate, but slower
                    // pathfinder. Using the faster method with smaller levels tend to create errors from the pathfinder.
                    if (_openCount > 1000) {
                        // The ConvertToPathFinder method has a special optional boolean parameter that sets up the grid
                        // to make floors heavier than floors, unlike with normal use, the walls are impassable.
                        connect = new PathFinderFast(ConvertToPathfinder());
                    } else {
                        connect = new PathFinder.PathFinder(ConvertToPathfinder());
                    }
                    
                    // Setting to pass to the pathfinder.
                    connect.Diagonals = false;
                    connect.Formula = HeuristicFormula.Manhattan;
                    connect.PunishChangeDirection = false;
                    connect.HeavyDiagonals = false;
                    connect.TieBreaker = true;
                    connect.SearchLimit = 500000;
                    
                    // Run the pathfinder and place the path in the solution list.
                    var solution =  new PathFinderNodeCollection(connect.FindPath(largestPoint, t));
                    
                    // Iterate through the list and make any path segment that was a wall into a floor tile.
                    foreach (PathFinderNode node in solution) {
                        _grid[node.X, node.Y] = GridType.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// Converts the level data into a byte array with weights for the pathfinder.
        /// </summary>
        /// <returns>The byte array.</returns>
        private byte[,] ConvertToPathfinder() {
            int convWidth = 0;
            int convHeight = 0;
            var multiples = new List<int>(11);
            for (int iter = 2; iter < 2050; iter *= 2) {
                multiples.Add(iter);
            }
            
            // Cavern must be multiples of two for the pathfinder to work
            if (!multiples.Contains(Width)) {
                int wid = Width << 1;
                for (int iter = 2; iter < wid; iter *= 2) {
                    convWidth = iter;
                }
            } else {
                convWidth = Width;
            }
            
            if (!multiples.Contains(Height)) {
                int hei = Height << 1;
                for (int iter = 2; iter < hei; iter *= 2) {
                    convHeight = iter;
                }
            } else {
                convHeight = Height;
            }
            
            var temp = new byte[convWidth, convHeight];
            for (int y = 0; y <= Height - 1; y++) {
                for (int x = 0; x <= Width - 1; x++) {
                    GridType tile = _grid[x, y];
                    if (x == 0 ||
                        x == _grid.GetLength(0) - 1 ||
                        y == 0 ||
                        y == _grid.GetLength(1) - 1) {
                        temp[x, y] = 0;
                    } else {
                        if (tile == GridType.Empty) {
                            temp[x, y] = 1;
                        } else {
                            temp[x, y] = 10;
                        }
                    }
                }
            }
            
            return temp;
        }

        /// <summary>
        /// The scanline method is used by the FloodFill alorithm to fill the line from the given point from one wall
        /// to the other.
        /// </summary>
        /// <param name="loc">Point to begin the scanning from.</param>
        /// <param name="line">Queue buffer to place the results in.</param>
        private void ScanLine(Point loc, Queue<Point> line) {
            // Save time by referencing these points once.
            int locx = loc.X;
            int locy = loc.Y;
            
            // Iterate from the point given to the left of it, until a wall is reached.
            for (int x = locx; x > 0; x--) {
                // Make sure it's within bounds, save stack calls by not using the method: IsLocationWithinBounds.
                if (x > 0 &&
                    locy > 0 &&
                    x < _grid.GetLength(0) - 1 &&
                    locy < _grid.GetLength(1) - 1) {
                    // Now, check to see if the tile is a wall, again, save stack calls by not calling: IsLocationAWall.
                    if (_grid[x, locy] != GridType.Filled) {
                        // It's not a wall, so add the point to the queue.
                        line.Enqueue(new Point(x, locy));
                    } else {
                        break;
                    }
                } else {
                    break;
                }
            }
            
            // Now, do the same thing to the right of the given point, up to the wall.
            for (int x = locx + 1; x < _grid.GetLength(0); x++) {
                if (x > 0 &&
                    locy > 0 &&
                    x < _grid.GetLength(0) - 1 &&
                    locy < _grid.GetLength(1) - 1) {
                    if (_grid[x, locy] != GridType.Filled) {
                        line.Enqueue(new Point(x, locy));
                    } else {
                        break;
                    }
                } else {
                    break;
                }
            }
        }

        /// <summary>
        /// Queue-Linear Flood fill algorithm as described by: J. Dunlap.
        /// http://www.codeproject.com/KB/GDI-plus/queuelinearfloodfill.aspx
        /// Used to determine which areas of the cavern are connected and which ones are isolated.
        /// </summary>
        /// <param name="loc">Location to fill from.</param>
        /// <param name="buffer">List of points buffer to record the filling.</param>
        private void FloodFill(Point loc, HashSet<Point> buffer) {
            // Queue buffer to store the horizontal scan line.
            var line = new Queue<Point>();
            
            // Fill the entire line, to the left and right of the point, record the results in the queue buffer.
            ScanLine(loc, line);
            
            // Add the queue buffer to the main buffer.
            foreach (Point pnt in line) {
                buffer.Add(pnt);
            }
            
            // If nothing was filled, then leave.
            if (line.Count == 0) {
                return;
            }
            
            // Loop through all of the points in the queue buffer.
            do {
                // Pop off a point...
                Point check = line.Dequeue();
                
                // Get the point above it...
                var check1 = new Point(check.X, check.Y - 1);
                
                // Check to see if this point is already in the main buffer...
                if (!buffer.Contains(check1)) {
                    // ... or a wall...
                    if (_grid[check1.X, check1.Y] != GridType.Filled) {
                        // ... if not, then recurse the flood fill method for the new line.
                        FloodFill(check1, buffer);
                    }
                }
                
                // Now, get the point that was below the point...
                var check2 = new Point(check.X, check.Y + 1);
                
                // Check for the same things....
                if (!buffer.Contains(check2)) {
                    if (_grid[check2.X, check2.Y] != GridType.Filled) {
                        // ...fill down now...
                        FloodFill(check2, buffer);
                    }
                }
                
                // Keep looping until there is no more points in the queue.
            } while (line.Count != 0);
        }

        /// <summary>
        /// Analyzes the neighboring points within a given area.
        /// </summary>
        /// <param name="areaX">X start position of area.</param>
        /// <param name="areaY">Y start position of area.</param>
        /// <param name="areaWidth">Width of area.</param>
        /// <param name="areaHeight">Height of area.</param>
        /// <returns>Area count results.</returns>
        private NeighborAnaylsis AreaGridAnalyze(int areaX, int areaY, int areaWidth, int areaHeight) {
            var result = new NeighborAnaylsis {Neighbors = 0, Region = 0};
            int centerX = areaX + (areaWidth / 2);
            int centerY = areaY + (areaHeight / 2);
            for (int y = areaY; y < areaY + areaHeight; y++) {
                for (int x = areaX; x < areaX + areaWidth; x++) {
                    if (x >= centerX - 1 && x <= centerX + 1 && y >= centerY - 1 && y <= centerY + 1) {
                        if (x >= 0 && y >= 0 && x < Width && y < Height) {
                            if (_grid[x, y] == GridType.Filled) {
                                result.Neighbors++;
                                result.Region++;
                            }
                        } else {
                            result.Neighbors++;
                            result.Region++;
                        }
                    } else {
                        if (x >= 0 && y >= 0 && x < Width && y < Height) {
                            if (_grid[x, y] == GridType.Filled) {
                                result.Region++;
                            }
                        } else {
                            result.Region++;
                        }
                    }
                }
            }
            
            return result;
        }

        /// <summary>
        /// Initializes the variables and the level for a fresh generation.
        /// </summary>
        private void Initialize() {
            // Set random seed. If one was given, then use that, if not, use default.
            _rand = _seed != 0 ? new Random(_seed) : new Random();

            // Set level arrays...
            _grid = new GridType[Width, Height];

            // Randomly fill level based on initial density.
            for (int y = _grid.GetLowerBound(1) + 1; y <= _grid.GetUpperBound(1) - 1; y++) {
                for (int x = _grid.GetLowerBound(0) + 1; x <= _grid.GetUpperBound(0) - 1; x++)
                {
                    int pick = _rand.Next(101);
                    if (pick >= Density) {
                        _grid[x, y] = GridType.Empty;
                    } else {
                        _grid[x, y] = GridType.Filled;
                    }
                }
            }

            if (_skipList != null)
            {
                // Fill all points within the passage out structure with floor tiles.
                foreach (Point location in _skipList)
                {
                    _grid[location.X, location.Y] = GridType.Empty;
                }
            }
        }

        /// <summary>
        /// Structure for analysis of tile neighbors.
        /// </summary>
        private struct NeighborAnaylsis {
            /// <summary>
            /// Number of valid tiles connected to center.
            /// </summary>
            public int Neighbors;

            /// <summary>
            /// Number of valid tiles within region.
            /// </summary>
            public int Region;
        }

        #endregion private methods

    }
}