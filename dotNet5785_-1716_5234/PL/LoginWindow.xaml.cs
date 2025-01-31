using PL.Volunteer;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public class LoggedInUser
        {
            public string? Id { get; set; } // ת.ז
        }
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int userId = int.Parse(IdTextBox.Text.Trim());
                var name = s_bl.Volunteer.Read(userId).Name;
                string password = PasswordBox.Password;
                // בדיקת אימות משתמש
                var V_role = s_bl.Volunteer.LoginUser(name, password);
                // ניתוב לפי סוג המשתמש
                if (V_role == DO.Role.volunteer)
                {
                    PersonalVolunteerWindow volunteerWindow = new PersonalVolunteerWindow(userId);
                    volunteerWindow.Show();
                    Close();
                }
                else if (V_role == DO.Role.manager)
                {
                    MessageBoxResult result = MessageBox.Show("האם ברצונך להיכנס בתור מנהל?", "כניסת מנהל",
                                                              MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        // מעבר לחלון המנהל
                        MainWindow managerChoiceWindow = new MainWindow();
                        managerChoiceWindow.Show();
                    }
                    else
                    {
                        // מעבר לחלון האישי
                        PersonalVolunteerWindow volunteerWindow = new PersonalVolunteerWindow(userId);
                        volunteerWindow.Show();
                    }

                    // סגירת החלון הנוכחי
                    Close();
                }

            }
            catch (BO.BlObjectNotFoundException ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BlValidationException ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}

        }

    }
    
}

