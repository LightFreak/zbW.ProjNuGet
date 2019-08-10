using NUnit.Framework;
using zbW.ProjNuGet.ViewModel;

namespace zbW.ProjNuGetTest.Tests.Regex
{
    [TestFixture]
    class RegexPassword
    {
        //***
        // Das Passwort erfordert im Minimum:
        // - 8 Zeichen oder mehr
        // - Eines der foldengenden Sonderzeichen (!@#$&*)
        // - Minimal ein Grossbuchstaben
        // - Minimal eine Zahl
        // - Minimal 3 kleinbuchstaben
        //***

        [Test]
        public void Check_Password_Case1_Sucessfull()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "abC4!abc";

            //Act
            var result = customerViewModel.Check_Password(testValue);
            
            //Assert
            Assert.AreEqual(true,result);
        }

        [Test]
        public void Check_Password_Case1_Failed1()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "abC4!";

            //Act
            var result = customerViewModel.Check_Password(testValue);

            //Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Check_Password_Case1_Failed2()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "abC4dabc";

            //Act
            var result = customerViewModel.Check_Password(testValue);

            //Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Check_Password_Case1_Failed3()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "abc4!abc";

            //Act
            var result = customerViewModel.Check_Password(testValue);

            //Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Check_Password_Case1_Failed4()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "abCd!abc";

            //Act
            var result = customerViewModel.Check_Password(testValue);

            //Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Check_Password_Case1_Failed5()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "abC4!123";

            //Act
            var result = customerViewModel.Check_Password(testValue);

            //Assert
            Assert.AreEqual(false, result);
        }
    }
}
