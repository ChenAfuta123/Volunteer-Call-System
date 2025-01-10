using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Volunteer
{
    /// <summary>
    /// Interaction logic for PersonalVolunteerWindow.xaml
    /// </summary>
    public partial class PersonalVolunteerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        private int UserId { get; set; }

        public PersonalVolunteerWindow(int userId)
        {
            InitializeComponent();
            UserId = userId;
        }
        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            var history = new HistoryWindow(UserId);
            history.Show();
        }
        private void VolunteerButton_Click(object sender, RoutedEventArgs e)
        {
            var volunteerWindow= new VolunteerWindow();
            volunteerWindow.Show();

        }
        private void LoadCallDetails(object sender, RoutedEventArgs e)
        {
            try
            {
                var volunteer = s_bl.Volunteer.Read(UserId);
                if (volunteer.VolunteerHandledCall == null)
                {  if(volunteer.Active != false)
                    CallSelectionButton.IsEnabled = true;  // מאפשר את כפתור הבחירה
                    HideCallDetails();  // מסיר את פרטי הקריאה
                }
                else
                {
                    // יש קריאה בטיפול - הצג את פרטי הקריאה
                    var callid = volunteer.VolunteerHandledCall.CallId;
                    var call = s_bl.Call.Read(callid);
                    ShowCallDetails(call);  // מציג את פרטי הקריאה
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading call details: {ex.Message}");
            }
        }
        private void HideCallDetails()
        {
            // הסתר את כל תיבות הטקסט של פרטי הקריאה
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
            // הצג את פרטי הקריאה במידה ויש קריאה בטיפול
            CallIdTextBox.Text = call.Id.ToString();
            CallStatusTextBox.Text = call.callStatus.ToString();
            CallTypeTextBox.Text = call.callType.ToString();
            CallDistanceTextBox.Text =s_bl.Volunteer.Read(UserId).VolunteerHandledCall!.CallDistanceFromVolunteer.ToString("F2");
            CallAddressTextBox.Text = call.Address;
            CallOpeningTimeTextBox.Text = call.OpeningTime.ToString("g");
            CallMaxEndingTimeTextBox.Text = call.MaxEndingTime?.ToString("g") ?? "לא מוגדר";
        }



        //private void CallAssignDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}

        //private void ButtonCallSelection_Click(object sender, RoutedEventArgs e)
        //{
        //    var callSelectionWindow=new CallSelectionWindow(UserId);
        //    callSelectionWindow.Show();
        //}
        private void ButtonCallSelection_Click(object sender, RoutedEventArgs e)
        {
            var callSelectionWindow = new CallSelectionWindow(UserId);
            callSelectionWindow.Show();
        }

        private void FinishCallButton_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Call.EndOftreatmentUpdate(UserId, s_bl.Volunteer.Read(UserId).VolunteerHandledCall!.CallId);
        }

        private void CancelCallButton_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Call.CanceltreatmentUpdate(UserId, s_bl.Volunteer.Read(UserId).VolunteerHandledCall!.CallId);
        }
    }
}
