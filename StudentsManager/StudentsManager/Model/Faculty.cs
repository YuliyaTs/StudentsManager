using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace StudentsManager.Model
{
    [Serializable, XmlRoot(ElementName="Faculties")]
    public class Faculty
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Faculty"/> class.
        /// </summary>
        public Faculty()
        {
            Students = new List<Student>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Faculty"/> class.
        /// </summary>
        /// /// <param name="id">Id</param>
        /// <param name="name">Name</param>
        /// <param name="caption">Caption</param>
        /// <param name="university">University</param>
        public Faculty(Guid id, string name, string caption, University university)
        {
            Id = id;
            Name = name;
            Caption = caption;
            University = university;
            Students = new List<Student>();
        }

        #endregion

        #region Properties

        [Browsable(false)]
        public Guid Id { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Caption { get; set; }

        [XmlAttribute]
        [Browsable(false)]
        public University University { get; set; }

        [Browsable(false)]
        public IList<Student> Students { get; set; } 

        #endregion
    }
}
