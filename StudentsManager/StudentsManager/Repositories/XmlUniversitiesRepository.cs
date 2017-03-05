using System.Collections.Generic;
using StudentsManager.Model;
using StudentsManager.Services.Serialization;

namespace StudentsManager.Repositories
{
    public class XmlUniversitiesRepository : IRepository<University>
    {
        #region Fields

        private readonly IXmlSerialization<University> _serializer = new XmlUniversitySerialization();
        private IList<University> _data;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates new instance of <see cref="XmlStudentRepository"/> class.
        /// </summary>
        public XmlUniversitiesRepository()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<University> GetAll(string newFileName = null)
        {
            _data = newFileName != null ? _serializer.Deserialize(newFileName) as List<University> : _serializer.Deserialize() as List<University>;

            return _data;
        }

        /// <summary>
        /// Returns deserialized list of universities.
        /// </summary>
        /// <returns></returns>
        public bool Save(IList<University> newCollection, string newFileName = null)
        {
            return newFileName != null ? _serializer.Save(newCollection, newFileName) : _serializer.Save(newCollection);
        }

        #endregion
    }
}
