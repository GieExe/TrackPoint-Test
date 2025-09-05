using System.Windows;
using TrackPoint_Test.ViewModels;

namespace TrackPoint_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // The MainViewModel is the brain of the application.
            // It controls the application's overall state, including which view is
            // currently displayed (Login or Dashboard).
            // By setting it as the DataContext, all the bindings in MainWindow.xaml
            // will connect to the properties and commands in MainViewModel.
            this.DataContext = new MainViewModel();
        }
    }
}