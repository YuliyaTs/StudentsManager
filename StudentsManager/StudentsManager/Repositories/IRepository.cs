using System.Collections.Generic;

namespace StudentsManager.Repositories
{
    public interface IRepository <T> where T : class
    {
        IList<T> GetAll(string newFileName = null);
        bool Save(IList<T> newCollection, string newFileName = null);
    }
}
