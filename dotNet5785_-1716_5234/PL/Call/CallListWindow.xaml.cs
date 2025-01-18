using PL.Volunteer;
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

namespace PL.Call
{
    /// <summary>
    /// Interaction logic for CallListWindow.xaml
    /// </summary>
    public partial class CallListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();


        public CallListWindow(BO.CallInListField? callFilter = BO.CallInListField.None,
        object? customFilter = null, BO.CallInListField? callSort = BO.CallInListField.None)
        {
            InitializeComponent();

            _callFilter = callFilter;
            _obj = customFilter;
            _callSort = callSort;


            // קריאה לעדכון הרשימה לפי הפילטרים
            queryCallList();
        }

        public IEnumerable<BO.CallInList> CallList
        {
            get { return (IEnumerable<BO.CallInList>)GetValue(CallListProperty); }
            set { SetValue(CallListProperty, value); }
        }

        public static readonly DependencyProperty CallListProperty =
            DependencyProperty.Register("CallList", typeof(IEnumerable<BO.CallInList>), typeof(CallListWindow), new PropertyMetadata(null));

        private BO.CallInListField? _callFilter;
        public BO.CallInListField? CallFilter
        {
            get => _callFilter;
            set
            {
                if (_callFilter != value)
                {
                    _callFilter = value;
                    queryCallList(); // Refresh the list based on the new filter
                }
            }
        }
        private BO.CallInListField? _callSort;
        public BO.CallInListField? CallSort
        {
            get => _callSort;
            set
            {
                if (_callSort != value)
                {
                    _callSort = value;
                    queryCallList(); // Refresh the list based on the new sort
                }
            }
        }
        private object? _obj;
        public object? CustomFilter
        {
            get => _obj;
            set
            {
                if (_obj != value)
                {
                    _obj = value;
                    queryCallList(); // Refresh the list based on the new sort
                }
            }
        }

        private void CustomFilterTextBox_TextChanged(object sender, SelectionChangedEventArgs e)
        {
            queryCallList(); // Refresh the list whenever the filter changes
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            queryCallList(); // Refresh the list whenever the filter changes
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            queryCallList(); // רענון הרשימה כאשר המשתמש לוחץ על "חפש"
        }
        private void queryCallList()
        {
            // טיפול בערכים ריקים: 
            // אם CallFilter הוא None, נשלח null
            var filter = CallFilter != BO.CallInListField.None ? CallFilter : null;

            // אם CustomFilter ריק או null, נשלח null
            var customFilter = !string.IsNullOrEmpty(CustomFilter?.ToString()) ? CustomFilter : null;

            // אם CallSort הוא None, נשלח null
            var sorter = CallSort != BO.CallInListField.None ? CallSort : null;

            // קריאה ל-ReadAll עם הערכים המתוקנים
            CallList = s_bl?.Call.ReadAll(filter, customFilter, sorter) ?? Enumerable.Empty<BO.CallInList>();
        }

        private void callListObserver()
            => queryCallList();


        private void Window_Closed(object sender, EventArgs e)
            => s_bl.Call.RemoveObserver(callListObserver);

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            queryCallList();
            s_bl.Call.AddObserver(callListObserver);

        }

        public BO.CallInList? SelectedCall { get; set; }
        private void dgCallList_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (SelectedCall != null)
                new CallWindow(SelectedCall.CallId).Show();

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var callWindow = new CallWindow();
            if (callWindow.ShowDialog() == true)
                queryCallList();

        }


        private void DataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete call?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                var button = sender as FrameworkElement;
                var callToDelete = button?.DataContext as BO.Call;
                try
                {
                    if (callToDelete != null)
                    {
                        s_bl.Call.Delete(callToDelete.Id);

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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancle the treatment for this call?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                var button = sender as FrameworkElement;
                //var callToCancel = button?.DataContext as BO.Call;
                var callToCancel2 = button?.DataContext as BO.CallInList;
                //int managerID = s_bl.Volunteer.ManagerID();

                if (callToCancel2 != null)
                {
                    try
                    {

                        var volunteer = s_bl.Volunteer.Read(callToCancel2!.Id!.Value);
                        var assignmentID = s_bl.Call.findAssignment(callToCancel2!.CallId, volunteer.Id);

                        if (callToCancel2 != null)
                        {
                            s_bl.Call.CanceltreatmentUpdate(200123456, assignmentID);
                        }

                        // Send email notification
                        string subject = "Your assignment has been cancelled";
                        string body = "<h1>Dear Volunteer,</h1><p>We appreciate your help and support!</p>";
                        List<string> email = new List<string> { volunteer.Email };

                        // Assuming a method SendEmail exists in your BL
                        s_bl.Volunteer.SendEmailToVolunteers(email, subject, body);
                    }
                    catch (BO.BlDoesNotExistsException ex)
                    {
                        MessageBox.Show($"Error:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (BO.BlValidationException ex)
                    {
                        MessageBox.Show($"Error:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    catch (BO.BlUnauthorizedException ex)
                    {
                        MessageBox.Show($"Error:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Manager ID is invalid or call not selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

    }
}