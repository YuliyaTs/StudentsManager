using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsManager.Model;

namespace StudentsManager.Services.Local
{
    public class FacultiesEditingService : IEditingService<Faculty>
    {
        public void Add(IList<Faculty> faculties, Faculty newFaculty)
        {
            faculties.Add(newFaculty);
        }

        public void Edit(IList<Faculty> faculties, Faculty renewedFaculty)
        {
            var indexOfEditingUniversity = faculties.IndexOf(faculties.FirstOrDefault(faculty => faculty.Id == renewedFaculty.Id));

            faculties[indexOfEditingUniversity] = renewedFaculty;
        }

        public void Delete(IList<Faculty> faculties, IList<Faculty> facultiesForDeleting)
        {
            foreach (var faculty in facultiesForDeleting)
                faculties.Remove(faculty);
        }
    }
}
