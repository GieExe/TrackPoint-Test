//C:\Users\Joji\source\repos\TrackPoint-Test\TrackPoint-Test\ViewModels\RelayCommand.cs
using System;
using System.Windows.Input;

namespace TrackPoint_Test.ViewModels
{
    /// <summary>
    /// A standard, reusable implementation of the ICommand interface.
    /// This class allows you to delegate the command's logic to methods (actions) 
    /// and functions (predicates) in your ViewModel.
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Fields

        // The action to execute when the command is invoked.
        private readonly Action<object> _execute;

        // A function that determines if the command can be executed. Can be null.
        private readonly Predicate<object> _canExecute;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the RelayCommand class that can always be executed.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region ICommand Members

        /// <summary>
        /// The event that is raised when the result of CanExecute has changed.
        /// WPF's CommandManager handles hooking this up to UI events.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion
    }
}
