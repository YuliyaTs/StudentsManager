using System;
using System.Collections.Generic;
using StudentsManager.Model;
using StudentsManager.Services.Serialization;

namespace StudentsManager.Repositories
{
    public class XmlStudentRepository : IRepository<Student>
    {
        #region Fields

        private readonly IXmlSerialization<Student> _serializer = new XmlStudentSerialization();
        private IList<Student> _data;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates new instance of <see cref="XmlStudentRepository"/> class.
        /// </summary>
        public XmlStudentRepository()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets students from xml file as list.
        /// </summary>
        /// <returns></returns>
        public IList<Student> GetAll(string newFileName = null)
        {
            _data = newFileName != null ? _serializer.Deserialize(newFileName) as List<Student> : _serializer.Deserialize() as List<Student>;

            return _data;
        }

        /// <summary>
        /// Returns deserialized list of studrnts.
        /// </summary>
        /// <returns></returns>
        public bool Save(IList<Student> newCollection, string newFileName = null)
        {
            return newFileName != null ? _serializer.Save(newCollection, newFileName) : _serializer.Save(newCollection);
        }

        #endregion
    }
}
