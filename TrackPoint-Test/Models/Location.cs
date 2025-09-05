//C:\Users\Joji\source\repos\TrackPoint-Test\TrackPoint-Test\Models\Location.cs
namespace TrackPoint_Test.Models
{
    /// <summary>
    /// Represents a physical location where an RFID reader is installed.
    /// </summary>
    public class Location
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public int DepartmentID { get; set; }
    }
}
