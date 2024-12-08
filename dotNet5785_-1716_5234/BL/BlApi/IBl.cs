
namespace BlApi;

public interface IBl
{
    IVolunteer Volunteer { get; init; }
    ICall Call { get; init; }
    IAdmin Admin { get; init; }
}
