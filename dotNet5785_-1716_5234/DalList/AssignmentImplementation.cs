

using DO;
using DalApi;

namespace Dal;

internal class AssignmentImplementation : IAssignment
{

    public void Create(Assignment item)
    {


        int id = /*datasource.*/Config.NextAssignmentId;
        Assignment copy = item with { Id = id };
        DataSource.Assignments.Add(copy);
    }


    public void Delete(int id)
    {

        Assignment? existId = Read(id);
        if (existId == null)
        {
            throw new DalDoesNotExistsException($"Assignment with ID={id} is not exists\n");
        }
        DataSource.Assignments.Remove(existId);

    }

    public void DeleteAll()
    {
        DataSource.Assignments.Clear();
    }

    public Assignment? Read(int id)
    {
        return DataSource.Assignments.FirstOrDefault(item => item.Id == id);
    }
    public Assignment? Read(Func<Assignment, bool> filter)
    {
        return DataSource.Assignments.FirstOrDefault(item => filter(item));
    }


    public IEnumerable<Assignment> ReadAll(Func<Assignment, bool>? filter = null) //stage 2
     => filter == null
         ? DataSource.Assignments.Select(item => item)
            : DataSource.Assignments.Where(filter);
        

    public void Update(Assignment item)
    {
        Assignment? existId= Read(item.Id);
        if (existId == null)
        {
            throw new DalDoesNotExistsException($"Assignment with ID={item.Id} is not exists\n");
        }
        DataSource.Assignments.Remove(existId);
        DataSource.Assignments.Add(item);
    }
}
