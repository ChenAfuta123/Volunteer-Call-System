using BO;
using PL.Call;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
            DependencyProperty.Register("History", typeof(IEnumerable<BO.ClosedCallInList>), typeof(HistoryWindow), new PropertyMetadata(null));

        // משתנה לסינון
        private BO.ClosedCallInListField? sortCriteria { get; set; } = BO.ClosedCallInListField.None;
        public IEnumerable<BO.CallType> CallType
        {
            get { return (IEnumerable<BO.CallType>)GetValue(CallTypeProperty); }
            set { SetValue(CallTypeProperty, value); }
        }

        public static readonly DependencyProperty CallTypeProperty =
            DependencyProperty.Register("CallType", typeof(IEnumerable<BO.CallType>), typeof(HistoryWindow), new PropertyMetadata(null));


        // משתנה לסינון
        private BO.CallType? filterCriteria { get; set; } = BO.CallType.None;
        private void queryCallList()
        {
            if (filterCriteria == BO.CallType.None)
                filterCriteria = null;
            if (sortCriteria == BO.ClosedCallInListField.None)
                sortCriteria = null;
            var closedCalls = s_bl.Call.ClosedCallsByVolunteer(UserId, filterCriteria, sortCriteria);

            // הצגת הנתונים ב-DataGrid
            HistoryDataGrid.ItemsSource = closedCalls;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            queryCallList(); // Refresh the list whenever the filter changes
        }
        private void callListObserver()
           => queryCallList();


        private void Window_Closed(object sender, EventArgs e)
            => s_bl.Call.RemoveObserver(callListObserver);

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            queryCallList();
            s_bl.Volunteer.AddObserver(callListObserver);

        }

      
        //private void LoadClosedCalls(BO.CallType? filterCriteria = null, BO.ClosedCallInListField? sortCriteria = null)
        //{
        //    try
        //    {
        //        // קבלת כל הקריאות הסגורות של המתנדב מהשכבה הלוגית עם אפשרויות סינון ומיון
        //        var closedCalls = s_bl.Call.ClosedCallsByVolunteer(UserId, filterCriteria, sortCriteria);

        //        // הצגת הנתונים ב-DataGrid
        //        HistoryDataGrid.ItemsSource = closedCalls;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error loading closed calls: {ex.Message}");
        //    }
        //}

        //private void OnFilterChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var selectedFilter = (CallType)((ComboBoxItem)((ComboBox)sender).SelectedItem).Tag;
        //    LoadClosedCalls(selectedFilter, null);
        //}

        //private void OnSortChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var selectedSort = (ClosedCallInListField)((ComboBoxItem)((ComboBox)sender).SelectedItem).Tag;
        //    LoadClosedCalls(null, selectedSort);
        //}
    }


}