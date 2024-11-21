using DO;
using DalApi;

namespace Dal;

/// <summary>
/// Implementation of the IAssignment interface for managing assignments in the data source.
/// </summary>
internal class AssignmentImplementation : IAssignment
{
    /// <summary>
    /// Creates a new assignment and adds it to the data source.
    /// </summary>
    public void Create(Assignment item)
    {
        int id = Config.NextAssignmentId;
        Assignment copy = item with { Id = id };
        DataSource.Assignments.Add(copy);
    }

    /// <summary>
    /// Deletes an assignment with the specified ID from the data source.
    /// </summary>
    public void Delete(int id)
    {
        Assignment? existId = Read(id);
        if (existId == null)
        {
            throw new DalDoesNotExistsException($"Assignment with ID={id} is not exists\n");
        }
        DataSource.Assignments.Remove(existId);
    }

    /// <summary>
    /// Deletes all assignments from the data source.
    /// </summary>
    public void DeleteAll()
    {
        if (DataSource.Assignments.Any())
        {
            DataSource.Assignments.Clear();
        }
    }

    /// <summary>
    /// Reads an assignment by its ID from the data source.
    /// </summary>
    public Assignment? Read(int id)
    {
        return DataSource.Assignments.FirstOrDefault(item => item.Id == id);
    }

    /// <summary>
    /// Reads an assignment that matches a specific filter from the data source.
    /// </summary>
    public Assignment? Read(Func<Assignment, bool> filter)
    {
        return DataSource.Assignments.FirstOrDefault(item => filter(item));
    }

    /// <summary>
    /// Reads all assignments from the data source, with an optional filter.
    /// </summary>
    public IEnumerable<Assignment> ReadAll(Func<Assignment, bool>? filter = null)
        => filter == null
            ? DataSource.Assignments.Select(item => item)
            : DataSource.Assignments.Where(filter);

    /// <summary>
    /// Updates an existing assignment in the data source.
    /// </summary>
    public void Update(Assignment item)
    {
        Assignment? existId = Read(item.Id);
        if (existId == null)
        {
            throw new DalDoesNotExistsException($"Assignment with ID={item.Id} is not exists\n");
        }
        DataSource.Assignments.Remove(existId);
        DataSource.Assignments.Add(item);
    }
}
