using BlApi;
using BO;
using DO;

namespace BlImplementation;

internal class VolunteerImplementation : IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public void add(Volunteer volunteer)
    {

        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Volunteer Read(int id)
    {
        try
        {

            DO.Volunteer volunteer = _dal.Volunteer.Read(id)!;
            if (volunteer == null)
                throw new ArgumentException("Volunteer not found.");
           
    }


    public IEnumerable<VolunteerInList> ReadAll(bool? active, Enum? sortByField)
    {
        throw new NotImplementedException();
    }

    public Role LoginUser(string name, string password)
    {
        var volunteers = _dal.Volunteer.ReadAll();
        var user = volunteers.FirstOrDefault(v => v.Name == name);
        if (user == null)
            throw new ArgumentException("User not found.");
        if (user.Password != password)
            throw new ArgumentException("Incorrect password.");
        return user.role;
    }


    public Volunteer Update (int id)
    {
        throw new NotImplementedException();
    }
}
