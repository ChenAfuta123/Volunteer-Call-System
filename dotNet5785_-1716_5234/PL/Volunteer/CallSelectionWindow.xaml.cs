
using BO;
using PL.Call;
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
    /// Interaction logic for CallSelectionWindow.xaml
    /// </summary>
    public partial class CallSelectionWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        private int UserId { get; set; }
        public CallSelectionWindow(int userId)
        {
            InitializeComponent();
            UserId = userId;

        }

        public IEnumerable<BO.OpenCallInList> OpenCallList
        {
            get { return (IEnumerable<BO.OpenCallInList>)GetValue(OpenCallListProperty); }
            set { SetValue(OpenCallListProperty, value); }
        }

        public static readonly DependencyProperty OpenCallListProperty =
            DependencyProperty.Register("OpenCallList", typeof(IEnumerable<BO.OpenCallInList>), typeof(CallSelectionWindow), new PropertyMetadata(null));

        // משתנה לסינון
        private BO.OpenCallInListField? callFilter { get; set; } = BO.OpenCallInListField.None;
        public BO.OpenCallInListField? CallFilter
        {
            get => callFilter;
            set
            {
                if (callFilter != value)
                {
                    callFilter = value;
                    /*  OnPropertyChanged();*/ // Notify the UI about the change
                    queryOpenCallList(); // Refresh the list based on the new sort
                }
            }
        }
        private BO.OpenCallInListField? callSorter { get; set; } = BO.OpenCallInListField.None;
        public BO.OpenCallInListField? CallSorter
        {
            get => callSorter;
            set
            {
                if (callSorter != value)
                {
                    callSorter = value;
                    /*  OnPropertyChanged();*/ // Notify the UI about the change
                    queryOpenCallList(); // Refresh the list based on the new sort
                }
            }
        }

        private void queryOpenCallList()
        {
            OpenCallList = (CallFilter == BO.OpenCallInListField.None)
                ? s_bl?.Call.ReadAll(null, null, null, UserId)!
                : s_bl?.Call.ReadAll(CallFilter, null, CallSorter, UserId)!;
        }

        private void callListObserver()
            => queryOpenCallList();

        private void Window_Loaded(object sender, RoutedEventArgs e)
            => s_bl.Call.AddObserver(callListObserver);

        private void Window_Closed(object sender, EventArgs e)
            => s_bl.Call.RemoveObserver(callListObserver);

        private void dgCallList_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            // הוסף את הלוגיקה לטיפול בלחיצה כפולה על רשומה
        }



        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            queryOpenCallList(); // Refresh the list whenever a filter or sort option changes
        }

        private void DataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
        private void TreatmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to treat call?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                var button = sender as FrameworkElement;
                var callToTreat = button?.DataContext as BO.Call;
                try
                {
                    if (callToTreat != null)
                    {
                        s_bl.Call.ChooseCallForTreatment(UserId, callToTreat.Id);

                    }
                }
                catch (BO.BlDoesNotExistsException ex)
                {
                    MessageBox.Show($"Error: The requested item does not exist.\nDetails: {ex.Message}",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BO.BlValidationException ex)
                {
                    MessageBox.Show($"Error: The requested item does not exist.\nDetails: {ex.Message}",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }
        private void ChangeAddressButton_Click(object sender, RoutedEventArgs e)
        {
            //if (MessageBox.Show("Are you sure you want to treat call?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            //{
            //    var button = sender as FrameworkElement;
            //    var callToTreat = button?.DataContext as BO.Call;
            //    try
            //    {
            //        if (callToTreat != null)
            //        {
            //            s_bl.Call.ChooseCallForTreatment(UserId, callToTreat.Id);

            //        }
            //    }
            //    catch (BO.BlDoesNotExistsException ex)
            //    {
            //        MessageBox.Show($"Error: The requested item does not exist.\nDetails: {ex.Message}",
            //            "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
            //    catch (BO.BlValidationException ex)
            //    {
            //        MessageBox.Show($"Error: The requested item does not exist.\nDetails: {ex.Message}",
            //            "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }

            //}
        }
        private void DescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            //if (MessageBox.Show("Are you sure you want to treat call?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            //{
            //    var button = sender as FrameworkElement;
            //    var callToTreat = button?.DataContext as BO.Call;
            //    try
            //    {
            //        if (callToTreat != null)
            //        {
            //            s_bl.Call.ChooseCallForTreatment(UserId, callToTreat.Id);

            //        }
            //    }
            //    catch (BO.BlDoesNotExistsException ex)
            //    {
            //        MessageBox.Show($"Error: The requested item does not exist.\nDetails: {ex.Message}",
            //            "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
            //    catch (BO.BlValidationException ex)
            //    {
            //        MessageBox.Show($"Error: The requested item does not exist.\nDetails: {ex.Message}",
            //            "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }

            //}
        }
    }
}
