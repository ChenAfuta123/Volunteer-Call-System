using BlApi;
using BO;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PL.Call
{
    /// <summary>
    /// Interaction logic for CallWindow.xaml
    /// </summary>
    public partial class CallWindow : Window
    {

        static readonly IBl s_bl = Factory.Get();
        // Dependency Properties
        public static readonly DependencyProperty CurrentCallProperty =
            DependencyProperty.Register(
                nameof(CurrentCall),
                typeof(BO.Call),
                typeof(CallWindow),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register(
                nameof(ButtonText),
                typeof(string),
                typeof(CallWindow),
                new PropertyMetadata("Add"));

        private BO.CallType? _callType;
        public BO.CallType? CallType
        {
            get => _callType;
            set
            {
                if (_callType != value)
                {
                    _callType = value;

                }
            }
        }

        private BO.CallStatus? _callStatus;
        public BO.CallStatus? CallStatus
        {
            get => _callStatus;
            set
            {
                if (_callStatus != value)
                {
                    _callStatus = value;

                }
            }
        }

        // Properties
        public BO.Call CurrentCall
        {
            get => (BO.Call)GetValue(CurrentCallProperty);
            set => SetValue(CurrentCallProperty, value);
        }

        public string ButtonText
        {
            get => (string)GetValue(ButtonTextProperty);
            set => SetValue(ButtonTextProperty, value);
        }


        public CallWindow(int id = 0)
        {
            InitializeComponent();

            if (id == 0)
            {
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


                ButtonText = "Add";
            }
            else
            {
                try
                {
                    CurrentCall = BlApi.Factory.Get().Call.Read(id);
                    ButtonText = "Update";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading call data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (ButtonText == "Add")
                {
                    s_bl.Call.Add(CurrentCall);
                    MessageBox.Show("Call added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (ButtonText == "Update")
                {
                    s_bl.Call.Update(CurrentCall);
                    MessageBox.Show("Call updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // שיטה זו יכולה להיות מופעלת בזמן טעינת המסך
        private void LoadScreen()
        {
            OnScreenLoaded(this, EventArgs.Empty);
        }

        // שיטה זו יכולה להיות מופעלת בזמן סגירת המסך
        private void CloseScreen()
        {
            OnScreenClosed(this, EventArgs.Empty);
        }
        // הגדרת מתודת השקפה שתמלא את הפריט מחדש
        private void CallObserver()
        {
            int id = CurrentCall!.Id;
            CurrentCall = null;
            CurrentCall = s_bl.Call.Read(id);
        }

        // הצטרפות לאירוע טעינת המסך
        private void OnScreenLoaded(object sender, EventArgs e)
        {
            if (CurrentCall!.Id != 0)
            {
                s_bl.Call.AddObserver(CurrentCall!.Id, CallObserver);
            }
        }

        // הצטרפות לאירוע סגירת המסך
        private void OnScreenClosed(object sender, EventArgs e)
        {
            if (CurrentCall!.Id != 0)
            {
                s_bl.Call.RemoveObserver(CurrentCall!.Id, CallObserver);
            }
        }


    }
}
