//C:\Users\Joji\source\repos\TrackPoint-Test\TrackPoint-Test\Models\Reader.cs
using System;

namespace TrackPoint_Test.Models
{
    /// <summary>
    /// Represents an RFID reader device.
    /// </summary>
    public class Reader
    {
        public int ReaderID { get; set; }
        public string ReaderName { get; set; }
        public string IPAddress { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; } // Joined from Locations for display
        public int StatusID { get; set; }
        public string StatusName { get; set; } // Joined from HardwareStatus for display
        public DateTime? LastHeartbeat { get; set; }
    }
}
