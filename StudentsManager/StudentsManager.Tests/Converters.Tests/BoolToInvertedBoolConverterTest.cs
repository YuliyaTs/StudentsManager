using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentsManager.Converters;

namespace StudentsManager.Tests.Converters.Tests
{
    [TestClass]
    public class BoolToInvertedBoolConverterTest
    {
        [TestMethod]
        public void Convert_WithValidParameters_ReturnFalse()
        {
            //Arrange
            var testBoolObject = true;
            var converter = new BoolToInvertedBoolConverter();
            var cultureInfo = System.Globalization.CultureInfo.CurrentCulture;
            var targetType = typeof(bool);

            //Act
            var result = converter.Convert(testBoolObject, targetType, new object(), cultureInfo);

            //Assert
            Assert.AreEqual(result, false);
        }
    }
}
