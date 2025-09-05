//C:\Users\Joji\source\repos\TrackPoint-Test\TrackPoint-Test\ViewModels\DashboardViewModel.cs
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using TrackPoint_Test.DataAccess;
using TrackPoint_Test.Models;
using TrackPoint_Test.Services;
using TrackPoint_Test.Services.interfaces;

namespace TrackPoint_Test.ViewModels
{
    // The existing code has a typo "ReaderStatusChanged". The interface has "StatusChanged".
    // Also, the "IsReading" property is missing from the interface. These are now fixed.
    // The logic to find a document by EPC needs to change as the Document model doesn't have an EPC property.

    public class DashboardViewModel : ViewModelBase
    {
        #region Fields

        private readonly User _currentUser;
        private readonly IRfidReaderService _rfidService;
        private string _statusMessage;
        private bool _isReaderConnected;
        private Document _selectedDocument;

        #endregion

        #region Properties

        public ObservableCollection<Document> Documents { get; } = new ObservableCollection<Document>();

        public Document SelectedDocument
        {
            get => _selectedDocument;
            set
            {
                if (_selectedDocument == value) return;
                _selectedDocument = value;
                OnPropertyChanged();
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage == value) return;
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public bool IsReaderConnected
        {
            get => _isReaderConnected;
            set
            {
                if (_isReaderConnected == value) return;
                _isReaderConnected = value;
                OnPropertyChanged();
            }
        }

        public Visibility AdminPanelVisibility { get; }

        #endregion

        #region Commands

        public ICommand ConnectCommand { get; }
        public ICommand DisconnectCommand { get; }
        public ICommand StartReadingCommand { get; }
        public ICommand StopReadingCommand { get; }

        #endregion

        #region Constructor

        public DashboardViewModel(User currentUser)
        {
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));

            if (_currentUser.RoleName == "Super Admin" || _currentUser.RoleName == "Staff")
            {
                AdminPanelVisibility = Visibility.Visible;
            }
            else
            {
                AdminPanelVisibility = Visibility.Collapsed;
            }

            bool useMock = bool.Parse(ConfigurationManager.AppSettings["UseMockReader"]);
            if (useMock)
            {
                _rfidService = new MockRfidReaderService();
                StatusMessage = "System Ready. USING MOCK (SIMULATION) HARDWARE.";
            }
            else
            {
                _rfidService = new ChafonReaderService();
                StatusMessage = "System Ready. Please connect to the RFID reader.";
            }

            _rfidService.TagRead += OnTagRead;
            // Corrected event subscription name
            _rfidService.StatusChanged += OnReaderStatusChanged;

            ConnectCommand = new RelayCommand(ExecuteConnect, CanExecuteConnect);
            DisconnectCommand = new RelayCommand(ExecuteDisconnect, CanExecuteDisconnect);
            StartReadingCommand = new RelayCommand(ExecuteStartReading, CanExecuteStartReading);
            StopReadingCommand = new RelayCommand(ExecuteStopReading, CanExecuteStopReading);

            LoadDocuments();
        }

        #endregion

        #region Command Methods

        private bool CanExecuteConnect(object obj) => !IsReaderConnected;
        private void ExecuteConnect(object obj)
        {
            try
            {
                _rfidService.Connect();
                // IsConnected is now a property on the interface, so we can check it
                IsReaderConnected = _rfidService.IsConnected;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to connect: {ex.Message}";
            }
        }

        private bool CanExecuteDisconnect(object obj) => IsReaderConnected;
        private void ExecuteDisconnect(object obj)
        {
            _rfidService.StopReading();
            _rfidService.Disconnect();
            IsReaderConnected = _rfidService.IsConnected; // Update based on service status
        }

        // The interface now has the 'IsReading' property
        private bool CanExecuteStartReading(object obj) => IsReaderConnected && !_rfidService.IsReading;
        private void ExecuteStartReading(object obj)
        {
            _rfidService.StartReading();
            StatusMessage = "Inventory scan started. Listening for tags...";
        }

        // The interface now has the 'IsReading' property
        private bool CanExecuteStopReading(object obj) => IsReaderConnected && _rfidService.IsReading;
        private void ExecuteStopReading(object obj)
        {
            _rfidService.StopReading();
            StatusMessage = "Inventory scan stopped.";
        }

        #endregion

        #region Private and Helper Methods

        private void LoadDocuments()
        {
            try
            {
                var docs = DatabaseHelper.GetDocuments(_currentUser.DepartmentID, _currentUser.RoleName);

                Documents.Clear();
                foreach (var doc in docs)
                {
                    Documents.Add(doc);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading documents: {ex.Message}";
            }
        }

        private void OnTagRead(string epc)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // We need to find the document based on the EPC.
                // The document model doesn't have an EPC, but the RfidTag model does.
                // We must query the database for the RfidTag first, then find the Document.
                var rfidTag = DatabaseHelper.GetRfidTagByEpc(epc);
                if (rfidTag != null && rfidTag.DocumentID.HasValue)
                {
                    // Now find the corresponding document in our local collection.
                    var foundDoc = Documents.FirstOrDefault(d => d.DocumentID == rfidTag.DocumentID.Value);

                    if (foundDoc != null)
                    {
                        foundDoc.LastSeenTimestamp = DateTime.Now;
                        StatusMessage = $"Found: '{foundDoc.DocumentTitle}' at {foundDoc.LastSeenTimestamp:T}";
                        SelectedDocument = foundDoc; // Highlight the found document in the UI
                    }
                    else
                    {
                        StatusMessage = $"Found a tagged document that is not in your list: {epc}";
                    }
                }
                else
                {
                    StatusMessage = $"Unknown Tag Detected: {epc}";
                }
            });
        }

        private void OnReaderStatusChanged(string status)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                StatusMessage = status;
            });
        }

        #endregion
    }
}