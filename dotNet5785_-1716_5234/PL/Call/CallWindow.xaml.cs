using BlApi;
using System;
using System.ComponentModel;
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
    /// Interaction logic for CallWindow.xaml
    /// </summary>
    public partial class CallWindow : Window, INotifyPropertyChanged
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
      
        private BO.Call? _currentCall;
        public BO.Call? CurrentCall
        {
            get { return _currentCall; }
            set
            {
                _currentCall = value;
                OnPropertyChanged(nameof(CurrentCall));
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

        public static readonly DependencyProperty CurrentCallProperty =
            DependencyProperty.Register("CurrentCall", typeof(BO.Call), typeof(CallWindow), new PropertyMetadata(null));

        public IEnumerable<BO.CallType> CallTypeCollection { get; set; }
        public IEnumerable<BO.CallStatus> StatusCollection { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CallWindow(int id = 0)
        {
            InitializeComponent();
            DataContext = this; // Set DataContext for data binding


            StatusCollection = Enum.GetValues(typeof(BO.CallStatus)).Cast<BO.CallStatus>();
            CallTypeCollection = Enum.GetValues(typeof(BO.CallType)).Cast<BO.CallType>();

            try
            {
                if (id == 0)
                {
                    CurrentCall = new BO.Call
                    {
                        Id =0 ,
                        callType = BO.CallType.None,
                        Description = null,
                        Address = "",
                        Latitude = 0.0,
                        Longitude = 0.0,
                        OpeningTime = DateTime.Now,
                        MaxEndingTime = null,
                        callStatus = BO.CallStatus.Open,
                        CallAssignList = null
                    };
                    ButtonText = "Add"; // Set initial button text
                }
                else
                {
                    CurrentCall = s_bl.Call.Read(id);
                    ButtonText = "Update"; // Set button text for updates
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading call data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void RefreshCall()
        {
            int id = CurrentCall!.Id;
            CurrentCall = null;
            CurrentCall = s_bl.Call.Read(id);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            if (CurrentCall.Id != 0)
            {
                s_bl.Call.AddObserver(CurrentCall.Id, RefreshCall);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (CurrentCall != null && CurrentCall.Id != 0)
            {
                s_bl.Call.RemoveObserver(CurrentCall.Id, RefreshCall);
            }
        }

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ButtonText == "Add")
                {
                    s_bl.Call.Add(CurrentCall!);
                    MessageBox.Show("Call added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (ButtonText == "Update")
                {
                    s_bl.Call.Update(CurrentCall!);
                    MessageBox.Show("Call updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
     


    }
}
