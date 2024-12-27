using BO;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PL.Volunteer
{
    /// <summary>
    /// Interaction logic for VolunteerListWindow.xaml
    /// </summary>
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
            set
            {
                SetValue(VolunteerListProperty, value);

            }
        }

        public static readonly DependencyProperty VolunteerListProperty =
            DependencyProperty.Register("VolunteerList", typeof(IEnumerable<BO.VolunteerInList>), typeof(VolunteerListWindow), new PropertyMetadata(null));

        // Filter variable
        private BO.VolunteerInListFields? VolunteerSort { get; set; } = null;
        private BO.IsActiveFilter? VolunteerFilter { get; set; } = IsActiveFilter.None;


        // Query the volunteer list
        private void queryVolunteerList()
        {
            bool? isActiveFilter = VolunteerFilter switch
            {
                BO.IsActiveFilter.None => null,
                BO.IsActiveFilter.Active => true,
                BO.IsActiveFilter.Not_Active => false,
              _ => null // Handle any unexpected cases
            };
            VolunteerList = s_bl?.Volunteer.ReadAll(isActiveFilter, VolunteerSort)!;
           
        }

        // Observer method for volunteer list
        private void volunteerListObserver()
        {

            queryVolunteerList();
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

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle selection changed event if needed
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

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
             
        }
    }
}