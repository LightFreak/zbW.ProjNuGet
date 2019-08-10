using NUnit.Framework;
using zbW.ProjNuGet.ViewModel;

namespace zbW.ProjNuGetTest.Tests.Regex
{
    [TestFixture]
    class RegexMail
    {
        [Test]
        public void Rege_Mail_Case1_Sucess()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "abc@gg.cc";

            //Act
            var result = customerViewModel.Check_Mail(testValue);

            //Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Rege_Mail_Case1_Failed1()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "abc@gg";

            //Act
            var result = customerViewModel.Check_Mail(testValue);

            //Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Rege_Mail_Case1_Failed2()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "abcgg.com";

            //Act
            var result = customerViewModel.Check_Mail(testValue);

            //Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Rege_Mail_Case1_Failed3()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "abcgg.com";

            //Act
            var result = customerViewModel.Check_Mail(testValue);

            //Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Rege_Mail_Case1_Failed4()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "ab@cgg.c";

            //Act
            var result = customerViewModel.Check_Mail(testValue);

            //Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Rege_Mail_Case1_Failed5()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "abc@.com";

            //Act
            var result = customerViewModel.Check_Mail(testValue);

            //Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Rege_Mail_Case1_Failed6()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "abc@g@g.com";

            //Act
            var result = customerViewModel.Check_Mail(testValue);

            //Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Rege_Mail_Case1_Failed7()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "abc@gg..com";

            //Act
            var result = customerViewModel.Check_Mail(testValue);

            //Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Rege_Mail_Case2_Sucess()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "a.bc@gg.cc";

            //Act
            var result = customerViewModel.Check_Mail(testValue);

            //Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Rege_Mail_Case3_Sucess()
        {
            //Arrange
            var customerViewModel = new CustomerViewModel();
            var testValue = "abc@gg.ff.cc";

            //Act
            var result = customerViewModel.Check_Mail(testValue);

            //Assert
            Assert.AreEqual(true, result);
        }

    }

    
}
