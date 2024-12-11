namespace BlImplementation;
using BlApi;
internal class Bl : IBl
{
    public ICall Call { get; init; } = new CallImplementation();
    public IVolunteer Volunteer { get; init; } = new VolunteerImplementation();
    public IAdmin Admin { get; init; } = new AdminImplementation();
}