using BlApi;
using BO;
using Helpers;
namespace BlImplementation;

internal class VolunteerImplementation :IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public void addVolunteer(BO.Volunteer volunteer)
    {
        // שלב 1: בדיקת תקינות המתנדב
        if (!ValidateVolunteer(volunteer))
        {
            throw new Exception("Invalid volunteer data.");
        }

        // שלב 2: המרת האובייקט מ-BO.Volunteer ל-DO.Volunteer
        DO.Volunteer volunteerToAdd = new DO.Volunteer
        {
            Id = volunteer.Id,
            Name = volunteer.Name,
            Email = volunteer.Email,
            PhoneNumber = volunteer.PhoneNumber,
            Address = volunteer.Address,
            Latitude = volunteer.Latitude,
            Longitude = volunteer.Longitude
        };

        try
        {
            // שלב 3: הוספת המתנדב לשכבת הנתונים
            _dal.Volunteer.Create(volunteerToAdd);
        }
        catch (Exception ex)
        {
            // שלב 4: טיפול בחריגות - במקרה של תעודת זהות כפולה או שגיאות אחרות בשכבת הנתונים
            throw new Exception("Error adding volunteer: " + ex.Message);
        }
    }



}

