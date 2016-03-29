using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace rgEngine.Core.RandomLevel {

    /// <summary>
    /// Collection list of skipped locations.
    /// </summary>
    public class SkipListCollection : Collection<Point> {
        /// <summary>
        /// Initializes a new instance of the SkipListCollection class.
        /// </summary>
        public SkipListCollection() :
        base(new List<Point>()) {
        }

        /// <summary>
        /// Copys a SkipListCollection from the origin to the destination.
        /// </summary>
        /// <param name="origin">Origin of the list.</param>
        /// <param name="destination">Destination list.</param>
        public static void Copy(SkipListCollection origin, SkipListCollection destination) {
            foreach (Point location in origin) {
                destination.Add(location);
            }
        }

        /// <summary>
        /// Copys a SkipListCollection to this instance.
        /// </summary>
        /// <param name="origin">Origin of the list.</param>
        public void Copy(SkipListCollection origin) {
            Copy(origin, this);
        }
    }
}
