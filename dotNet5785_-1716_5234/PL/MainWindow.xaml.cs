using BO;
using PL.Call;
using PL.Volunteer;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public static readonly DependencyProperty OpenCallsCountProperty =
     DependencyProperty.Register("OpenCallsCount", typeof(int), typeof(MainWindow));

        public static readonly DependencyProperty ClosedCallsCountProperty =
            DependencyProperty.Register("ClosedCallsCount", typeof(int), typeof(MainWindow));

        public static readonly DependencyProperty InProgressCallsCountProperty =
            DependencyProperty.Register("InProgressCallsCount", typeof(int), typeof(MainWindow));

        public static readonly DependencyProperty ExpiredCallsCountProperty =
            DependencyProperty.Register("ExpiredCallsCount", typeof(int), typeof(MainWindow));

        public static readonly DependencyProperty AtRiskOpenCallsCountProperty =
            DependencyProperty.Register("AtRiskOpenCallsCount", typeof(int), typeof(MainWindow));

        public static readonly DependencyProperty AtRiskInProgressCallsCountProperty =
            DependencyProperty.Register("AtRiskInProgressCallsCount", typeof(int), typeof(MainWindow));

        public int OpenCallsCount
        {
            get { return (int)GetValue(OpenCallsCountProperty); }
            set { SetValue(OpenCallsCountProperty, value); }
        }

        public int ClosedCallsCount
        {
            get { return (int)GetValue(ClosedCallsCountProperty); }
            set { SetValue(ClosedCallsCountProperty, value); }
        }

        public int InProgressCallsCount
        {
            get { return (int)GetValue(InProgressCallsCountProperty); }
            set { SetValue(InProgressCallsCountProperty, value); }
        }

        public int ExpiredCallsCount
        {
            get { return (int)GetValue(ExpiredCallsCountProperty); }
            set { SetValue(ExpiredCallsCountProperty, value); }
        }

        public int AtRiskOpenCallsCount
        {
            get { return (int)GetValue(AtRiskOpenCallsCountProperty); }
            set { SetValue(AtRiskOpenCallsCountProperty, value); }
        }

        public int AtRiskInProgressCallsCount
        {
            get { return (int)GetValue(AtRiskInProgressCallsCountProperty); }
            set { SetValue(AtRiskInProgressCallsCountProperty, value); }
        }


        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        public TimeSpan RiskRange
        {
            get { return (TimeSpan)GetValue(RiskRangeProperty); }
            set { SetValue(RiskRangeProperty, value); }
        }
      

        public static readonly DependencyProperty CurrentTimeProperty =
        DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow));


        public static readonly DependencyProperty RiskRangeProperty =
       DependencyProperty.Register("RiskRange", typeof(TimeSpan), typeof(MainWindow));
        public MainWindow()
        {
            InitializeComponent();


        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                
                // קריאה למתודת BO כדי לקבל את הנתונים
                var statusCounts = s_bl.Call.CallQuantities();
                Dispatcher.Invoke(() =>
                {
                    // עדכון כמויות הקריאות
                    OpenCallsCount = statusCounts[0];
                    ClosedCallsCount = statusCounts[1];
                    InProgressCallsCount = statusCounts[2];
                    ExpiredCallsCount = statusCounts[3];
                    AtRiskOpenCallsCount = statusCounts[4];
                    AtRiskInProgressCallsCount = statusCounts[5];
                    this.DataContext = this;
                });

                // עידכון התצוגה
                // עדכון מחדש של ה-DataContext

                CurrentTime = s_bl.Admin.getClockTime();

                var configValues = s_bl.Admin.getRiskTimeRange();


                s_bl.Admin.AddClockObserver(clockObserver);


                s_bl.Admin.AddConfigObserver(configObserver);


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during initialization: {ex.Message}");
            }
        }
        private void MainWindow_Close(object sender, RoutedEventArgs e)
        {
            try
            {


                s_bl.Admin.RemoveClockObserver(clockObserver);

                s_bl.Admin.RemoveConfigObserver(configObserver);


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during initialization: {ex.Message}");
            }
        }
        private void clockObserver()
        {
            try
            {
                CurrentTime = s_bl.Admin.getClockTime();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating clock: {ex.Message}");
            }

        }
        private void configObserver()
        {
            try
            {
                // כאן ניתן לקרוא למתודה רלוונטית מה-BL לעדכון משתני התצורה
                var configValues = s_bl.Admin.getRiskTimeRange();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating configuration: {ex.Message}");
            }
        }
        private void btnAddOneMinute_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.MINUTE);

        }
        private void btnAddOneHour_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.HOUR);

        }
        private void btnAddOneDay_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.DAY);

        }

        private void btnAddOneMonth_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.MONTH);

        }
        private void btnAddOneYear_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.AdvanceClock(BO.TimeUnit.YEAR);

        }

        private void btRiskRangeUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (TimeSpan.TryParse(txtTimeSpan.Text, out TimeSpan timeSpanValue))
                {

                    s_bl.Admin.setRiskTimeRange(timeSpanValue);
                    RiskRange = s_bl.Admin.getRiskTimeRange();

                }
                else
                {
                    MessageBox.Show("Invalid TimeSpan format. Please use HH:mm:ss.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }
        private void btnCalls_Click(object sender, RoutedEventArgs e)
        {
            // יצירת מופע של מסך תצוגת הרשימה ופתיחתו
            CallListWindow callListWindow = new CallListWindow();
            callListWindow.Show(); // שימוש ב-Show לשמירה על גישה למסך הראשי
        }
        private void btnVolunteers_Click(object sender, RoutedEventArgs e)
        {
            // יצירת מופע של מסך תצוגת הרשימה ופתיחתו
            VolunteerListWindow volunteerListWindow = new VolunteerListWindow();
            volunteerListWindow.Show(); // שימוש ב-Show לשמירה על גישה למסך הראשי
        }
        private void btnInitializeDB_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to initialize the database?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                //try
                //{
                // שינוי סמן העכבר לשעון חול
                Mouse.OverrideCursor = Cursors.Wait;

                // סגירת כל החלונות הפתוחים פרט לחלון הנוכחי
                foreach (Window window in Application.Current.Windows)
                {
                    if (window != this)
                    {
                        window.Close();
                    }
                }

                // קריאה למתודה לאתחול בסיס הנתונים
                s_bl.Admin.setDatabase();

                MessageBox.Show("Database initialized successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
               
                Mouse.OverrideCursor = null;
               
            }



        }

        /// <summary>
        /// Handles the click event to reset the database.
        /// </summary>
        private void btnResetDB_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reset the database?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {

                    // שינוי סמן העכבר לשעון חול
                    Mouse.OverrideCursor = Cursors.Wait;

                    // סגירת כל החלונות הפתוחים פרט לחלון הנוכחי
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window != this)
                        {
                            window.Close();
                        }
                    }

                    // קריאה למתודה לאיפוס בסיס הנתונים
                    s_bl.Admin.resetDatabase();

                    MessageBox.Show("Database reset successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while resetting the database: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    // החזרת סמן העכבר לברירת המחדל
                    Mouse.OverrideCursor = null;
                }
            }
        }

        public BO.CallInListField Call { get; set; } = BO.CallInListField.None;

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var callListWindow = new CallListWindow(BO.CallInListField.CallStatus, "Open");

            callListWindow.ShowDialog();
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var callListWindow = new CallListWindow(BO.CallInListField.CallStatus, "Closed");

            // הצגת החלון
            callListWindow.ShowDialog();
        }


        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var callListWindow = new CallListWindow(BO.CallInListField.CallStatus, "InProgress");

            // הצגת החלון
            callListWindow.ShowDialog();
        }

        // הצגת החלון

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var callListWindow = new CallListWindow(BO.CallInListField.CallStatus, "Expired");

            // הצגת החלון
            callListWindow.ShowDialog();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            var callListWindow = new CallListWindow(BO.CallInListField.CallStatus, "OpenAtRisk");

            // הצגת החלון
            callListWindow.ShowDialog();
        }




        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            var callListWindow = new CallListWindow(BO.CallInListField.CallStatus, "InProgressAtRisk");

            // הצגת החלון
            callListWindow.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void TextBlock_OpenCalls_TextChanged(object sender, TextChangedEventArgs e)
        {
            var statusCounts = s_bl.Call.CallQuantities();
            (sender as TextBlock)!.Text = statusCounts[0].ToString();
        }

        private void TextBlock_ClosedCalls_TextChanged(object sender, TextChangedEventArgs e)
        {
            var statusCounts = s_bl.Call.CallQuantities();
            (sender as TextBlock)!.Text = statusCounts[2].ToString();
        }

        private void TextBlock_ExpiredCalls_TextChanged(object sender, TextChangedEventArgs e)
        {
            var statusCounts = s_bl.Call.CallQuantities();
            (sender as TextBlock)!.Text = statusCounts[3].ToString();
        }

        private void TextBlock_InProcessCalls_TextChanged(object sender, TextChangedEventArgs e)
        {
            var statusCounts = s_bl.Call.CallQuantities();
            (sender as TextBlock)!.Text = statusCounts[1].ToString();
        }

        private void TextBlock_InProcessRiskCalls_TextChanged(object sender, TextChangedEventArgs e)
        {
            var statusCounts = s_bl.Call.CallQuantities();
            (sender as TextBlock)!.Text = statusCounts[5].ToString();
        }

        private void TextBlock_OpenRiskCalls_TextChanged(object sender, TextChangedEventArgs e)
        {
            var statusCounts = s_bl.Call.CallQuantities();
            (sender as TextBlock)!.Text = statusCounts[4].ToString();
        }


    }
}