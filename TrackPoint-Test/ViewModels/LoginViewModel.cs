//C:\Users\Joji\source\repos\TrackPoint-Test\TrackPoint-Test\ViewModels\LoginViewModel.cs
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TrackPoint_Test.DataAccess;
using TrackPoint_Test.Models;

namespace TrackPoint_Test.ViewModels
{
    /// <summary>
    /// Provides the logic and data for the LoginView.
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        #region Fields
        private string _username;
        private string _errorMessage;
        private Visibility _errorVisibility = Visibility.Collapsed;
        #endregion

        #region Properties

        /// <summary>
        /// The username entered by the user.
        /// </summary>
        public string Username
        {
            get => _username;
            set
            {
                if (_username == value) return;
                _username = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The error message to display if login fails.
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage == value) return;
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Controls the visibility of the error message TextBlock in the View.
        /// </summary>
        public Visibility ErrorVisibility
        {
            get => _errorVisibility;
            set
            {
                if (_errorVisibility == value) return;
                _errorVisibility = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The command to execute when the user clicks the Login button.
        /// </summary>
        public ICommand LoginCommand { get; }

        #endregion

        #region Events

        /// <summary>
        /// This event is raised when a login attempt is successful.
        /// The MainViewModel will listen for this event to switch to the dashboard view.
        /// </summary>
        public event Action<User> LoginSuccess;

        #endregion

        #region Constructor

        public LoginViewModel()
        {
            // The command takes the ExecuteLogin method as its action.
            // We pass null for the CanExecute predicate, as we'll handle it inside the XAML trigger for simplicity,
            // or you could implement a CanExecute method that checks if username/password are filled.
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// The logic that runs when the LoginCommand is executed.
        /// </summary>
        /// <param name="parameter">The command parameter, which we expect to be the PasswordBox from the View.</param>
        private void ExecuteLogin(object parameter)
        {
            // Hide any previous error messages
            ErrorVisibility = Visibility.Collapsed;

            // The parameter from the View is the PasswordBox control itself.
            if (parameter is PasswordBox passwordBox)
            {
                string password = passwordBox.Password;

                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(password))
                {
                    ShowError("Username and password cannot be empty.");
                    return;
                }

                try
                {
                    // Call the database helper to validate the user
                    User user = DatabaseHelper.ValidateUser(Username, password);

                    if (user != null)
                    {
                        // User is valid. Raise the LoginSuccess event and pass the user object.
                        // The MainViewModel will catch this and switch the current view.
                        LoginSuccess?.Invoke(user);
                    }
                    else
                    {
                        // User is invalid.
                        ShowError("Invalid username or password. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    // Handle potential database connection errors
                    ShowError($"A database error occurred: {ex.Message}");
                }
            }
        }

        private void ShowError(string message)
        {
            ErrorMessage = message;
            ErrorVisibility = Visibility.Visible;
        }

        #endregion
    }
}
