using BlApi;
using Helpers;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
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


    private  async Task updateCoordinatesForVolunteerAddressAsync(DO.Volunteer doVolunteer)
    {
        if (doVolunteer.Address is not null)
        {
            var loc = await Tools.DistanceCalculator.GetAddressCoordinatesAsync(doVolunteer.Address);
            double Long = loc.Longitude ?? 0.0;
            double Lat = loc.Latitude ?? 0.0;

            doVolunteer = doVolunteer with { Latitude = Lat, Longitude = Long };
            lock (AdminManager.BlMutex)
                _dal.Volunteer.Update(doVolunteer);
            CallManager.Observers.NotifyListUpdated();
            CallManager.Observers.NotifyItemUpdated(doVolunteer.Id);

        }
    }
    public void Add(BO.Volunteer? boVolunteer)
    {
        AdminManager.ThrowOnSimulatorIsRunning();
        if (boVolunteer == null) throw new BO.BlObjectNotFoundException("volunteer not found.");

     
        VolunteerManager.ValidateVolunteer(boVolunteer,true);
        DO.Volunteer doVolunteer = new DO.Volunteer
        {
            Id = boVolunteer.Id,
            distanceType = (DO.DistanceType)boVolunteer.distanceType,
            role = (DO.Role)boVolunteer.role,
            Name = boVolunteer.Name,
            PhoneNumber = boVolunteer.PhoneNumber,
            Email = boVolunteer.Email,
            Password = /*BCrypt.Net.BCrypt.HashPassword(*/boVolunteer.Password/*)*/,
            Address = boVolunteer.Address,
            Latitude = boVolunteer.Latitude,
            Longitude = boVolunteer.Longitude,
            MaxDistance = boVolunteer.MaxDistance,
            Active = boVolunteer.Active
        };
        try
        {
            lock (AdminManager.BlMutex)
                _dal.Volunteer.Create(doVolunteer);
            VolunteerManager.Observers.NotifyItemUpdated(doVolunteer.Id);
            VolunteerManager.Observers.NotifyListUpdated();
            _ = updateCoordinatesForVolunteerAddressAsync(doVolunteer);
        }

        catch (DO.DalAlreadyExistsException ex)
        {

            throw new BO.BlAlreadyExistsException("Error occurred while attempting to add the volunteer.", ex);

        }



    }
    public bool CanBeDeleted(int id)
    {
        lock (AdminManager.BlMutex)
        {
            var volunteer = _dal.Volunteer.Read(id);

            return VolunteerManager.TotalEndTimeType(id, DO.EndTimeType.Treated) == 0 &&VolunteerManager.DOtoBO(volunteer).VolunteerHandledCall == null;
        }
    }
    public void Delete(int id)
    {
        AdminManager.ThrowOnSimulatorIsRunning();
        try
        {
            
                if (!CanBeDeleted(id))
                {
                    throw new BO.BlCannotBeDeletedException("The volunteer cannot be deleted as they are handling or have handled calls.");
                }
            lock (AdminManager.BlMutex)
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
            lock (AdminManager.BlMutex)
            {
                DO.Volunteer? volunteer = _dal.Volunteer.Read(id);
                return VolunteerManager.DOtoBO(volunteer);
            }
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistsException($"Error while reading a volunteer:", ex);
        }
      
    }
    public IEnumerable<BO.VolunteerInList> ReadAll(bool? active, BO.VolunteerInListFields? sort)
    {
        lock (AdminManager.BlMutex)
        {
            var volunteers = _dal.Volunteer.ReadAll();

            if (active == null)
            {
                volunteers = _dal.Volunteer.ReadAll();

            }
            // פילטר לפי Active
            if (active == true)
            {
                volunteers = volunteers.Where(v => v.Active == active.Value);

            }
            if (active == false)
            {
                volunteers = volunteers.Where(v => v.Active == active.Value);

            }

            // המרה מ-DO ל-BO
            var BOvolunteers = volunteers.Select(v =>
            {
                return VolunteerManager.DOtoBO(v);
            });



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
    }


    public DO.Role LoginUser(string name, string password)
    {
        lock (AdminManager.BlMutex)
        {
            DO.Volunteer? user = _dal.Volunteer.Read(v => v.Name == name);

            if (user == null)
                throw new BO.BlObjectNotFoundException("User not found.");
            if (user.Password != "")
            {
                if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                    throw new BO.BlValidationException("Incorrect password.");
            }
            return user.role;
        }
    }
    public void Update(int id, BO.Volunteer boVolunteer)
    {
        AdminManager.ThrowOnSimulatorIsRunning();
        lock (AdminManager.BlMutex)
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
                throw new BO.BlUnauthorizedException("Volunteer ca  nnot update other volunteer");
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
                VolunteerManager.ValidateVolunteer(boVolunteer,false);

                lock (AdminManager.BlMutex)
                    _dal.Volunteer.Update(updatedVolunteer);
                VolunteerManager.Observers.NotifyItemUpdated(existingVolunteer.Id);
                VolunteerManager.Observers.NotifyListUpdated();
                _ = updateCoordinatesForVolunteerAddressAsync(updatedVolunteer);
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

    public async Task SendEmailToVolunteersAsync(IEnumerable<string> volunteerEmails, string subject, string body)
    {
        try
        {
            // הגדרות ה-SMTP
            using SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("chenafuta@gmail.com", "lgfi iurz fvvy xicc"),
                EnableSsl = true
            };

            // יצירת הודעת המייל
            using MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("chenafuta@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true // אם את רוצה לשלוח מייל בפורמט HTML
            };

            // הוספת כל המתנדבים לרשימת הנמענים
            foreach (string email in volunteerEmails)
            {
                mailMessage.To.Add(email);
            }

            // שליחת המייל
            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public int ManagerID()
    {
        lock (AdminManager.BlMutex)
        {
            var manager = _dal.Volunteer.Read(a => (BO.Role)a.role == BO.Role.manager);
            if (manager != null)
                return manager.Id;
            else
                return -1;
        }
    }
   
    public List<string> CloseVolunteersToCallEmails(int callId)
    {
        lock (AdminManager.BlMutex)
        {
            var call = _dal.Call.Read(callId);


            if (call != null)
            {
                // שליפת כל המתנדבים מה-DAL
                var volunteers = _dal.Volunteer.ReadAll();

                // סינון המתנדבים שנמצאים קרוב לקריאה
                var closeVolunteers = volunteers.Where(v =>VolunteerManager.IfisClose(v, call));

                // שליפת כתובות האימייל של המתנדבים הקרובים
                return closeVolunteers.Select(v => v.Email).ToList();
            }
            return new List<string>();
        }

    }

    public double CallDistanceFromvolunteer(int Vid)
    {
        var volunteer = _dal.Volunteer.Read(Vid);
        var assignment = _dal.Assignment.Read(a => a.VolunteerId == Vid && a.EndTime == null);
        if (assignment != null)
        {

            var call = _dal.Call.Read(call => call.Id == assignment.CallId);
            if (call != null)
                return Tools.DistanceCalculator.CalculateDistance(volunteer!.Latitude, volunteer.Longitude,
                               call.Latitude, call.Longitude, volunteer.distanceType);

        }
        return 0;
    }

}