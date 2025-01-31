using PL.Call;
using System.Windows;
using System.Windows.Threading;

namespace PL.Volunteer
{
    public partial class PersonalVolunteerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        private int UserId { get; set; }

        public static readonly DependencyProperty IsCallAnableToSelectProperty =
         DependencyProperty.Register("IsCallAnableToSelect", typeof(bool),typeof(PersonalVolunteerWindow), new PropertyMetadata(false));

       

        public static readonly DependencyProperty IsCallInProgressProperty =
            DependencyProperty.Register("IsCallInProgress", typeof(bool), typeof(PersonalVolunteerWindow), new PropertyMetadata(false));

        public static readonly DependencyProperty CurrentCallProperty =
         DependencyProperty.Register(
             nameof(CurrentCall),
             typeof(BO.Call),
             typeof(PersonalVolunteerWindow),
             new PropertyMetadata(null));

        public static readonly DependencyProperty CallDistanceFromVolunteerProperty =
     DependencyProperty.Register(
         nameof(CallDistanceFromVolunteer),
         typeof(double),
         typeof(PersonalVolunteerWindow),
         new PropertyMetadata(null));


        public bool IsCallAnableToSelect
        {
            get { return (bool)GetValue(IsCallAnableToSelectProperty); }
            set { SetValue(IsCallAnableToSelectProperty, value); }
        }
        public bool IsCallInProgress
        {
            get { return (bool)GetValue(IsCallInProgressProperty); }
            set { SetValue(IsCallInProgressProperty, value); }

        }
        public BO.Call? CurrentCall
        {
            get => (BO.Call)GetValue(CurrentCallProperty);
            set => SetValue(CurrentCallProperty, value);
        }



        public double? CallDistanceFromVolunteer
        {
            get => (double)GetValue(CallDistanceFromVolunteerProperty);
            set => SetValue(CallDistanceFromVolunteerProperty, value);
        }

        public PersonalVolunteerWindow(int userId)
        {
            InitializeComponent();
            UserId = userId;
            CallDistanceFromVolunteer = s_bl.Volunteer.CallDistanceFromvolunteer(userId);
            CurrentCall = new BO.Call
                {
                    Id = 0,
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

            CallDistanceFromVolunteer = s_bl.Volunteer.CallDistanceFromvolunteer(userId);

            try
            {
                try
                {
                    var volunteer = s_bl.Volunteer.Read(UserId);
                    var volunteerHandledCall = volunteer.VolunteerHandledCall;
                    if (volunteerHandledCall == null)
                    {
                        if (volunteer.Active)
                            IsCallAnableToSelect = true;
                    }
                    else
                    {
                        IsCallInProgress = true;
                        int callid = volunteerHandledCall.CallId;
                        CurrentCall = s_bl.Call.Read(callid);


                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading call data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading call data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }
        private void UpdateCallStatus()
        {
            try
            {
                var volunteer = s_bl.Volunteer.Read(UserId);
                var volunteerHandledCall = volunteer.VolunteerHandledCall;

                if (volunteerHandledCall == null)
                {
                    IsCallInProgress = false;
                    //CurrentCall = null;
                    IsCallAnableToSelect = volunteer.Active;
                    CallDistanceFromVolunteer = null;
                }
                else
                {
                    IsCallAnableToSelect = false;
                    IsCallInProgress = true;
                    int callId = volunteerHandledCall.CallId;
                    CurrentCall = s_bl.Call.Read(callId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating call status: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private volatile DispatcherOperation? _observerOperation = null;

        // Observer method for volunteer list
        private void CallObserver()
        {
            if (_observerOperation is null || _observerOperation.Status == DispatcherOperationStatus.Completed)
                _observerOperation = Dispatcher.BeginInvoke(() =>
                {
                    int id = CurrentCall!.Id;
                    CurrentCall = s_bl.Call.Read(id);
                    CallDistanceFromVolunteer = s_bl.Volunteer.CallDistanceFromvolunteer(UserId);
                });

        }

        // הצטרפות לאירוע טעינת המסך
        private void OnScreenLoaded(object sender, EventArgs e)
        {
            if (CurrentCall!.Id != -1)
            {
                s_bl.Call.AddObserver(CurrentCall!.Id, CallObserver);
            }
        }

        // הצטרפות לאירוע סגירת המסך
        private void OnScreenClosed(object sender, EventArgs e)
        {
            if (CurrentCall!.Id != -1 )
            
                s_bl.Call.RemoveObserver(CurrentCall!.Id, CallObserver);
            
        }
        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            var history = new HistoryWindow(UserId);
            history.Show();
        }

        private void VolunteerButton_Click(object sender, RoutedEventArgs e)
        {
            var volunteerWindow = new VolunteerWindow(UserId);
            volunteerWindow.Show();

        }

        // עדכון פרטי קריאה
      
        private void ButtonCallSelection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var callSelectionWindow = new CallSelectionWindow(UserId);
                  callSelectionWindow.ShowDialog();
                  UpdateCallStatus();

            }
            catch (BO.BlNullPropertyException ex)
            {
                MessageBox.Show($"Error: The requested item does not exist.\nDetails: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void FinishCallButton_Click(object sender, RoutedEventArgs e)
        {

            try

            {
               
                
                    var VolunteerHandledCall = s_bl.Volunteer.Read(UserId).VolunteerHandledCall;
                   
                    
                        int callid = s_bl.Volunteer.Read(UserId).VolunteerHandledCall!.CallId;

                        if (MessageBox.Show("Are you sure you want to finish call treatment?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            s_bl.Call.EndOftreatmentUpdate(UserId, s_bl.Call.findAssignment(callid, UserId));
                            UpdateCallStatus();
                        }
                    
                
            }
            catch (BO.BlValidationException ex)
            {
                MessageBox.Show($"Error:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
           
        }

        private void CancelCallButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int callid = s_bl.Volunteer.Read(UserId).VolunteerHandledCall!.CallId;

                if (MessageBox.Show("Are you sure you want to cancle call treatment?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    s_bl.Call.CanceltreatmentUpdate(UserId, s_bl.Call.findAssignment(callid, UserId));
                    UpdateCallStatus();
                }
            }
            catch (BO.BlValidationException ex)
            {
                MessageBox.Show($"Error:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
           
        }
    }
}