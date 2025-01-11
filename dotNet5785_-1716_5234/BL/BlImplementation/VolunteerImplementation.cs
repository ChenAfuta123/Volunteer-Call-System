using BlApi;
using Helpers;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace BlImplementation;

internal class VolunteerImplementation : IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    public void AddObserver(Action listObserver) =>
   VolunteerManager.Observers.AddListObserver(listObserver); //stage 5
    public void AddObserver(int id, Action observer) =>
   VolunteerManager.Observers.AddObserver(id, observer); //stage 5
    public void RemoveObserver(Action listObserver) =>
   VolunteerManager.Observers.RemoveListObserver(listObserver); //stage 5
    public void RemoveObserver(int id, Action observer) =>
    VolunteerManager.Observers.RemoveObserver(id, observer); //stage 5
    public void Add(BO.Volunteer? boVolunteer)
    {
        if (boVolunteer == null) throw new BO.BlObjectNotFoundException("volunteer not found.");

        var (Latitude, Longitude) = Tools.DistanceCalculator.GetAddressCoordinates(boVolunteer.Address);
        double longtitude = Longitude ?? 0.0;
        double latitude = Latitude ?? 0.0;
        boVolunteer.Latitude = latitude;
        boVolunteer.Longitude = longtitude;
        VolunteerManager.ValidateVolunteer(boVolunteer);
        DO.Volunteer doVolunteer = new DO.Volunteer
        {
            Id = boVolunteer.Id,
            distanceType = (DO.DistanceType)boVolunteer.distanceType,
            role = (DO.Role)boVolunteer.role,
            Name = boVolunteer.Name,
            PhoneNumber = boVolunteer.PhoneNumber,
            Email = boVolunteer.Email,
            Password =BCrypt.Net.BCrypt.HashPassword(boVolunteer.Password),
            Address = boVolunteer.Address,
            Latitude = boVolunteer.Latitude ,
            Longitude = boVolunteer.Longitude,
            MaxDistance = boVolunteer.MaxDistance,
            Active = boVolunteer.Active
        };
        try
        {

            _dal.Volunteer.Create(doVolunteer);
            VolunteerManager.Observers.NotifyItemUpdated(doVolunteer.Id);
            VolunteerManager.Observers.NotifyListUpdated();
        }

        catch (DO.DalAlreadyExistsException ex)
        {

            throw new BO.BlAlreadyExistsException("Error occurred while attempting to add the volunteer.", ex);

        }



    }
    public void Delete(int id)
    {
        try
        {

            var volunteer = _dal.Volunteer.Read(id);


            if (VolunteerManager.TotalEndTimeType(id, DO.EndTimeType.Treated) > 0 || VolunteerManager.DOtoBO(volunteer).VolunteerHandledCall != null)
            {
                throw new BO.BlCannotBeDeletedException("The volunteer cannot be deleted as they are handling or have handled calls.");
            }

            _dal.Volunteer.Delete(id);
            VolunteerManager.Observers.NotifyItemUpdated(id);
            VolunteerManager.Observers.NotifyListUpdated();
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException("Error occurred while attempting to delete the volunteer.", ex);
        }
    }
    public BO.Volunteer Read(int id)
    {
        try
        {

            DO.Volunteer? volunteer = _dal.Volunteer.Read(id);
            return VolunteerManager.DOtoBO(volunteer);
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Error while reading a volunteer:", ex);
        }
        //catch (Exception ex)
        //{
        //    throw new Exception($"Unexpected error while  reading a volunteer: {ex.Message}");

        //}
    }
    public IEnumerable<BO.VolunteerInList> ReadAll(bool? active, BO.VolunteerInListFields? sort)
    {
  // קריאה ל-DAL
            var volunteers = _dal.Volunteer.ReadAll();

        if (active == null)
        {
            volunteers = _dal.Volunteer.ReadAll();

        }
        // פילטר לפי Active
        if (active==true)
        {
            volunteers = volunteers.Where(v => v.Active == active.Value);
           
        }
        if (active == false)
        {
            volunteers = volunteers.Where(v => v.Active == active.Value);

        }

        // המרה מ-DO ל-BO
        var BOvolunteers = volunteers.Select(VolunteerManager.DOtoBO);
      

        // המרה לרשימת VolunteerInList
        var volunteerList = BOvolunteers.Select(VolunteerManager.VolunteerToVolunteerList);
 

        // מיון
        volunteerList = sort switch
        {
            BO.VolunteerInListFields.Name => volunteerList.OrderBy(v => v.Name),
            BO.VolunteerInListFields.HandledCallId => volunteerList.OrderBy(v => v.HandledCallId),
            BO.VolunteerInListFields.TotalHandledCalls => volunteerList.OrderBy(v => v.TotalHandledCalls),
            _ => volunteerList.OrderBy(v => v.Id)
        };


        return volunteerList;
    }
 
  
    public DO.Role LoginUser(string name, string password)
    {
        DO.Volunteer? user = _dal.Volunteer.Read(v => v.Name == name);

        if (user == null)
            throw new BO.BlObjectNotFoundException("User not found.");
        //if (!BCrypt.Net.BCrypt.Verify(password, user.Password))

        //    throw new BO.BlValidationException("Incorrect password.");
        return user.role;
    }
    public void Update(int id, BO.Volunteer boVolunteer)
    {

        DO.Volunteer? existingVolunteer = _dal.Volunteer.Read(boVolunteer.Id);
        if (existingVolunteer == null) 
            throw new BO.BlDoesNotExistsException($"Volunteer with ID {id} does not exist.");

        DO.Role newRole = existingVolunteer.role;
        if (existingVolunteer.role == DO.Role.manager)
        {
            newRole = (DO.Role)boVolunteer.role;
        }
        else if (existingVolunteer.Id != id)
        {
            throw new BO.BlUnauthorizedException("Volunteer cannot update other volunteer");
        }
        else if (existingVolunteer.role != DO.Role.manager && existingVolunteer.role != (DO.Role)boVolunteer.role)
        {
            throw new BO.BlUnauthorizedException("Volunteer cannot update role");
        }

        DO.Volunteer updatedVolunteer = new DO.Volunteer
        {
            Id = existingVolunteer.Id,
            Name = boVolunteer.Name,
            PhoneNumber = boVolunteer.PhoneNumber,
            Email = boVolunteer.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(boVolunteer.Password),
            Address = boVolunteer.Address,
            Latitude = boVolunteer.Latitude,
            Longitude = boVolunteer.Longitude,
            MaxDistance = boVolunteer.MaxDistance,
            Active = boVolunteer.Active,
            distanceType = (DO.DistanceType)boVolunteer.distanceType,
            role = newRole
        };


        try
        {
            _dal.Volunteer.Update(updatedVolunteer);
            VolunteerManager.Observers.NotifyItemUpdated(existingVolunteer.Id);
            VolunteerManager.Observers.NotifyListUpdated();
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException("Error occurred while attempting to update the volunteer.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected error while updating the volunteer: {ex.Message}");
        }
    }



}