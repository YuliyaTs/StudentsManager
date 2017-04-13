using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentsManager.Converters;

namespace StudentsManager.Tests.Converters.Tests
{
    [TestClass]
    public class NumberToStringConverterTest
    {
        [TestMethod]
        public void Convert_WithValidParameters_ReturnSixth()
        {
            //Arrange
            var testBoolObject = 6;
            var converter = new NumberToStringConverter();
            var cultureInfo = System.Globalization.CultureInfo.CurrentCulture;
            var targetType = typeof(bool);

            //Act
            var result = converter.Convert(testBoolObject, targetType, new object(), cultureInfo);

            //Assert
            Assert.AreEqual(result, "Sixth");
        }
    }
}
