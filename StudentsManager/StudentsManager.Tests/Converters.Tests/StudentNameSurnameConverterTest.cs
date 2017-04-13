using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentsManager.Converters;

namespace StudentsManager.Tests.Converters.Tests
{
    /// <summary>
    /// Summary description for StudentNameSurnameConverterTest
    /// </summary>
    [TestClass]
    public class StudentNameSurnameConverterTest
    {
        [TestMethod]
        public void Convert_WithValidParameters_ReturnVasylynaMykhalchuk()
        {
            //Arrange
            var testBoolObject = new object[] { "Vasylyna", "Mykhalchuk" };
            var converter = new StudentNameSurnameConverter();
            var cultureInfo = System.Globalization.CultureInfo.CurrentCulture;
            var targetType = typeof(string);

            //Act
            var result = converter.Convert(testBoolObject, targetType, new object(), cultureInfo);

            //Assert
            Assert.AreEqual(result, "Vasylyna Mykhalchuk");
        }
    }
}
