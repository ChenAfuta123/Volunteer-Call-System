using BlApi;
using BO;
using DalApi;
using DO;
using Helpers;
using System.Collections.Generic;
namespace BlImplementation;

internal class VolunteerImplementation :IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public void Add(BO.Volunteer boVolunteer)
    {
        VolunteerManager.ValidateVolunteer(boVolunteer);
        DO.Volunteer doVolunteer = new DO.Volunteer
        {
            Id = boVolunteer.Id,
            distanceType = (DO.DistanceType)boVolunteer.distanceType,
            role = (DO.Role)boVolunteer.role,
            Name = boVolunteer.Name,
            PhoneNumber = boVolunteer.PhoneNumber,
            Email = boVolunteer.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(boVolunteer.Password);
            Address = boVolunteer.Address,
            Latitude = boVolunteer.Latitude,
            Longitude = boVolunteer.Longitude,
            MaxDistance = boVolunteer.MaxDistance,
            Active = boVolunteer.Active
        };
        try
        {

            _dal.Volunteer.Create(doVolunteer);

        }
        catch (DO.DalAlreadyExistsException ex)
        {

            throw new BO.BlAlreadyExistsException($"Volunteer with ID={boVolunteer.Id} already exists", ex);

        }

        catch (Exception ex)
        {
            throw new Exception($"Unexpected error while adding a volunteer: {ex.Message}");
        }



    }

    public void Delete(int id)
    {
        try
        {
            // קריאת פרטי המתנדב משכבת הנתונים
            var volunteer = _dal.Volunteer.Read(id);
            if (volunteer == null)
            {
                throw new ArgumentException("Volunteer with the given ID does not exist.");
            }

            // בדיקת תנאי המחיקה: לא טיפל אף פעם ולא מטפל כעת
            if (VolunteerManager.TotalEndTimeType(id,EndTimeType.Treated) > 0 || VolunteerManager.DOtoBO(volunteer).VolunteerHandledCall != null)
            {
                throw new InvalidOperationException("The volunteer cannot be deleted as they are handling or have handled calls.");
            }

            // מחיקת המתנדב
            _dal.Volunteer.Delete(id);
        }
        catch (DataAccessException ex) // חריגה משכבת הנתונים
        {
            throw new ApplicationException("Error occurred while attempting to delete the volunteer.", ex);
        }
    }


    public BO.Volunteer Read(int id)
    {
        try
        {

            DO.Volunteer volunteer = _dal.Volunteer.Read(id)!;
            if (volunteer == null)
                throw new ArgumentException("Volunteer not found.");
            return VolunteerManager.DOtoBO(volunteer);
        }
        catch (Exception )
        { throw new ArgumentException("Volunteer not found."); }
    }


    public IEnumerable<BO.VolunteerInList> ReadAll(bool? active, Filter? sortByField)
    {
        var volunteers = _dal.Volunteer.ReadAll();

        // סינון לפי מתנדבים פעילים ולא פעילים אם נדרש
        if (active.HasValue)
        {
            volunteers = volunteers.Where(v => v.Active == active.Value);
        }

        // המרה ל-BO.VolunteerInLis
        var BOvolunteers = volunteers.Select(v => VolunteerManager.DOtoBO( v));
        var volunteerList = BOvolunteers.Select(v => VolunteerManager.VolunteerToVolunteerList(v));

        // מיון לפי הפרמטרים
        volunteerList = sortByField switch
        {
            Filter.Name => volunteerList.OrderBy(v => v.Name),
            Filter.HandledCallId => volunteerList.OrderBy(v => v.HandledCallId),
            Filter.TotalHandledCalls => volunteerList.OrderBy(v => v.TotalHandledCalls),
            _ => volunteerList.OrderBy(v => v.Id) // מיון ברירת מחדל לפי ת.ז
        };

        return volunteerList;
    }
    public Role LoginUser(string name, string password)
    {
        var volunteers = _dal.Volunteer.ReadAll();
        var user = volunteers.FirstOrDefault(v => v.Name == name);
        if (user == null)
            throw new ArgumentException("User not found.");
        if (user.Password != password)
            throw new ArgumentException("Incorrect password.");
        return user.role;
    }


    public void Update (int id, BO.Volunteer volunteer) 
    {
        throw new NotImplementedException();
    }
}
