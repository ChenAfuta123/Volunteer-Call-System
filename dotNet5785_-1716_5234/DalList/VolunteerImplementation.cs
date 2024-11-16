namespace DalList;

using Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class VolunteerImplementation : IVolunteer
{
   
        public void Create(Volunteer item)
        {
         Volunteer? existId = Read(item.Id);
            if (existId != null)
            {
                throw new Exception($"Volunteer with ID={item.Id} already exists\n");
            }


            DataSource.Volunteers.Add(item);
            //return item.Id;
        }

    public void Delete(int id)
    {
        Volunteer? existId = Read(id);
        if (existId == null)
        {
            throw new Exception($"Volunteer with ID={id} is not exists\n");
        }
        DataSource.Volunteers.Remove(existId);
    }

    public void DeleteAll()
    {
        DataSource.Volunteers.Clear();
    }

    public Volunteer? Read(int id)
    {
       return DataSource.Volunteers.FirstOrDefault(item => item.Id == id);
    }

    public List<Volunteer> ReadAll()
    {
        return new List<Volunteer>(DataSource.Volunteers);
    }

    public void Update(Volunteer item)
    {
        Volunteer? existId = Read(item.Id);
        if (existId == null)
        {
            throw new Exception($"Volunteer with ID={item.Id} is not exists\n");
        }
        DataSource.Volunteers.Remove(existId);
        DataSource.Volunteers.Add(item);
    }
}
