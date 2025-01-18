using BO;

namespace BlApi;

/// <summary>Interface for managing volunteer operations.</summary>
public interface IVolunteer : IObservable
{
    /// <summary>Add a new volunteer.</summary>
    public void Add(BO.Volunteer volunteer);

    /// <summary>Authenticate a volunteer and return their role.</summary>
    public DO.Role LoginUser(string name, string password);

    /// <summary>
    /// Get a list of volunteers, with optional filtering and sorting.
    /// </summary>
    public IEnumerable<BO.VolunteerInList> ReadAll(bool? active, BO.VolunteerInListFields? sortByField);

    /// <summary>Read a volunteer by their ID.</summary>
    public BO.Volunteer Read(int id);

    /// <summary>Update an existing volunteer's details.</summary>
    public void Update(int id, BO.Volunteer volunteer);

    /// <summary>Delete a volunteer by their ID.</summary>
    public void Delete(int id);
    public void SendEmailToVolunteers(IEnumerable<string> volunteerEmails, string subject, string body);
    public int ManagerID();
    public bool CanBeDeleted(int id);

}
