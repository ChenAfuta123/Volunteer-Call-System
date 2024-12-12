using BlApi;
using Helpers;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace BlImplementation;

internal class VolunteerImplementation : IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public void Add(BO.Volunteer? boVolunteer)
    {
        if (boVolunteer == null) throw new BO.BlObjectNotFoundException("volunteer not found.");
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

            var volunteer = _dal.Volunteer.Read(id);


            if (VolunteerManager.TotalEndTimeType(id, DO.EndTimeType.Treated) > 0 || VolunteerManager.DOtoBO(volunteer).VolunteerHandledCall != null)
            {
                throw new BO.BlCannotBeDeletedException("The volunteer cannot be deleted as they are handling or have handled calls.");
            }

            _dal.Volunteer.Delete(id);
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

            DO.Volunteer volunteer = _dal.Volunteer.Read(id)!;
            if (volunteer == null)
                throw new BO.BlObjectNotFoundException("Volunteer not found.");
            return VolunteerManager.DOtoBO(volunteer);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Error while reading a volunteer:", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected error while  reading a volunteer: {ex.Message}");

        }
    }
    public IEnumerable<BO.VolunteerInList> ReadAll(bool? active, BO.VolunteerInListFields? sort)
    {
        var volunteers = _dal.Volunteer.ReadAll();

        if (active.HasValue)
        {
            volunteers = volunteers.Where(v => v.Active == active.Value);
        }


        var BOvolunteers = volunteers.Select(VolunteerManager.DOtoBO);
        var volunteerList = BOvolunteers.Select(VolunteerManager.VolunteerToVolunteerList);


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
        if (!BCrypt.Net.BCrypt.Verify(password, user.Password))

            throw new BO.BlValidationException("Incorrect password.");
        return user.role;
    }
    public void Update(int id, BO.Volunteer boVolunteer)
    {

        DO.Volunteer? existingVolunteer = _dal.Volunteer.Read(id);
        if (existingVolunteer == null) throw new BO.BlDoesNotExistsException($"Volunteer with ID {id} does not exist.");

        DO.Role newRole = existingVolunteer.role;
        if (existingVolunteer.role == DO.Role.manager)
        {
            newRole = (DO.Role)boVolunteer.role;
        }
        else if (existingVolunteer.Id != boVolunteer.Id)
        {
            throw new BO.BlUnauthorizedException("Volunteer cannot update other volunteer");
        }
        else if (existingVolunteer.role != DO.Role.manager)
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