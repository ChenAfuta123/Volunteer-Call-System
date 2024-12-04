
namespace BlApi;

public interface IVolunteer
{
    public Role LoginUser(string username, string password);
    public IEnumerable<BO.VolunteerInList> ReadAll(bool? active, Enum? sortByField);
    public BO.Volunteer Read(int id);
    public BO.Volunteer Update(int id);
    public void Delete(int id);
    public void addV(BO.Volunteer volunteer);






}
