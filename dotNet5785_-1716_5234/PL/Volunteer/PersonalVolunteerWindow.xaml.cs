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

        public PersonalVolunteerWindow()
        {
            InitializeComponent();
        }
        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void VolunteerButton_Click(object sender, RoutedEventArgs e)
        {
            var volunteerWindow= new VolunteerWindow();

        }
    }
}
