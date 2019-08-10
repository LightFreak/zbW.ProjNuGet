using NUnit.Framework;
using zbW.ProjNuGet.ViewModel;

namespace zbW.ProjNuGetTest.Tests.Regex
{
    [TestFixture]
    class RegexWebsite
    {
        [Test]
        public void Website_Regex_Case1_Sucessfull()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "www.test.cc";

            //Act
            var result = customerViewModel.Check_Website(testValue);

            //Assert
            Assert.AreEqual(true, result);

        }

        [Test]
        public void Website_Regex_Case1_Failed1()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = ".test.cc";

            //Act
            var result = customerViewModel.Check_Website(testValue);

            //Assert
            Assert.AreEqual(false, result);

        }

        [Test]
        public void Website_Regex_Case1_Failed2()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "www..test.cc";

            //Act
            var result = customerViewModel.Check_Website(testValue);

            //Assert
            Assert.AreEqual(false, result);

        }

        [Test]
        public void Website_Regex_Case1_Failed3()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "www..cc";

            //Act
            var result = customerViewModel.Check_Website(testValue);

            //Assert
            Assert.AreEqual(false, result);

        }

        [Test]
        public void Website_Regex_Case1_Failed4()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "www.test..cc";

            //Act
            var result = customerViewModel.Check_Website(testValue);

            //Assert
            Assert.AreEqual(false, result);

        }

        [Test]
        public void Website_Regex_Case1_Failed5()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "www.test.cc.";

            //Act
            var result = customerViewModel.Check_Website(testValue);

            //Assert
            Assert.AreEqual(false, result);

        }

        [Test]
        public void Website_Regex_Case2_Sucessfull()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "regex.cc";

            //Act
            var result = customerViewModel.Check_Website(testValue);

            //Assert
            Assert.AreEqual(true, result);

        }

        [Test]
        public void Website_Regex_Case3_Sucessfull()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "http://www.test.cc";

            //Act
            var result = customerViewModel.Check_Website(testValue);

            //Assert
            Assert.AreEqual(true, result);

        }

        [Test]
        public void Website_Regex_Case3_Failed1()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "ttp://www.test.cc";

            //Act
            var result = customerViewModel.Check_Website(testValue);

            //Assert
            Assert.AreEqual(false, result);

        }

        [Test]
        public void Website_Regex_Case3_Failed2()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "htp://www.test.cc";

            //Act
            var result = customerViewModel.Check_Website(testValue);

            //Assert
            Assert.AreEqual(false, result);

        }

        [Test]
        public void Website_Regex_Case3_Failed3()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "htt://www.test.cc";

            //Act
            var result = customerViewModel.Check_Website(testValue);

            //Assert
            Assert.AreEqual(false, result);

        }

        [Test]
        public void Website_Regex_Case3_Failed4()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "http:/www.test.cc";

            //Act
            var result = customerViewModel.Check_Website(testValue);

            //Assert
            Assert.AreEqual(false, result);

        }

        [Test]
        public void Website_Regex_Case4_Sucessfull()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "https://www.test.cc";

            //Act
            var result = customerViewModel.Check_Website(testValue);

            //Assert
            Assert.AreEqual(true, result);

        }

        [Test]
        public void Website_Regex_Case5_Sucessfull()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "https://www.test.cc/abc%20mm#22m?ee";

            //Act
            var result = customerViewModel.Check_Website(testValue);

            //Assert
            Assert.AreEqual(true, result);

        }
    }
}
