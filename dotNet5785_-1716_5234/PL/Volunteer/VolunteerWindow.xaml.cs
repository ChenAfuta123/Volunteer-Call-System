using BlApi;
using System;
using System.ComponentModel;
using System.Windows;

namespace PL.Volunteer
{
    public partial class VolunteerWindow : Window, INotifyPropertyChanged
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        private BO.Volunteer? _currentVolunteer;
        public BO.Volunteer? CurrentVolunteer
        {
            get { return _currentVolunteer; }
            set
            {
                _currentVolunteer = value;
                OnPropertyChanged(nameof(CurrentVolunteer));
            }
        }
       
        private string _buttonText = "Add";
        public string ButtonText
        {
            get => _buttonText;
            set
            {
                _buttonText = value;
                OnPropertyChanged(nameof(ButtonText));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public VolunteerWindow(int id = 0)
        {
            InitializeComponent();
            DataContext = this;

            try
            {
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
                    CurrentVolunteer = s_bl.Volunteer.Read(id);
                    ButtonText = "Update";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading volunteer data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshVolunteer()
        {
            int id = CurrentVolunteer!.Id;
            CurrentVolunteer = null;
            CurrentVolunteer = s_bl.Volunteer.Read(id);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            if (CurrentVolunteer!.Id != 0)
            {
                s_bl.Volunteer.AddObserver(CurrentVolunteer.Id, RefreshVolunteer);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (CurrentVolunteer != null && CurrentVolunteer.Id != 0)
            {
                s_bl.Volunteer.RemoveObserver(CurrentVolunteer.Id, RefreshVolunteer);
            }
        }

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ButtonText == "Add")
                {
                    s_bl.Volunteer.Add(CurrentVolunteer!);
                    MessageBox.Show("Volunteer added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                else if (ButtonText == "Update")
                {
                    s_bl.Volunteer.Update(CurrentVolunteer!.Id, CurrentVolunteer);
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

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is VolunteerWindow window)
            {
                window.CurrentVolunteer!.Password = PasswordBox.Password;
            }
        }
    }
}
