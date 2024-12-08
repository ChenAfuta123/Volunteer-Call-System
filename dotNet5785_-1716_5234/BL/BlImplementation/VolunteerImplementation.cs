using BlApi;
using DO;
using Helpers;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;
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
            Password = BCrypt.Net.BCrypt.HashPassword(boVolunteer.Password),
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
            
            throw new BO.BlAlreadyExistsException("Error occurred while attempting to add the volunteer.", ex);

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
           

            // בדיקת תנאי המחיקה: לא טיפל אף פעם ולא מטפל כעת
            if (VolunteerManager.TotalEndTimeType(id, DO.EndTimeType.Treated) > 0 || VolunteerManager.DOtoBO(volunteer).VolunteerHandledCall != null)
            {
                throw new BO.BlValidationException("The volunteer cannot be deleted as they are handling or have handled calls.");
            }

            // מחיקת המתנדב
            _dal.Volunteer.Delete(id);
        }
        catch (DO.DalDoesNotExistsException ex) // חריגה משכבת הנתונים
        {
            throw new BO.BlDoesNotExistsException("Error occurred while attempting to delete the volunteer.", ex);
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


    public IEnumerable<BO.VolunteerInList> ReadAll(bool? active, BO.VolunteerInListFields? sort)
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
        volunteerList = sort switch
        {
            BO.VolunteerInListFields.Name => volunteerList.OrderBy(v => v.Name),
            BO.VolunteerInListFields.HandledCallId => volunteerList.OrderBy(v => v.HandledCallId),
            BO.VolunteerInListFields.TotalHandledCalls => volunteerList.OrderBy(v => v.TotalHandledCalls),
            _ => volunteerList.OrderBy(v => v.Id) // מיון ברירת מחדל לפי ת.ז
        };

        return volunteerList;
    }
    public DO.Role LoginUser(string name, string password)
    {
        var volunteers = _dal.Volunteer.ReadAll();
        var user = volunteers.FirstOrDefault(v => v.Name == name);
        if (user == null)
            throw new ArgumentException("User not found.");
        if (user.Password != password)
            throw new ArgumentException("Incorrect password.");
        return user.role;
    }


    public void Update (int id, BO.Volunteer boVolunteer) 
    {
        VolunteerManager.ValidateVolunteer(boVolunteer);
        DO.Volunteer? V = _dal.Volunteer.Read(id);

        DO.Volunteer doVolunteer = new DO.Volunteer
        {
            //Id = boVolunteer.Id,
            distanceType = (DO.DistanceType)boVolunteer.distanceType,
            role = (DO.Role)boVolunteer.role,
            Name = boVolunteer.Name,
            PhoneNumber = boVolunteer.PhoneNumber,
            Email = boVolunteer.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(boVolunteer.Password),
            Address = boVolunteer.Address,
            Latitude = boVolunteer.Latitude,
            Longitude = boVolunteer.Longitude,
            MaxDistance = boVolunteer.MaxDistance,
            Active = boVolunteer.Active
        };
        try
        {

            _dal.Volunteer.Update(doVolunteer);

        }
        catch (DO.DalDoesNotExistsException ex)
        {

            throw new BO.BlDoesNotExistsException("Error occurred while attempting to update the volunteer.", ex);

        }

        catch (Exception ex)
        {
            throw new Exception($"Unexpected error while updateing a volunteer: {ex.Message}");
        }
    }



}
