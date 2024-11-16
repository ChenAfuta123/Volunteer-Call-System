namespace Dal;
using DalApi;
using global::DalList;

sealed public class DalList : IDal
{
    public IVolunteer Volunteer => throw new NotImplementedException();

    public ICall Call => throw new NotImplementedException();

    public IAssignment Assignment => throw new NotImplementedException();

    public IConfig Config => throw new NotImplementedException();

    public void ResetDB()
    {
        Call.DeleteAll();
        Volunteer.DeleteAll();
        Assignment.DeleteAll();
        Config.Reset();

    }
public ICall call { get; } = new CallImplementation();
public IVolunteer volunteer { get; } = new VolunteerImplementation();
public IAssignment assignment { get; } = new AssignmentImplementation();

public IConfig config { get; } = new ConfigImplementation();


}

