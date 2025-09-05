//C:\Users\Joji\source\repos\TrackPoint-Test\TrackPoint-Test\Models\RfidTag.cs
using System;

namespace TrackPoint_Test.Models
{
    /// <summary>
    /// Represents a single RFID tag, corresponding to the RFIDTags table.
    /// </summary>
    public class RfidTag
    {
        /// <summary>
        /// The unique Electronic Product Code (EPC) of the tag.
        /// </summary>
        public string TagEPC { get; set; }

        /// <summary>
        /// The ID of the document this tag is assigned to. Null if unassigned.
        /// </summary>
        public int? DocumentID { get; set; }

        public bool IsActive { get; set; }

        /// <summary>
        /// The user who assigned the tag. Null if unassigned.
        /// </summary>
        public int? AssignedByUserID { get; set; }

        /// <summary>
        /// The timestamp when the tag was assigned. Null if unassigned.
        /// </summary>
        public DateTime? AssignedAt { get; set; }
    }
}
