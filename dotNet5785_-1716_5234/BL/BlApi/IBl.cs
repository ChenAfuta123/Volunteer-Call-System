namespace BlApi;

/// <summary>Business Logic interface.</summary>
public interface IBl
{
    /// <summary>Volunteer-related operations.</summary>
    IVolunteer Volunteer { get; init; }

    /// <summary>Call-related operations.</summary>
    ICall Call { get; init; }

    /// <summary>Administrative operations.</summary>
    IAdmin Admin { get; init; }
}
