using System.Collections.Generic;

namespace WebApplication2.Models.Repositories

{
    public interface IRepository<T>
    {
        T Get(int Id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Search(string term);
        T Add(T t);
        T Update(T t);
        T Delete(int Id);
    }
}
