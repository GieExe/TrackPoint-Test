//C:\Users\Joji\source\repos\TrackPoint-Test\TrackPoint-Test\ViewModels\ViewModelBase.cs
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TrackPoint_Test.ViewModels
{
    /// <summary>
    /// A base class for all ViewModels that implements the INotifyPropertyChanged interface.
    /// This allows for two-way data binding between the View (UI) and the ViewModel.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// The event that is fired when a property's value changes. The WPF binding system
        /// listens for this event to know when to update the UI.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// A helper method to raise the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.
        /// The [CallerMemberName] attribute automatically provides the name of the calling property,
        /// so you don't have to specify it manually.
        /// </param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Invoke the event, which notifies the UI.
            // The null-conditional operator (?.) ensures that the event is only raised if there are subscribers.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
