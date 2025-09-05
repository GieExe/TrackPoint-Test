//C:\Users\Joji\source\repos\TrackPoint-Test\TrackPoint-Test\Models\HardwareStatus.cs
namespace TrackPoint_Test.Models
{
    /// <summary>
    /// Represents a possible status for a piece of hardware (e.g., "Online", "Offline").
    /// This is used for BOTH Readers and Antennas.
    /// Corresponds to the HardwareStatus lookup table.
    /// </summary>
    public class HardwareStatus
    {
        public int StatusID { get; set; }
        public string StatusName { get; set; }
    }
}
