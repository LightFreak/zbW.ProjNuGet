using NUnit.Framework;
using zbW.ProjNuGet.ViewModel;

namespace zbW.ProjNuGetTest.Tests.Regex
{
    [TestFixture]
    class RegexCustomerId
    {
        [Test]
        public void Customer_idRegexIstErfolgreich()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "CU12345";
            //Act
            var test = customerViewModel.Check_Customer_Id(testValue);
            //Assert
            Assert.AreEqual(test, true);
        }

        [Test]
        public void Customer_id_Regex_fail()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "C112345";
            //Act
            var test = customerViewModel.Check_Customer_Id(testValue);
            //Assert
            Assert.AreEqual(test, false);

        }

        [Test]
        public void Customer_id_Regex_fail2()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "CUA2345";
            //Act
            var test = customerViewModel.Check_Customer_Id(testValue);
            //Assert
            Assert.AreEqual(test, false);

        }

        [Test]
        public void Customer_id_Regex_fail3()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "CU1245";
            //Act
            var test = customerViewModel.Check_Customer_Id(testValue);
            //Assert
            Assert.AreEqual(test, false);

        }
    }
}
