using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace rgEngine.Core.PathFinder 
{
    /// <summary>
    /// Collection for wrapping the PathfinderNode List.
    /// </summary>
    public class PathFinderNodeCollection : Collection<PathFinderNode> {
        /// <summary>
        /// Initializes a new instance of the PathfinderNodeCollection class.
        /// </summary>
        public PathFinderNodeCollection() : base(new List<PathFinderNode>()) {
        }

        public PathFinderNodeCollection(List<PathFinderNode> nodes) : base( nodes )
        {
            
        }
    }
}
