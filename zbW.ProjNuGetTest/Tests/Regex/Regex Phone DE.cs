using NUnit.Framework;
using zbW.ProjNuGet.ViewModel;

namespace zbW.ProjNuGetTest.Tests.Regex
{
    [TestFixture]
    class Regex_Phone_DE
    {
        [Test]
        public void Phone_CH_Regex_Case1_Sucessfull()
        {
            //Assert
            var customerViewModel = new CustomerViewModel();
            var testValue = "+491739341284";
            //Act
            var test = customerViewModel.Check_Phone(testValue, "de");
            //Assert
            Assert.AreEqual(true, test);
        }

        [Test]
        public void Phone_CH_Regex_Case2_Sucessfull()
        {
            //Assert
            var customerViewModel = new CustomerViewModel();
            var testValue = "+49 1739341284";
            //Act
            var test = customerViewModel.Check_Phone(testValue, "de");
            //Assert
            Assert.AreEqual(true, test);
        }

        [Test]
        public void Phone_CH_Regex_Case3_Sucessfull()
        {
            //Assert
            var customerViewModel = new CustomerViewModel();
            var testValue = "(+49) 1739341284";
            //Act
            var test = customerViewModel.Check_Phone(testValue, "de");
            //Assert
            Assert.AreEqual(true, test);
        }

        [Test]
        public void Phone_CH_Regex_Case4_Sucessfull()
        {
            //Assert
            var customerViewModel = new CustomerViewModel();
            var testValue = "+49 17 39 34 12 84";
            //Act
            var test = customerViewModel.Check_Phone(testValue, "de");
            //Assert
            Assert.AreEqual(true, test);
        }

        [Test]
        public void Phone_CH_Regex_Case5_Sucessfull()
        {
            //Assert
            var customerViewModel = new CustomerViewModel();
            var testValue = "+49 (1739) 34 12 84";
            //Act
            var test = customerViewModel.Check_Phone(testValue, "de");
            //Assert
            Assert.AreEqual(true, test);
        }

        [Test]
        public void Phone_CH_Regex_Case6_Sucessfull()
        {
            //Assert
            var customerViewModel = new CustomerViewModel();
            var testValue = "+(49) (1739) 34 12 84";
            //Act
            var test = customerViewModel.Check_Phone(testValue, "de");
            //Assert
            Assert.AreEqual(true, test);
        }

        [Test]
        public void Phone_CH_Regex_Case7_Sucessfull()
        {
            //Assert
            var customerViewModel = new CustomerViewModel();
            var testValue = "+49 (1739) 34-12-84";
            //Act
            var test = customerViewModel.Check_Phone(testValue, "de");
            //Assert
            Assert.AreEqual(true, test);
        }

        [Test]
        public void Phone_CH_Regex_Case8_Sucessfull()
        {
            //Assert
            var customerViewModel = new CustomerViewModel();
            var testValue = "00491739341284";
            //Act
            var test = customerViewModel.Check_Phone(testValue, "de");
            //Assert
            Assert.AreEqual(true, test);
        }
    }
}
