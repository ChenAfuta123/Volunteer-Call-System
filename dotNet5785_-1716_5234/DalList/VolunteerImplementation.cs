namespace Dal;

using DalApi;
using DO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

internal class VolunteerImplementation : IVolunteer
{
    /// <summary>
    /// Creates a new volunteer and adds it to the data source.
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Create(Volunteer item)
    {
        Volunteer? existId = Read(item.Id);
        if (existId != null)
        {
            throw new DalAlreadyExistsException($"Volunteer with ID={item.Id} already exists\n");
        }

        DataSource.Volunteers.Add(item);
    }

    /// <summary>
    /// Deletes a volunteer by ID from the data source.
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        Volunteer? existId = Read(id);
        if (existId == null)
        {
            throw new DalDoesNotExistsException($"Volunteer with ID={id} is not exists\n");
        }
        DataSource.Volunteers.Remove(existId);
    }

    /// <summary>
    /// Deletes all volunteers from the data source.
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)]

    public void DeleteAll()
    {
        if (DataSource.Volunteers.Any())
        {
            DataSource.Volunteers.Clear();
        }
    }

    /// <summary>
    /// Reads a volunteer by ID from the data source.
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Volunteer? Read(int id)
    {
        return DataSource.Volunteers.FirstOrDefault(item => item.Id == id);
    }

    /// <summary>
    /// Reads a volunteer that matches a specified filter from the data source.
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Volunteer? Read(Func<Volunteer, bool> filter)
    {
        return DataSource.Volunteers.FirstOrDefault(item => filter(item));
    }

    /// <summary>
    /// Reads all volunteers from the data source, with an optional filter.
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Volunteer> ReadAll(Func<Volunteer, bool>? filter = null)
        => filter == null
            ? DataSource.Volunteers.Select(item => item)
            : DataSource.Volunteers.Where(filter);

    /// <summary>
    /// Updates an existing volunteer in the data source.
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Volunteer item)
    {
        Volunteer? existId = Read(item.Id);
        if (existId == null)
        {
            throw new DalDoesNotExistsException($"Volunteer with ID={item.Id} is not exists\n");
        }
        DataSource.Volunteers.Remove(existId);
        DataSource.Volunteers.Add(item);
    }
}
