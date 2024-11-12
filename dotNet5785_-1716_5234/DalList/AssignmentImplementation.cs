

using DO;
using DalApi;

namespace Dal;

public class AssignmentImplementation : IAssignment
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
            throw new Exception($"Assignment with ID={id} is not exists\n");
        }
        DataSource.Assignments.Remove(existId); 

    }

    public void DeleteAll()
    {
        DataSource.Assignments.Clear();
    }

    public Assignment? Read(int id)
    {
        foreach (var item in DataSource.Assignments)
        {
            if (item.Id == id)
            {
                return item;
            }
        }
       return null;
    }

    public List<Assignment> ReadAll()
    {
        return new List<Assignment>(DataSource.Assignments);
    }

    public void Update(Assignment item)
    {
        Assignment? existId= Read(item.Id);
        if (existId == null)
        {
            throw new Exception($"Assignment with ID={item.Id} is not exists\n");
        }
        DataSource.Assignments.Remove(existId);
        DataSource.Assignments.Add(item);
    }
}
