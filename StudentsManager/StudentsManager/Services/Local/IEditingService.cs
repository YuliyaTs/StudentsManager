using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsManager.Services.Local
{
    interface IEditingService <T> where T : class
    {
        void Add(IList<T> collection, T newInstance);
        void Edit(IList<T> collection, T renewedInstance);
        void Delete(IList<T> collection,IList<T> instancesForDeleting);
    }
}
