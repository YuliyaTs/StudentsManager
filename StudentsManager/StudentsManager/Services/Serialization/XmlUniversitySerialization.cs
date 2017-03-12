using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using StudentsManager.Model;

namespace StudentsManager.Services.Serialization
{
    public class XmlUniversitySerialization : IXmlSerialization<University>
    {
        #region Fields

        private XDocument _document;

        #endregion

        #region Constructors

        public XmlUniversitySerialization()
        {
            _document = new XDocument();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deserializes xml file and gets list of universities.
        /// </summary>
        /// <returns></returns>
        public object Deserialize(string newFileName = null)
        {
            var fileName = newFileName ?? Properties.Resources.UniversitiesFileName;

            _document = XDocument.Load(fileName);
            var universities = new List<University>();

            if (_document != null && _document.Root.HasElements) 
            {
                XElement root = _document.Element("Universities");

                if (root != null && root.HasElements)
                {
                    foreach (var xmlUniversity in root.Elements("University"))
                    {
                        var name = GetAttributeValueByName("Name", xmlUniversity);
                        var caption = GetAttributeValueByName("Caption", xmlUniversity);
                        var id = GetAttributeValueByName("Id", xmlUniversity);

                        if (!IsCorrectUniversityName(name))
                            name = null;
                        if (!IsCorrectUniversityCaption(name, caption))
                            caption = null;

                        universities.Add(new University(new Guid(id), name, caption)); 
                    }
                }
            }

            return universities;
    }

        /// <summary>
        /// Writes to file new list of universities and save the file.
        /// </summary>
        /// <param name="universities"></param>
        /// <param name="newFileName"></param>
        /// <returns></returns>
        public bool Save(IList<University> universities, string newFileName = null)
        {
            var fileName = newFileName ?? Properties.Resources.UniversitiesFileName;

            _document = XDocument.Load(fileName);

            if (_document != null)
            {
                var root = _document.Element("Universities");

                if (root != null)
                {
                    if (root.HasElements)
                        root.RemoveAll();

                    foreach (var university in universities)
                    {
                        var newXUniversity = new XElement("University");

                        newXUniversity.SetAttributeValue("Id", university.Id);
                        newXUniversity.SetAttributeValue("Name", university.Name);
                        newXUniversity.SetAttributeValue("Caption", university.Caption);

                        root.Add(newXUniversity);
                    }
                }

                else
                    return false;
            }

            else
                return false;

            _document.Save(fileName);
            return true;
        }


        private static bool IsCorrectUniversityName(string name)
        {
            switch (name)
            {
                case "LNU":
                    return true;

                case "LNPU":
                    return true;

                case "LNMU":
                    return true;

                case "LCA":
                    return true;

                default:
                    return false;
            }
        }

         private static bool IsCorrectUniversityCaption(string name, string caption)
        {
            switch (name)
            {
                case "LNU":
                    if (caption == "Lviv Ivan Franko National University")
                        return true;
                    return false;

                case "LNPU":
                    if (caption == "Lviv National Polytechnic University")
                        return true;
                    return false;

                case "LNMU":
                    if (caption == "Lviv National Medical University")
                        return true;
                    return false;

                case "LCA":
                    if (caption == "Lviv Commercial Acadamy")
                        return true;
                    return false;

                default:
                    return false;
            }
        }

        private static string GetAttributeValueByName(string name, XElement xElement)
        {
            return xElement.Attributes().FirstOrDefault(x => x.Name == name).Value;
        }

        #endregion
    }
}
