using BlApi;
using BO;
using DalApi;
using DO;
using Helpers;
using System.ComponentModel.DataAnnotations;
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
            distanceType =(DO.DistanceType)boVolunteer.distanceType,
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

    public BO.Role LoginUser(string username, string password)
    {
        throw (new NotImplementedException());
    }
    public IEnumerable<BO.VolunteerInList> ReadAll(bool? active, Enum? sortByField)
    {
        throw(new NotImplementedException());
    }
    public BO.Volunteer Read(int id)
    {
        throw (new NotImplementedException());
    }
    public void Update(int id, BO.Volunteer boVolunteer)
    {
        VolunteerManager.ValidateVolunteer(boVolunteer);
        try
        {

            //_dal.Volunteer.Update(doVolunteer);

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
        throw (new NotImplementedException());

    }


}

