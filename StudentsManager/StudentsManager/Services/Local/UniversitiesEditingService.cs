using System.Collections.Generic;
using System.Linq;
using StudentsManager.Model;

namespace StudentsManager.Services.Local
{
    public class UniversitiesEditingService : IEditingService<University>
    {
        public void Add(IList<University> universities, University newUniversity)
        {
            universities.Add(newUniversity);
        }

        public void Edit(IList<University> universities, University renewedUniversity)
        {
            var indexOfEditingUniversity = universities.IndexOf(universities.FirstOrDefault(university => university.Id == renewedUniversity.Id));

            universities[indexOfEditingUniversity] = renewedUniversity;
        }

        public void Delete(IList<University> universities, IList<University> universitiesForDeleting)
        {
            foreach (var student in universitiesForDeleting)
                universities.Remove(student);
        }
    }
}
