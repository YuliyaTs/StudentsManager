using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using StudentsManager.Model;
using StudentsManager.Repositories;

namespace StudentsManager.Services.Serialization
{
    public class XmlStudentSerialization : IXmlSerialization<Student>
    {
        #region Fields

        private readonly IRepository<University> _universityRepository;
        private readonly IRepository<Faculty> _facultyRepository;
        private XDocument _document;
        private DateTime minDateTime = new DateTime(1950, 1, 1);

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlStudentSerialization"/> class.
        /// </summary>
        public XmlStudentSerialization()
        {
            _document = new XDocument();
            _universityRepository = new XmlUniversitiesRepository();
            _facultyRepository = new XmlFacultiesRepository();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deserializes xml file and returns tha list of students.
        /// </summary>
        /// <returns></returns>
        public object Deserialize(string newFileName = null)
        {
            var fileName = newFileName ?? Properties.Resources.StudentsFileName;

            _document = XDocument.Load(fileName);

            var students = new List<Student>();

            if (_document != null)
            {
                var root = _document.Element("Students");

                if (root != null && root.HasElements)
                {
                    foreach (var xmlStudent in root.Elements("Student"))
                    {
                        var xName = xmlStudent.Element("Name");
                        var name = xName != null ? xName.Value : null;

                        var xSurname = xmlStudent.Element("Surname");
                        var surname = xSurname != null ? xSurname.Value : null;

                        var xDateOfBirth = xmlStudent.Element("DateOfBirth");
                        var dateOfBirth = xDateOfBirth != null ? xDateOfBirth.Value : null;

                        var xUniversity = xmlStudent.Element("University");
                        var universityName = xUniversity != null ? xUniversity.Value : null;

                        var xFaculty = xmlStudent.Element("Faculty");
                        var facultyName = xFaculty != null ? xFaculty.Value : null;

                        var xGrade = xmlStudent.Element("Grade");
                        var grade = xGrade != null ? xGrade.Value : null;

                        var xNotes = xmlStudent.Element("Notes");
                        var notes = xNotes != null ? xNotes.Value : null;

                        var xHasScholarship = xmlStudent.Element("HasScholarship");
                        var hasScholarship = xHasScholarship != null ? xHasScholarship.Value : null;

                        var xId = xmlStudent.Element("Id");
                        var id = xId != null ? xId.Value : null;


                        var student = new Student(new Guid(id), name, surname);

                        if (IsCorrectDate(dateOfBirth))
                            student.DateOfBirth = DateTime.ParseExact(dateOfBirth, "dd.MM.yyyy",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None);
                        else
                            student.DateOfBirth = minDateTime;


                        var university = _universityRepository.GetAll()
                                                              .FirstOrDefault(repoUniversity => repoUniversity.Name == universityName);

                        var faculty = _facultyRepository.GetAll()
                                                        .FirstOrDefault(repoFaculty => repoFaculty.Name == facultyName);

                        student.Faculty = faculty;
                        student.University = university;

                        if (IsGrade(grade))
                            student.Grade = UInt16.Parse(grade);

                        student.Notes = notes;

                        if (IsConvertibleToBool(hasScholarship))
                            student.HasScholarship = ConvertToBool(hasScholarship);

                        students.Add(student);

                    }
                }
            }

            return students;
        }


        /// <summary>
        /// Save new student collection to file.
        /// </summary>
        /// <param name="students">New collection of students</param>
        /// <param name="newFileName">New file name in case saving to new file.</param>
        /// <returns></returns>
        public bool Save(IList<Student> students, string newFileName = null)
        {
            var fileName = newFileName ?? Properties.Resources.StudentsFileName;

            _document = new XDocument();

            if (_document != null)
            {
                var root = new XElement("Students");


                if (root.HasElements)
                    root.RemoveAll();

                foreach (var student in students)
                {
                    var newXStudent = new XElement("Student");

                    newXStudent.Add(new XElement("Id", student.Id));
                    newXStudent.Add(new XElement("Name", student.Name));
                    newXStudent.Add(new XElement("Surname", student.Surname));
                    newXStudent.Add(new XElement("DateOfBirth", student.DateOfBirth.ToShortDateString()));

                    var universityName = "";
                    if (student.University != null)
                        universityName = student.University.Name;
                    newXStudent.Add(new XElement("University", universityName));

                    var facultyName = "";
                    if (student.Faculty != null)
                        facultyName = student.Faculty.Name;
                    newXStudent.Add(new XElement("Faculty", facultyName));

                    newXStudent.Add(new XElement("Grade", student.Grade.ToString()));

                    var notes = "";
                    if (student.Notes != null)
                        notes = student.Notes;
                    newXStudent.Add(new XElement("Notes", notes));

                    newXStudent.Add(new XElement("HasScholarship", student.HasScholarship ? "yes" : "no"));

                    root.Add(newXStudent);
                }

                _document.Add(root);
            }

            else
                return false;

            _document.Save(fileName);
            return true;

        }

        private static bool IsCorrectDate(string date)
        {
            DateTime convertedDate;

            if (DateTime.TryParseExact(date, "dd.MM.yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None, out convertedDate))
            {
                if (convertedDate.Year > 1950 && convertedDate.Year < 2000)
                    return true;
            }

            return false;
        }

        private static bool IsGrade(string grade)
        {
            UInt16 convertedGrade;

            if (UInt16.TryParse(grade, out convertedGrade))
            {
                if (convertedGrade > 0 && convertedGrade <= 6)
                    return true;
            }

            return false;
        }

        private static bool IsConvertibleToBool(string boolValue)
        {
            return boolValue == "yes" || boolValue == "no";
        }

        private static bool ConvertToBool(string boolValue)
        {
            return boolValue == "yes";
        }

        #endregion
    }
}
