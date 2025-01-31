using BlApi;
using BO;
using PL.Call;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PL.Volunteer
{
    public partial class VolunteerWindow : Window
    {
        static readonly IBl s_bl = Factory.Get();



        public static readonly DependencyProperty CurrentVolunteerProperty =
          DependencyProperty.Register("CurrentVolunteer", typeof(BO.Volunteer), typeof(VolunteerWindow), new PropertyMetadata(null));

        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register(
                nameof(ButtonText),
                typeof(string),
                typeof(VolunteerWindow),
                new PropertyMetadata("הוספה"));

        // Properties
        public BO.Volunteer? CurrentVolunteer
        {
            get => (BO.Volunteer)GetValue(CurrentVolunteerProperty);
            set => SetValue(CurrentVolunteerProperty, value);
        }

        public string ButtonText
        {
            get => (string)GetValue(ButtonTextProperty);
            set => SetValue(ButtonTextProperty, value);
        }

        //public IEnumerable<BO.Role> RolesCollection { get; set; }
        //public IEnumerable<BO.DistanceType> DistanceTypesCollection { get; set; }

        public VolunteerWindow(int id = 0)
        {
            InitializeComponent();


            // Initialize the CurrentVolunteer property and ButtonText based on the id
            if (id == 0)
            {
                CurrentVolunteer = new BO.Volunteer
                {
                    Id = 0,
                    role = BO.Role.volunteer,
                    Name = string.Empty,
                    PhoneNumber = string.Empty,
                    Email = string.Empty,
                    Address = string.Empty,
                    Latitude = null,
                    Longitude = null,
                    MaxDistance = null,
                    Password = null,
                    Active = true,
                    distanceType = BO.DistanceType.AirDistance,
                    TotalHandledCalls = 0,
                    TotalCanceledCalls = 0,
                    TotalExpiredCalls = 0,
                    VolunteerHandledCall = null
                };
                ButtonText = "הוספה";
            }
            else
            {
                try
                {
                    CurrentVolunteer = s_bl.Volunteer.Read(id);
                    ButtonText = "עדכון";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading volunteer data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



        private void queryVolunteer()
        {
            var updatedVolunteer = s_bl.Volunteer.Read(CurrentVolunteer!.Id);
            CurrentVolunteer = updatedVolunteer;  // עדכון ה-CurrentCall עם המידע החדש
        }


        // Event handler for the Add/Update button
        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (ButtonText == "הוספה")
                {
                    int newVolunteerId = CurrentVolunteer!.Id;
                    s_bl.Volunteer.Add(CurrentVolunteer!);
                    MessageBox.Show("Volunteer added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    queryVolunteer();
                }
                else if (ButtonText == "עדכון")
                {

                    s_bl.Volunteer.Update(CurrentVolunteer!.Id, CurrentVolunteer);
                    MessageBox.Show("Volunteer updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    queryVolunteer();
                }

                Close();
            }
            catch (BO.BlDoesNotExistsException ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private volatile DispatcherOperation? _observerOperation = null;

        // Observer method for volunteer list
        private void VolunteerObserver()
        {
            if (_observerOperation is null || _observerOperation.Status == DispatcherOperationStatus.Completed)
                _observerOperation = Dispatcher.BeginInvoke(() =>
                {
                    queryVolunteer();
                });

        }
        private void LoadScreen()
        {
            OnScreenLoaded(this, EventArgs.Empty);
        }

        // שיטה זו יכולה להיות מופעלת בזמן סגירת המסך
        private void CloseScreen()
        {
            OnScreenClosed(this, EventArgs.Empty);
        }
        // Method for observing and updating the volunteer data

        // Register the observer when the window is loaded
        private void OnScreenLoaded(object sender, EventArgs e)
        {
            if (CurrentVolunteer != null && CurrentVolunteer.Id != 0)
            {
                s_bl.Volunteer.AddObserver(CurrentVolunteer.Id, VolunteerObserver);
            }

        }

        // Remove the observer when the window is closed
        private void OnScreenClosed(object sender, EventArgs e)
        {
            if (CurrentVolunteer != null && CurrentVolunteer.Id != 0)
            {
                s_bl.Volunteer.RemoveObserver(CurrentVolunteer.Id, VolunteerObserver);
            }
        }



        // Event handler for password changes
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is VolunteerWindow window)
            {
                window.CurrentVolunteer!.Password = ((PasswordBox)sender).Password;
            }
        }


    }
}