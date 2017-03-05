using System;
using System.ComponentModel;

namespace StudentsManager.Model
{
    public class Student
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Student"/> class.
        /// </summary>
        public Student()
        {
            Id = new Guid();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Student"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="surname"></param>
        public Student(Guid id, string name, string surname)
        {
            Id = id;
            Name = name;
            Surname = surname;
        }

        #endregion

        #region Properties

        [Browsable(false)] 
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        [DisplayName(@"Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        public University University { get; set; }

        public Faculty Faculty { get; set; }

        public UInt16 Grade { get; set; }

        public string Notes { get; set; }

        [DisplayName(@"Has Scholarship")]
        public bool HasScholarship { get; set; }

        #endregion

    }
}
