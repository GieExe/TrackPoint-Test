//C:\Users\Joji\source\repos\TrackPoint-Test\TrackPoint-Test\Models\DocumentMovement.cs
using System;

namespace TrackPoint_Test.Models
{
    /// <summary>
    /// Represents a single logged event of a tag being read at a location.
    /// </summary>
    public class DocumentMovement
    {
        public long MovementID { get; set; }
        public string TagEPC { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; } // Joined from Locations for display
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Received Signal Strength Indicator.
        /// </summary>
        public int RSSI { get; set; }
    }
}
