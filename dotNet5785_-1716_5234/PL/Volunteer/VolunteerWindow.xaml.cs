using BlApi;
using System;
using System.ComponentModel;
using System.Windows;

namespace PL.Volunteer
{
    /// <summary>
    /// Interaction logic for VolunteerWindow.xaml
    /// </summary>
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

        private string _buttonText = "Add"; // Default value
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
            DataContext = this; // Set DataContext for data binding

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
                        Active = true,
                        distanceType = BO.DistanceType.AirDistance,
                        TotalHandledCalls = 0,
                        TotalCanceledCalls = 0,
                        TotalExpiredCalls = 0,
                        VolunteerHandledCall = null
                    };
                    ButtonText = "Add"; // Set initial button text
                }
                else
                {
                    CurrentVolunteer = s_bl.Volunteer.Read(id);
                    ButtonText = "Update"; // Set button text for updates
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading volunteer data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_2(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
