

using DalApi;
using DO;


namespace Dal;

public class CallImplementation : Icall
{
    public void Create(Call item)
    {
        int id = Config.NextCallId;
        Call temp = new Call();
        temp = item;
        temp.Id = id;
        DataSource.Calls.Add(temp);
        return id;
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public void DeleteAll()
    {
        throw new NotImplementedException();
    }

    public Call? Read(int id)
    {
        foreach (var item in DataSource.Calls)
        {
            if (item.Id == id)
            {
                return item; // החזרה של הפניה לאובייקט אם נמצא
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
        Call? ExistId = Read(item.Id);
        if (ExistId == null)
        {
            throw new NotImplementedException("An object of type Call with such an ID does not exist\n");
        }
        DataSource.Calls.Remove(ExistId);
        DataSource.Calls.Add(item);

    }
}
