using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PL.Volunteer
{
    public partial class VolunteerWindow : Window
    {
        static readonly IBl s_bl = Factory.Get();

        // Dependency Properties
        public static readonly DependencyProperty CurrentVolunteerProperty =
            DependencyProperty.Register(
                nameof(CurrentVolunteer),
                typeof(BO.Volunteer),
                typeof(VolunteerWindow),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register(
                nameof(ButtonText),
                typeof(string),
                typeof(VolunteerWindow),
                new PropertyMetadata("Add"));

        // Properties
        public BO.Volunteer CurrentVolunteer
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
                ButtonText = "Add";
            }
            else
            {
                try
                {
                    CurrentVolunteer = s_bl.Volunteer.Read(id);
                    ButtonText = "Update";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading volunteer data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
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
        private void VolunteerObserver()
        {
            int id = CurrentVolunteer.Id;
            CurrentVolunteer = null;
            CurrentVolunteer = s_bl.Volunteer.Read(id);
        }

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

        // Event handler for the Add/Update button
        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ButtonText == "Add")
                {
                    s_bl.Volunteer.Add(CurrentVolunteer);
                    MessageBox.Show("Volunteer added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (ButtonText == "Update")
                {
                    s_bl.Volunteer.Update(CurrentVolunteer.Id, CurrentVolunteer);
                    MessageBox.Show("Volunteer updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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

        // Event handler for password changes
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is VolunteerWindow window)
            {
                window.CurrentVolunteer.Password = ((PasswordBox)sender).Password;
            }
        }

      
    }
}
