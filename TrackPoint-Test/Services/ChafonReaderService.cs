//C:\Users\Joji\source\repos\TrackPoint-Test\TrackPoint-Test\Services\ChafonReaderService.cs
using System;
using System.Configuration;
using System.Windows;
using TrackPoint_Test.Services.interfaces;

// TODO: Add the 'using' statement for the Chafon SDK namespace after you reference its .dll
// Example: using Chafon.UHFReader;

namespace TrackPoint_Test.Services
{
    public class ChafonReaderService : IRfidReaderService
    {
        public event Action<string> TagRead;
        public event Action<string> StatusChanged;

        public bool IsConnected { get; private set; }
        public bool IsReading { get; private set; } // This property was missing

        private object _readerDevice; // Using 'object' for now. Cast it to the SDK's type.
        private readonly string _readerIpAddress;
        private readonly int _readerPort;

        public ChafonReaderService()
        {
            _readerIpAddress = ConfigurationManager.AppSettings["ReaderIPAddress"] ?? "192.168.1.116";
            _readerPort = int.Parse(ConfigurationManager.AppSettings["ReaderPort"] ?? "27011");
        }

        public void Connect()
        {
            if (IsConnected) return;
            try
            {
                // TODO: 1. Instantiate your reader object from the SDK
                // Example: _readerDevice = new UHFReader();
                // TODO: 2. Subscribe to the SDK's tag reading event.
                // Example: ((UHFReader)_readerDevice).TagReadEvent += OnReaderTagRead;
                // TODO: 3. Call the SDK's method to connect to the reader.
                int statusCode = 0; // Placeholder for success

                if (statusCode == 0)
                {
                    IsConnected = true;
                    OnStatusChanged($"Successfully connected to reader at {_readerIpAddress}.");
                }
                else
                {
                    IsConnected = false;
                    OnStatusChanged($"Failed to connect. SDK Error Code: {statusCode}.");
                }
            }
            catch (Exception ex)
            {
                IsConnected = false;
                OnStatusChanged($"Connection Error: {ex.Message}");
            }
        }

        public void Disconnect()
        {
            if (!IsConnected) return;
            try
            {
                // TODO: Call the SDK's 'StopReading' or 'StopInventory' method first, if required.
                // Example: ((UHFReader)_readerDevice).StopInventory();
                // TODO: Call the SDK's disconnect method.
                // Example: ((UHFReader)_readerDevice).Disconnect();
                // TODO: Unsubscribe from the event to prevent memory leaks.
                // Example: ((UHFReader)_readerDevice).TagReadEvent -= OnReaderTagRead;

                _readerDevice = null;
            }
            catch (Exception ex)
            {
                OnStatusChanged($"Disconnection Error: {ex.Message}");
            }
            finally
            {
                IsConnected = false;
                IsReading = false; // Set the property to false
                OnStatusChanged("Reader disconnected.");
            }
        }

        public void StartReading()
        {
            if (!IsConnected)
            {
                OnStatusChanged("Cannot start reading. Please connect to the reader first.");
                return;
            }
            try
            {
                // TODO: Call the SDK's method to start reading tags (often called 'StartInventory' or 'ReadTags').
                // Example: ((UHFReader)_readerDevice).StartInventory();
                IsReading = true; // Set the property to true
                OnStatusChanged("Reader started scanning for tags...");
            }
            catch (Exception ex)
            {
                OnStatusChanged($"Error starting scan: {ex.Message}");
            }
        }

        public void StopReading()
        {
            if (!IsConnected) return;
            try
            {
                // TODO: Call the SDK's method to stop reading tags.
                // Example: ((UHFReader)_readerDevice).StopInventory();
                IsReading = false; // Set the property to false
                OnStatusChanged("Reader stopped scanning.");
            }
            catch (Exception ex)
            {
                OnStatusChanged($"Error stopping scan: {ex.Message}");
            }
        }

        private void OnReaderTagRead(object sender, EventArgs e)
        {
            try
            {
                string epc = "E200" + new Random().Next(10000, 99999); // Placeholder
                if (!string.IsNullOrEmpty(epc))
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        TagRead?.Invoke(epc);
                    });
                }
            }
            catch
            {
                // Suppress errors here to prevent the SDK's background thread from crashing.
            }
        }

        private void OnStatusChanged(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                StatusChanged?.Invoke(message);
            });
        }
    }
}