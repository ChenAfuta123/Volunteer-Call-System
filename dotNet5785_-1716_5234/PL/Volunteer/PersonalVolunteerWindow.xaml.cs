using System.Windows;

namespace PL.Volunteer
{
    public partial class PersonalVolunteerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        private int UserId { get; set; }

        public PersonalVolunteerWindow(int userId)
        {
            InitializeComponent();
            UserId = userId;
            LoadCallDetails(); // טוען את פרטי הקריאה בעת פתיחת החלון
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

        private void LoadCallDetails()
        {
            try
            {
                var volunteer = s_bl.Volunteer.Read(UserId);

                if (volunteer.VolunteerHandledCall == null)
                {
                    HideCallDetails();  // הסתרת פרטי הקריאה כאשר אין קריאה בטיפול
                    if (volunteer.Active)
                        CallSelectionButton.IsEnabled = true; // מאפשר את כפתור בחירת קריאה
                    else
                        CallSelectionButton.IsEnabled = false;
                }
                else
                {
                    var callid = volunteer.VolunteerHandledCall.CallId;
                    var call = s_bl.Call.Read(callid);
                    ShowCallDetails(call);  // הצגת פרטי הקריאה כאשר יש קריאה בטיפול
                    CallSelectionButton.IsEnabled = false; // ביטול אפשרות בחירת קריאה
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading call details: {ex.Message}");
            }
        }

        private void HideCallDetails()
        {
            // הסתרת כל תיבות הטקסט של פרטי הקריאה
            CallIdTextBox.Visibility = Visibility.Hidden;
            CallStatusTextBox.Visibility = Visibility.Hidden;
            CallTypeTextBox.Visibility = Visibility.Hidden;
            CallDistanceTextBox.Visibility = Visibility.Hidden;
            CallAddressTextBox.Visibility = Visibility.Hidden;
            CallOpeningTimeTextBox.Visibility = Visibility.Hidden;
            CallMaxEndingTimeTextBox.Visibility = Visibility.Hidden;
        }

        private void ShowCallDetails(BO.Call call)
        {
            // הצגת פרטי הקריאה
            CallIdTextBox.Text = call.Id.ToString();
            CallStatusTextBox.Text = call.callStatus.ToString();
            CallTypeTextBox.Text = call.callType.ToString();
            CallDistanceTextBox.Text = s_bl.Volunteer.Read(UserId).VolunteerHandledCall!.CallDistanceFromVolunteer.ToString("F2");
            CallAddressTextBox.Text = call.Address;
            CallOpeningTimeTextBox.Text = call.OpeningTime.ToString("g");
            CallMaxEndingTimeTextBox.Text = call.MaxEndingTime?.ToString("g") ?? "לא מוגדר";

            // הצגת כל תיבות הטקסט של פרטי הקריאה
            CallIdTextBox.Visibility = Visibility.Visible;
            CallStatusTextBox.Visibility = Visibility.Visible;
            CallTypeTextBox.Visibility = Visibility.Visible;
            CallDistanceTextBox.Visibility = Visibility.Visible;
            CallAddressTextBox.Visibility = Visibility.Visible;
            CallOpeningTimeTextBox.Visibility = Visibility.Visible;
            CallMaxEndingTimeTextBox.Visibility = Visibility.Visible;
        }

        private void ButtonCallSelection_Click(object sender, RoutedEventArgs e)
        {
            var callSelectionWindow = new CallSelectionWindow(UserId);
            callSelectionWindow.Show();
        }

        private void FinishCallButton_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Call.EndOftreatmentUpdate(UserId, s_bl.Volunteer.Read(UserId).VolunteerHandledCall!.CallId);
            LoadCallDetails(); // טען מחדש את פרטי הקריאה לאחר סיום הטיפול
        }

        private void CancelCallButton_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Call.CanceltreatmentUpdate(UserId, s_bl.Volunteer.Read(UserId).VolunteerHandledCall!.CallId);
            LoadCallDetails(); // טען מחדש את פרטי הקריאה לאחר ביטול הטיפול
        }
    }
}
