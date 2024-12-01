
namespace BlApi;

public interface IVolunteer
{
    public Role LoginUser(string username, string password);
    public BO.VolunteerInList[] GetVolunteerList(bool? active, Enum? sortByField);
    public BO.Volunteer GetVolunteerDetails(int id);
    public BO.Volunteer UpdateVolunteerDetails(int id);
    public void DeleteVolunteer(int id);
    public void addVolunteer(BO.Volunteer volunteer);






}
