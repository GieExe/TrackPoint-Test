//C:\Users\Joji\source\repos\TrackPoint-Test\TrackPoint-Test\Models\AuditTrail.cs
using System;

namespace TrackPoint_Test.Models
{
    /// <summary>
    /// Represents a single entry in the user action audit log.
    /// </summary>
    public class AuditTrail
    {
        public long AuditID { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; } // Joined from Users for display
        public string Action { get; set; }
        public int? TargetDocumentID { get; set; }
        public DateTime Timestamp { get; set; }
        public string Details { get; set; }
    }
}
