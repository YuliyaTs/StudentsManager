using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentsManager.Converters;
using System.Globalization;

namespace StudentsManager.Tests.Converters.Tests
{
    [TestClass]
    public class BoolToStringConverterTest
    {
        [TestMethod]
        public void Convert_WithValidParameters_ReturnNo()
        {
            //Arrange
            var convertableObject = false;
            var targetType = typeof(string);
            var culture = CultureInfo.CurrentCulture;
            var testObject = new BoolToStringConverter();

            //Act
            var result = testObject.Convert(convertableObject, targetType, new object(), culture);

            //Assert
            Assert.AreEqual("no", result);
        }
    }
}
