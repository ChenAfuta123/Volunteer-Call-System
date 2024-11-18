using DalApi;
using DO;

namespace Dal;

/// <summary>
/// Implementation of the ICall interface, responsible for CRUD operations on Call entities.
/// </summary>
internal class CallImplementation : ICall
{
    /// <summary>
    /// Creates a new Call entity with a unique auto-generated ID and adds it to the data source.
    /// </summary>
    /// <param name="item">The Call entity to be created.</param>
    public void Create(Call item)
    {
        int id =Config.NextCallId;
        Call copy = item with { Id = id };
        DataSource.Calls.Add(copy);
    }

    /// <summary>
    /// Deletes a Call entity from the data source by its ID.
    /// Throws an exception if the entity does not exist.
    /// </summary>
    /// <param name="id">The ID of the Call entity to be deleted.</param>
    public void Delete(int id)
    {
        Call? existId = Read(id);
        if (existId == null)
        {
            throw new DalDoesNotExistsException($"Call with ID={id} does not exist\n");
        }
        DataSource.Calls.Remove(existId);
    }

    /// <summary>
    /// Deletes all Call entities from the data source.
    /// </summary>
    public void DeleteAll()
    {
        DataSource.Calls.Clear();
    }

    /// <summary>
    /// Reads a Call entity from the data source by its ID.
    /// Returns null if the entity is not found.
    /// </summary>
    /// <param name="id">The ID of the Call entity to be read.</param>
    /// <returns>The Call entity if found, otherwise null.</returns>
    public Call? Read(int id)
    {
        return DataSource.Calls.FirstOrDefault(item => item.Id == id);
    }

    /// <summary>
    /// Reads all Call entities from the data source.
    /// </summary>
    /// <returns>A list of all Call entities.</returns>
    public IEnumerable<Call> ReadAll(Func<Call, bool>? filter = null) //stage 2
     => filter == null
         ? DataSource.Calls.Select(item => item)
            : DataSource.Calls.Where(filter);
    /// <summary>
    /// Updates an existing Call entity in the data source.
    /// Throws an exception if the entity does not exist.
    /// </summary>
    /// <param name="item">The Call entity to be updated.</param>
    public void Update(Call item)
    {
        Call? existId = Read(item.Id);
        if (existId == null)
        {
            throw new DalDoesNotExistsException($"Call with ID={item.Id} does not exist\n");
        }
        DataSource.Calls.Remove(existId);
        DataSource.Calls.Add(item);
    }
}
