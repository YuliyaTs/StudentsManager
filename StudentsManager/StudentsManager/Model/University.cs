using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace StudentsManager.Model
{
    [Serializable, XmlRoot(ElementName="Universities")]
    public class University
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="University"/> class.
        /// </summary>
        public University()
        {
            Faculties = new List<Faculty>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="University"/> class.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="name">Name</param>
        /// <param name="caption">Caption</param>
        public University(Guid id, string name, string caption)
        {
            Id = id;
            Name = name;
            Caption = caption;
            Faculties = new List<Faculty>();
        }

        #endregion

        #region Properties

        [Browsable(false)]
        public Guid Id { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Caption { get; set; }

        [Browsable(false)]
        public IList<Faculty> Faculties { get; set; }

        #endregion
    }
}
