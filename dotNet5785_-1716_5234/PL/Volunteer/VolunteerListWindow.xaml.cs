using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

        public IEnumerable<BO.VolunteerInList> VolunteerList
        {
            get { return (IEnumerable<BO.VolunteerInList>)GetValue(VolunteerListProperty); }
            set { SetValue(VolunteerListProperty, value); }
        }

        public static readonly DependencyProperty VolunteerListProperty =
            DependencyProperty.Register("VolunteerList", typeof(IEnumerable<BO.VolunteerInList>), typeof(VolunteerListWindow), new PropertyMetadata(null));

        // Filter variable
        private BO.VolunteerInListFields? VolunteerFilter { get; set; } = null;

        private void queryVolunteerList()
        {
            VolunteerList = s_bl?.Volunteer.ReadAll(null, VolunteerFilter)!;
        }

        private void volunteerListObserver()
            => queryVolunteerList();

        private void Window_Loaded(object sender, RoutedEventArgs e)
            => s_bl.Volunteer.AddObserver(volunteerListObserver);

        private void Window_Closed(object sender, EventArgs e)
            => s_bl.Volunteer.RemoveObserver(volunteerListObserver);

        private void dgVolunteerList_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            // Add logic for handling double-click on a row
        }

        public BO.VolunteerInList? SelectedVolunteer { get; set; }

        private void lsvVolunteerList_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (SelectedVolunteer != null)
                new VolunteerWindow(SelectedVolunteer.Id).Show();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            new VolunteerWindow().Show();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
                    }
                }
                catch (BO.BlDoesNotExistsException ex)
                {
                    MessageBox.Show($"Error: The requested item does not exist.\nDetails: {ex.Message}",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BO.BlCannotBeDeletedException ex)
                {
                    MessageBox.Show($"Error: The item cannot be deleted.\nDetails: {ex.Message}",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
}