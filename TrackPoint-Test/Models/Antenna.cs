//C:\Users\Joji\source\repos\TrackPoint-Test\TrackPoint-Test\Models\Antenna.cs
namespace TrackPoint_Test.Models
{
    /// <summary>
    /// Represents a single antenna connected to an RFID reader.
    /// </summary>
    public class Antenna
    {
        public int AntennaID { get; set; }
        public int ReaderID { get; set; }
        public int PortNumber { get; set; }
        public int StatusID { get; set; }
        public string StatusName { get; set; } // Joined from HardwareStatus for display
    }
}
