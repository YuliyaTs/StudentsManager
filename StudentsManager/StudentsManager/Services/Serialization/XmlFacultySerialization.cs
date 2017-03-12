using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using StudentsManager.Model;
using StudentsManager.Repositories;

namespace StudentsManager.Services.Serialization
{
    public class XmlFacultySerialization : IXmlSerialization<Faculty>
    {
        #region Fields

        private XDocument _document;
        private IRepository<University> _universityRepository; 

        #endregion

        #region Constructors

        public XmlFacultySerialization()
        {
            _document = new XDocument();
            _universityRepository = new XmlUniversitiesRepository();
        }

        #endregion

        #region Methods

        public object Deserialize(string newFileName = null)
        {
            var fileName = newFileName ?? Properties.Resources.FacultiesFileName;

            _document = XDocument.Load(fileName);
            var faculties = new List<Faculty>();

            if (_document != null && _document.Root.HasElements)
            {
                XElement root = _document.Element("Faculties");

                if (root != null && root.HasElements)
                {
                    foreach (var xmlFaculty in root.Elements("Faculty"))
                    {
                        var name = GetAttributeValueByName("Name", xmlFaculty);
                        var caption = GetAttributeValueByName("Caption", xmlFaculty);
                        var universityName = GetAttributeValueByName("University", xmlFaculty);
                        var id = GetAttributeValueByName("Id", xmlFaculty);

                        if (!IsCorrectFacultyName(name))
                            name = null;
                        if (name != null && !IsCorrectFacultyCaption(name, caption))
                            caption = null;

                        var university = _universityRepository.GetAll()
                                                              .FirstOrDefault(repoUniversity => repoUniversity.Name == universityName);

                        faculties.Add(new Faculty(new Guid(id), name, caption, university));
                    }
                }
            }

            return faculties;
        }

        public bool Save(IList<Faculty> faculties, string newFileName = null)
        {
            var fileName = newFileName ?? Properties.Resources.FacultiesFileName;

            _document = XDocument.Load(fileName);

            if (_document != null)
            {
                var root = _document.Element("Faculties");

                if (root != null)
                {
                    if (root.HasElements)
                        root.RemoveAll();

                    foreach (var faculty in faculties)
                    {
                        var newXFaculty = new XElement("Faculty");

                        newXFaculty.SetAttributeValue("Id", faculty.Id);
                        newXFaculty.SetAttributeValue("Name", faculty.Name);
                        newXFaculty.SetAttributeValue("Caption", faculty.Caption);
                        newXFaculty.SetAttributeValue("University", faculty.University.Name);

                        root.Add(newXFaculty);
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


        private static bool IsCorrectFacultyName(string name)
        {
            switch (name)
            {
                case "AMF":
                    return true;

                case "FRF":
                    return true;

                case "JF":
                    return true;

                case "EF":
                    return true;

                case "AF":
                    return true;

                case "CSF":
                    return true;

                case "PF":
                    return true;

                case "MF":
                    return true; ;

                case "PFF":
                    return true;

                case "DF":
                    return true;

                default:
                    return false;
            }
        }

        private static bool IsCorrectFacultyCaption(string name, string caption)
        {
            switch (name)
            {
                case "AMF":
                    if (caption == "Faculty of Applied Mathematics and Informatics")
                        return true;
                    return false;

                case "FRF":
                    if (caption == "Faculty of Foreign Relations")
                        return true;
                    return false;

                case "JF":
                    if (caption == "Faculty of Journalistics")
                        return true;
                    return false;

                case "EF":
                    if (caption == "Faculty of Economics")
                        return true;
                    return false;

                case "AF":
                    if (caption == "Faculty of Architecture")
                        return true;
                    return false;

                case "CSF":
                    if (caption == "Faculty of Computer Science")
                        return true;
                    return false;

                case "PF":
                    if (caption == "Faculty of Physics")
                        return true;
                    return false;

                case "MF":
                    if (caption == "Medical Faculty")
                        return true;
                    return false;

                case "PFF":
                    if (caption == "Pharmacy Faculty")
                        return true;
                    return false;

                case "DF":
                    if (caption == "Dentistry Faculty")
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
