
namespace BlApi;

public interface IVolunteer
{
    public void AddVolunteer(BO.Volunteer volunteer);
    public BO.Role LoginUser(string username, string password);
    public IEnumerable<BO.VolunteerInList> ReadAll(bool? active, Enum? sortByField);
    public BO.Volunteer Read(int id, BO.Volunteer);
    public void Update(int id, BO.Volunteer);
    public void Delete(int id);
   






}
