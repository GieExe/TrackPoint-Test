//C:\Users\Joji\source\repos\TrackPoint-Test\TrackPoint-Test\Services\interfaces\IRfidReaderService.cs
using System;

namespace TrackPoint_Test.Services.interfaces
{
    /// <summary>
    /// Defines the contract for an RFID Reader Service.
    /// This interface provides an abstraction layer for the hardware,
    /// allowing the application to use a real or mock reader interchangeably.
    /// It specifies the essential functionalities any RFID reader must implement.
    /// </summary>
    public interface IRfidReaderService
    {
        /// <summary>
        /// Event that is triggered whenever an RFID tag is successfully read by the hardware.
        /// The string argument passed with the event is the unique EPC (Electronic Product Code) of the tag.
        /// </summary>
        event Action<string> TagRead;

        /// <summary>
        /// Event that is triggered when the reader's connection status changes (e.g., connected, disconnected)
        /// or an error occurs. It provides a descriptive message to be displayed to the user.
        /// </summary>
        event Action<string> StatusChanged; // This was the original name. The ViewModel used a different name.

        /// <summary>
        /// Gets a value indicating whether the service is currently connected to the reader hardware.
        /// </summary>
        /// <value>true if connected; otherwise, false.</value>
        bool IsConnected { get; }

        /// <summary>
        /// Gets a value indicating whether the reader is actively scanning for tags.
        /// </summary>
        bool IsReading { get; } // This property was missing.

        /// <summary>
        /// Attempts to establish a connection with the RFID reader hardware.
        /// This method will handle the necessary SDK calls to initialize the device.
        /// </summary>
        void Connect();

        /// <summary>
        /// Disconnects cleanly from the RFID reader hardware and releases any resources.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Commands the reader to begin the inventory process (continuously scanning for tags).
        /// Tag reads will be reported via the TagRead event.
        /// </summary>
        void StartReading();

        /// <summary>
        /// Commands the reader to stop the inventory process.
        /// </summary>
        void StopReading();
    }
}