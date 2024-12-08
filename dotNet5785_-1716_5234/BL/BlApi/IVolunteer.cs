
namespace BlApi;

public interface IVolunteer
{
    public void Add(BO.Volunteer volunteer);
    public DO.Role LoginUser(string name, string password);
    public IEnumerable<BO.VolunteerInList> ReadAll(bool? active, BO.VolunteerInListFields? sortByField);
    public BO.Volunteer Read(int id);
    public void Update(int id, BO.Volunteer volunteer);
    public void Delete(int id);
   






}
