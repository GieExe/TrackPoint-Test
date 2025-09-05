//C:\Users\Joji\source\repos\TrackPoint-Test\TrackPoint-Test\ViewModels\MainViewModel.cs
using System.Windows;
using System.Windows.Input;
using TrackPoint_Test.Models;

namespace TrackPoint_Test.ViewModels
{
    /// <summary>
    /// The main ViewModel for the application. It acts as a shell,
    /// managing the current view and the overall application state (e.g., logged-in user).
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private ViewModelBase _currentViewModel;
        private User _currentUser;
        private LoginViewModel _loginViewModel;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the currently active ViewModel. The ContentControl in MainWindow.xaml
        /// is bound to this property. When this property changes, the displayed view changes.
        /// </summary>
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (_currentViewModel == value) return;
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Stores the currently authenticated user.
        /// </summary>
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                if (_currentUser == value) return;
                _currentUser = value;
                OnPropertyChanged();
                // Notify the UI that related properties have also changed.
                OnPropertyChanged(nameof(IsUserLoggedIn));
                OnPropertyChanged(nameof(LoggedInUserGreeting));
            }
        }

        /// <summary>
        /// A calculated property that returns true if a user is logged in.
        /// Used to control visibility of UI elements like the header or logout button.
        /// </summary>
        public bool IsUserLoggedIn => CurrentUser != null;

        /// <summary>
        /// A greeting message for the logged-in user, displayed in the main window's header.
        /// </summary>
        public string LoggedInUserGreeting => IsUserLoggedIn ? $"Welcome, {CurrentUser.FullName} ({CurrentUser.RoleName})" : string.Empty;

        #endregion

        #region Commands
        public ICommand LogoutCommand { get; }
        #endregion

        #region Constructor

        public MainViewModel()
        {
            LogoutCommand = new RelayCommand(ExecuteLogout);

            // Start the application by showing the login view.
            ShowLoginView();
        }

        #endregion

        #region Private Methods

        private void ShowLoginView()
        {
            // If there's an old LoginViewModel, unsubscribe to prevent memory leaks.
            if (_loginViewModel != null)
            {
                _loginViewModel.LoginSuccess -= OnLoginSuccess;
            }

            // Create a new instance of the LoginViewModel.
            _loginViewModel = new LoginViewModel();
            // Subscribe to its LoginSuccess event. This is the key communication mechanism.
            _loginViewModel.LoginSuccess += OnLoginSuccess;
            // Set the current view to the login view.
            CurrentViewModel = _loginViewModel;
        }

        /// <summary>
        /// This method is the event handler that gets called when the LoginSuccess event is fired from the LoginViewModel.
        /// </summary>
        /// <param name="authenticatedUser">The User object passed from the LoginViewModel.</param>
        private void OnLoginSuccess(User authenticatedUser)
        {
            // 1. Set the current user for the application.
            CurrentUser = authenticatedUser;

            // 2. Create the DashboardViewModel, passing the authenticated user to it.
            //    This is crucial so the dashboard knows what data to load based on the user's role and department.
            var dashboardViewModel = new DashboardViewModel(CurrentUser);

            // 3. Switch the current view to the dashboard.
            CurrentViewModel = dashboardViewModel;
        }

        private void ExecuteLogout(object parameter)
        {
            // Clear the current user session.
            CurrentUser = null;

            // Go back to the login screen.
            ShowLoginView();
        }

        #endregion
    }
}
