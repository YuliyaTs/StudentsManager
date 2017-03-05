using System.Collections.Generic;
using StudentsManager.Model;
using StudentsManager.Services.Serialization;

namespace StudentsManager.Repositories
{
    public class XmlFacultiesRepository : IRepository<Faculty>
    {
        #region Fields

        private readonly IXmlSerialization<Faculty> _serializer = new XmlFacultySerialization();
        private IList<Faculty> _data;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates new instance of <see cref="XmlStudentRepository"/> class.
        /// </summary>
        public XmlFacultiesRepository()
        {
           
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns deserialized list of faculties.
        /// </summary>
        /// <returns></returns>
        public IList<Faculty> GetAll(string newFileName = null)
        {
            _data = newFileName != null ? _serializer.Deserialize(newFileName) as List<Faculty> : _serializer.Deserialize() as List<Faculty>;

            return _data;
        }

        public bool Save(IList<Faculty> newCollection, string newFileName = null)
        {
            return newFileName != null ? _serializer.Save(newCollection, newFileName) : _serializer.Save(newCollection);
        }

        #endregion
    }
}
