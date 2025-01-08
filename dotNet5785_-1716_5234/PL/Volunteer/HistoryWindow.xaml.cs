using BO;
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
            LoadClosedCalls();
        }

        private void LoadClosedCalls(BO.CallType? filterCriteria = null, BO.ClosedCallInListField? sortCriteria = null)
        {
            try
            {
                // קבלת כל הקריאות הסגורות של המתנדב מהשכבה הלוגית עם אפשרויות סינון ומיון
                var closedCalls = s_bl.Call.ClosedCallsByVolunteer(UserId, filterCriteria, sortCriteria);

                // הצגת הנתונים ב-DataGrid
                HistoryDataGrid.ItemsSource = closedCalls;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading closed calls: {ex.Message}");
            }
        }

        private void OnFilterChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedFilter = (CallType)((ComboBoxItem)((ComboBox)sender).SelectedItem).Tag;
            LoadClosedCalls(selectedFilter, null);
        }

        private void OnSortChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSort = (ClosedCallInListField)((ComboBoxItem)((ComboBox)sender).SelectedItem).Tag;
            LoadClosedCalls(null, selectedSort);
        }
    }

   
}
