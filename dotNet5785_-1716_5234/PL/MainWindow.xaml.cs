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
        //public IEnumerable<BO.CallInList> CallList
        //{
        //    get { return (IEnumerable<BO.CallInList>)GetValue(CallListProperty); }
        //    set { SetValue(CallListProperty, value); }
        //}
        //public static readonly DependencyProperty CallListProperty =
        //    DependencyProperty.Register("CallList", typeof(IEnumerable<BO.CallInList>), typeof(CallListWindow), new PropertyMetadata(null));

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
                    MessageBox.Show($"Risk time range updated successfully: {timeSpanValue}");
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
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show($"An error occurred while initializing the database: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //}
                //finally
                //{
                    // החזרת סמן העכבר לברירת המחדל
                    Mouse.OverrideCursor = null;
                //}
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
        //private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //    CallList = (Call == BO.CallInListField.None) ?//לתקן
        //    s_bl?.Call.ReadAll(null,null,null)! : s_bl?.Call.ReadAll(null, BO.CallInListField.Id, Call)!;

        //}
      

    }
}
