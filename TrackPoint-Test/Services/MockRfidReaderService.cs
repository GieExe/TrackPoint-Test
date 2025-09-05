//C:\Users\Joji\source\repos\TrackPoint-Test\TrackPoint-Test\Services\MockRfidReaderService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using TrackPoint_Test.Services.interfaces;

namespace TrackPoint_Test.Services
{
    public class MockRfidReaderService : IRfidReaderService
    {
        public event Action<string> TagRead;
        public event Action<string> StatusChanged;

        public bool IsConnected { get; private set; }
        public bool IsReading { get; private set; } // This property was missing

        private readonly DispatcherTimer _simulationTimer;
        private readonly List<string> _mockTagEpcs;
        private readonly Random _random;

        public MockRfidReaderService()
        {
            _random = new Random();
            _mockTagEpcs = new List<string>
            {
                "E2000016341200381940C517",
                "E2000017341200381940C518",
                "300833B2DDD9014000000000",
                "300E9992047D61F000000000",
                "00000000000000000000ABCD",
                "E28011606000020611B108A7"
            };

            _simulationTimer = new DispatcherTimer();
            _simulationTimer.Interval = TimeSpan.FromSeconds(2);
            _simulationTimer.Tick += OnTimerTick;
        }

        public void Connect()
        {
            if (IsConnected) return;
            IsConnected = true;
            StatusChanged?.Invoke("Mock Reader Connected Successfully.");
        }

        public void Disconnect()
        {
            if (!IsConnected) return;
            if (IsReading)
            {
                StopReading();
            }
            IsConnected = false;
            StatusChanged?.Invoke("Mock Reader Disconnected.");
        }

        public void StartReading()
        {
            if (!IsConnected)
            {
                StatusChanged?.Invoke("Error: Cannot start reading. Not connected.");
                return;
            }
            if (IsReading) return;

            _simulationTimer.Start();
            IsReading = true; // Set the property to true
            StatusChanged?.Invoke("Mock reading started...");
        }

        public void StopReading()
        {
            if (!IsReading) return;

            _simulationTimer.Stop();
            IsReading = false; // Set the property to false
            StatusChanged?.Invoke("Mock reading stopped.");
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            int index = _random.Next(_mockTagEpcs.Count);
            string randomEpc = _mockTagEpcs[index];
            TagRead?.Invoke(randomEpc);
        }
    }
}