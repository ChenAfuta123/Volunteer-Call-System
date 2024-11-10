

using DalApi;
using DO;


namespace Dal;

public class CallImplementation : Icall
{
    public void Create(Call item)
    {
        //for entities with auto id
        int id = /*datasource*/Config.NextCallId;
        Call copy = item with { Id = id };
        DataSource.Calls.Add(copy);
    }


    public void Delete(int id)
    {
        Call? existId = Read(id);
        if (existId == null)
        {
            throw new NotImplementedException($"Call with ID={id} is not exists\n");
        }
        DataSource.Calls.Remove(existId);
    }

    public void DeleteAll()
    {
        DataSource.Calls.Clear();
    }

    public Call? Read(int id)
    {
        foreach (var item in DataSource.Calls)
        {
            if (item.Id == id)
            {
                return item;
            }
        }
        return null;
    }

    public List<Call> ReadAll()
    {
        return new List<Call>(DataSource.Calls);
    }

    public void Update(Call item)
    {

        Call? existId = Read(item.Id);
        if (existId==null)
        {
            throw new NotImplementedException($"Volunteer with ID={item.Id} is not exists\n");
        }
        DataSource.Calls.Remove(existId);
        DataSource.Calls.Add(item);
    }
}
