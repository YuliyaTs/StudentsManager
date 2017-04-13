using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentsManager.Converters;

namespace StudentsManager.Tests.Converters.Tests
{
    [TestClass]
    public class ListViewWidthConverterTest
    {
        [TestMethod]
        public void Convert_WithValidParameters_Return520()
        {
            //Arrange
            var testBoolObject = 720.0;
            var converter = new ListViewWidthConverter();
            var cultureInfo = System.Globalization.CultureInfo.CurrentCulture;
            var targetType = typeof(double);

            //Act
            var result = converter.Convert(testBoolObject, targetType, new object(), cultureInfo);

            //Assert
            Assert.AreEqual(result, 520.0);
        }
    }
}
