using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsManager.Repositories
{
    public interface IRepository <T> where T : class
    {
        IList<T> GetAll(string newFileName = null);
        bool Save(IList<T> newCollection, string newFileName = null);
    }
}
