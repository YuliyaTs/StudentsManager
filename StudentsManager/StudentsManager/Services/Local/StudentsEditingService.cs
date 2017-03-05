using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsManager.Model;

namespace StudentsManager.Services.Local
{
    public class StudentsEditingService : IEditingService<Student>
    {
        #region Methods

        public void Add(IList<Student> students, Student newStudent)
        {
            students.Add(newStudent);
        }

        public void Edit(IList<Student> students, Student renewedStudent)
        {
            var indexOfEditingStudent = students.IndexOf(students.FirstOrDefault(student => student.Id == renewedStudent.Id));

            students[indexOfEditingStudent] = renewedStudent;
        }

        public void Delete(IList<Student> students, IList<Student> studentsForDeleting)
        {
            foreach (var student in studentsForDeleting)
                students.Remove(student);
        }

        #endregion
    }
}
