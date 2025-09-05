//C:\Users\Joji\source\repos\TrackPoint-Test\TrackPoint-Test\Models\Document.cs
using System;

namespace TrackPoint_Test.Models
{
    /// <summary>
    /// Represents a single document asset, corresponding to the Documents table.
    /// </summary>
    public class Document
    {
        public int DocumentID { get; set; }
        public string DocumentTitle { get; set; }
        public string DocumentReferenceCode { get; set; }
        public string Description { get; set; }
        public int OwningDepartmentID { get; set; }
        public string DepartmentName { get; set; } // Joined from Departments table
        public int StatusID { get; set; }
        public string StatusName { get; set; } // Joined from DocumentStatus table
        public int? LastKnownLocationID { get; set; }
        public string LocationName { get; set; } // Joined from Locations table
        public DateTime? LastSeenTimestamp { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
