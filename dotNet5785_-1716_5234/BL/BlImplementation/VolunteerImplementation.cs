using BlApi;
using BO;

namespace BlImplementation;

internal class VolunteerImplementation : IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public void addVolunteer(Volunteer volunteer)
    {

        throw new NotImplementedException();
    }

    public void DeleteVolunteer(int id)
    {
        throw new NotImplementedException();
    }

    public Volunteer GetVolunteerDetails(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<VolunteerInList> GetVolunteerList(bool? active, Enum? sortByField)
    {
        throw new NotImplementedException();
    }

    public Role LoginUser(string username, string password)
    {
        var doVolunteer = _dal.Volunteer.Readall;
        if (password != doVolunteer->password)
        throw new NotImplementedException();
    }

    public Volunteer UpdateVolunteerDetaiint id)
    {
        throw new NotImplementedException();
    }
}
