using System;
using System.Collections;
using System.Collections.Generic;

namespace StudentsManager.Services.Serialization
{
    public interface IXmlSerialization <T> where T : class
    {
        object Deserialize(string newFileName = null);
        bool Save(IList<T> newCollection, string newFileName = null);
    }
}
