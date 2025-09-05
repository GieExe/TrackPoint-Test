//C:\Users\Joji\source\repos\TrackPoint-Test\TrackPoint-Test\Models\DocumentStatus.cs
namespace TrackPoint_Test.Models
{
    /// <summary>
    /// Represents a possible status for a document (e.g., "Available", "Missing").
    /// Corresponds to the DocumentStatus lookup table.
    /// </summary>
    public class DocumentStatus
    {
        public int StatusID { get; set; }
        public string StatusName { get; set; }
    }
}
