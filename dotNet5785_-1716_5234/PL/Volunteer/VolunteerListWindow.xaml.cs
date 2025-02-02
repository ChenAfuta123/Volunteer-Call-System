using BO;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PL.Volunteer
{
    public partial class VolunteerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public VolunteerListWindow()
        {
            InitializeComponent();
        }

        // Property to bind Volunteer List
        public IEnumerable<BO.VolunteerInList> VolunteerList
        {
            get { return (IEnumerable<BO.VolunteerInList>)GetValue(VolunteerListProperty); }
            set { SetValue(VolunteerListProperty, value); }
        }

        public static readonly DependencyProperty VolunteerListProperty =
            DependencyProperty.Register("VolunteerList", typeof(IEnumerable<BO.VolunteerInList>), typeof(VolunteerListWindow), new PropertyMetadata(null));

        private BO.IsActiveFilter? _volunteerFilter = BO.IsActiveFilter.None;
        public BO.IsActiveFilter? VolunteerFilter
        {
            get => _volunteerFilter;
            set
            {
                if (_volunteerFilter != value)
                {
                    _volunteerFilter = value;
                    queryVolunteerList();
                }
            }
        }

        private BO.VolunteerInListFields? _volunteerSort;
        public BO.VolunteerInListFields? VolunteerSort
        {
            get => _volunteerSort;
            set
            {
                if (_volunteerSort != value)
                {
                    _volunteerSort = value;
                    queryVolunteerList();
                }
            }
        }

        // Query the volunteer list
        private void queryVolunteerList()
        {
            bool? isActiveFilter = VolunteerFilter switch
            {
                BO.IsActiveFilter.None => null,
                BO.IsActiveFilter.Active => true,
                BO.IsActiveFilter.Not_Active => false,
                _ => null
            };
            VolunteerList = s_bl?.Volunteer.ReadAll(isActiveFilter, VolunteerSort)!;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            queryVolunteerList();
        }
        private volatile DispatcherOperation? _observerOperation = null;
      
        // Observer method for volunteer list
        private void volunteerListObserver()
        {
            if (_observerOperation is null || _observerOperation.Status == DispatcherOperationStatus.Completed)
                _observerOperation = Dispatcher.BeginInvoke(() =>
                {
                    queryVolunteerList();
                });

        }

        // Register the observer on Window Loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            queryVolunteerList();
            s_bl.Volunteer.AddObserver(volunteerListObserver);
        }

        // Remove the observer on Window Closed
        private void Window_Closed(object sender, EventArgs e)
        {
            s_bl.Volunteer.RemoveObserver(volunteerListObserver);
        }

        private void dgVolunteerList_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (SelectedVolunteer != null)
                new VolunteerWindow(SelectedVolunteer.Id).Show();
            queryVolunteerList();
        }

        public BO.VolunteerInList? SelectedVolunteer { get; set; }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var volunteerWindow = new VolunteerWindow();
            if (volunteerWindow.ShowDialog() == true)
                queryVolunteerList(); // Refresh the list to include the new volunteer
        }

        private void ComboBox_FilterSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            queryVolunteerList(); // Refresh the list whenever the filter changes
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this volunteer?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                var button = sender as FrameworkElement;
                var volunteerToDelete = button?.DataContext as BO.VolunteerInList;
                try
                {
                    if (volunteerToDelete != null)
                    {
                        s_bl.Volunteer.Delete(volunteerToDelete.Id);
                        queryVolunteerList();
                    }
                }
                catch (BO.BlDoesNotExistsException ex)
                {
                    MessageBox.Show($"Error: The requested item does not exist.\nDetails: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BO.BlCannotBeDeletedException ex)
                {
                    MessageBox.Show($"Error: The item cannot be deleted.\nDetails: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}