using BO;
using PL.Call;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PL.Volunteer
{
    public partial class HistoryWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        private int UserId { get; set; }

        public HistoryWindow(int userId)
        {
            InitializeComponent();
            UserId = userId;

        }
        public IEnumerable<BO.ClosedCallInList> ClosedCallInList
        {
            get { return (IEnumerable<BO.ClosedCallInList>)GetValue(ClosedCallInListProperty); }
            set { SetValue(ClosedCallInListProperty, value); }
        }

        public static readonly DependencyProperty ClosedCallInListProperty =
            DependencyProperty.Register("ClosedCallInList", typeof(IEnumerable<BO.ClosedCallInList>), typeof(HistoryWindow), new PropertyMetadata(null));

        private BO.CallType? _callFilter;
        public BO.CallType? CallFilter
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
        private BO.ClosedCallInListField? _callSort;
        public BO.ClosedCallInListField? CallSort
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


        private void queryCallList()
        {
            var filter = CallFilter != BO.CallType.None ? CallFilter : null;
            // אם CallSort הוא None, נשלח null
            var sorter = CallSort != BO.ClosedCallInListField.None ? CallSort : null;

            ClosedCallInList = s_bl.Call.ClosedCallsByVolunteer(UserId, CallFilter, CallSort);


        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            queryCallList(); // Refresh the list whenever the filter changes
        }
        private volatile DispatcherOperation? _observerOperation = null;

        // Observer method for volunteer list
        private void callListObserver()
        {
            if (_observerOperation is null || _observerOperation.Status == DispatcherOperationStatus.Completed)
                _observerOperation = Dispatcher.BeginInvoke(() =>
                {
                    queryCallList();
                });

        }
      
        


        private void Window_Closed(object sender, EventArgs e)
            => s_bl.Call.RemoveObserver(callListObserver);

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            queryCallList();
            s_bl.Volunteer.AddObserver(callListObserver);

        }


    }


}