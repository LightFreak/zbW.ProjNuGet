using NUnit.Framework;
using zbW.ProjNuGet.ViewModel;

namespace zbW.ProjNuGetTest.Tests.Regex
{
    [TestFixture]
    class Regex_Phone_CH
    {
        // +41 79 361 61 61
        [Test]
        public void Phone_CH_Regex_Case1_Sucessfull()
        {
            //Assert
            var customerViewModel = new CustomerViewModel();
            var testValue = "+41 79 361 61 61";
            //Act
            var test = customerViewModel.Check_Phone(testValue,"ch");
            //Assert
            Assert.AreEqual(test, true);
        }

        [Test]
        public void Phone_CH_Regex_Case1_Sucessfull2()
        {
            //Assert
            var customerViewModel = new CustomerViewModel();
            var testValue = "+41793616161";
            //Act
            var test = customerViewModel.Check_Phone(testValue,"ch");
            //Assert
            Assert.AreEqual(test, true);
        }

        [Test]
        public void Phone_CH_Regex_Case1_Failed1()
        {
            //Assert
            var customerViewModel = new CustomerViewModel();
            var testValue = "41 79 361 61 61";
            //Act
            var test = customerViewModel.Check_Phone(testValue,"ch");
            //Assert
            Assert.AreEqual(false, test);
        }

        [Test]
        public void Phone_CH_Regex_Case1_Failed2()
        {
            //Assert
            var customerViewModel = new CustomerViewModel();
            var testValue = "++41 79 361 61 61";
            //Act
            var test = customerViewModel.Check_Phone(testValue,"ch");
            //Assert
            Assert.AreEqual(false, test);
        }

        [Test]
        public void Phone_CH_Regex_Case1_Failed3()
        {
            //Assert
            var customerViewModel = new CustomerViewModel();
            var testValue = "+41 9 361 61 61";
            //Act
            var test = customerViewModel.Check_Phone(testValue,"ch");
            //Assert
            Assert.AreEqual(false, test);
        }

        [Test]
        public void Phone_CH_Regex_Case1_Failed4()
        {
            //Assert
            var customerViewModel = new CustomerViewModel();
            var testValue = "+41 79 36 61 61";
            //Act
            var test = customerViewModel.Check_Phone(testValue,"ch");
            //Assert
            Assert.AreEqual(false, test);
        }

        [Test]
        public void Phone_CH_Regex_Case1_Failed5()
        {
            //Assert
            var customerViewModel = new CustomerViewModel();
            var testValue = "+41 79 361 6 61";
            //Act
            var test = customerViewModel.Check_Phone(testValue,"ch");
            //Assert
            Assert.AreEqual(false, test);
        }

        [Test]
        public void Phone_CH_Regex_Case1_Failed6()
        {
            //Assert
            var customerViewModel = new CustomerViewModel();
            var testValue = "+41 79 361 61 6";
            //Act
            var test = customerViewModel.Check_Phone(testValue,"ch");
            //Assert
            Assert.AreEqual(false, test);
        }

        // +41 (0) 79 361 61 61
        [Test]
        public void Phone_CH_Regex_Case2_Sucessfull()
        {
            var customerViewModel = new CustomerViewModel();
            var testValue = "+41 (0)79 361 61 61";
            //Act
            var test = customerViewModel.Check_Phone(testValue,"ch");
            //Assert
            Assert.AreEqual(test, true);
        }

        [Test]
        public void Phone_CH_Regex_Case2_Sucessfull2()
        {
            var customerViewModel = new CustomerViewModel();
            var testValue = "+41 (0)79 361 61 61";
            //Act
            var test = customerViewModel.Check_Phone(testValue,"ch");
            //Assert
            Assert.AreEqual(test, true);
        }

        [Test]
        public void Phone_CH_Regex_Case2_Failed()
        {
            var customerViewModel = new CustomerViewModel();
            var testValue = "+41 0) 79 361 61 61";
            //Act
            var test = customerViewModel.Check_Phone(testValue,"ch");
            //Assert
            Assert.AreEqual(false, test);
        }

        [Test]
        public void Phone_CH_Regex_Case2_Failed2()
        {
            var customerViewModel = new CustomerViewModel();
            var testValue = "+41 (0 79 361 61 61";
            //Act
            var test = customerViewModel.Check_Phone(testValue,"ch");
            //Assert
            Assert.AreEqual(false, test);
        }

        //0041 75 409 00 00
        [Test]
        public void Phone_CH_Regex_Case3_Sucessfull()
        {
            var customerViewModel = new CustomerViewModel();
            var testValue = "0042 79 361 61 61";
            //Act
            var test = customerViewModel.Check_Phone(testValue,"ch");
            //Assert
            Assert.AreEqual(test, true);
        }

        [Test]
        public void Phone_CH_Regex_Case3_Failed()
        {
            var customerViewModel = new CustomerViewModel();
            var testValue = "041 79 361 61 61";
            //Act
            var test = customerViewModel.Check_Phone(testValue,"ch");
            //Assert
            Assert.AreEqual(false, test);
        }

        [Test]
        public void Phone_CH_Regex_Case3_Failed2()
        {
            var customerViewModel = new CustomerViewModel();
            var testValue = "00041 79 361 61 61";
            //Act
            var test = customerViewModel.Check_Phone(testValue,"ch");
            //Assert
            Assert.AreEqual(false, test);
        }

        //0041 (0)75 409 00 00
        [Test]
        public void Phone_CH_Regex_Case4_Sucessfull()
        {
            var customerViewModel = new CustomerViewModel();
            var testValue = "0041 (0)79 361 61 61";
            //Act
            var test = customerViewModel.Check_Phone(testValue,"ch");
            //Assert
            Assert.AreEqual(true, test);
        }

        //075 409 00 00
        [Test]
        public void Phone_CH_Regex_Case5_Sucessfull()
        {
            var customerViewModel = new CustomerViewModel();
            var testValue = "079 361 61 61";
            //Act
            var test = customerViewModel.Check_Phone(testValue,"ch");
            //Assert
            Assert.AreEqual(test, true);
        }

        [Test]
        public void Phone_CH_Regex_Case5_Failed()
        {
            var customerViewModel = new CustomerViewModel();
            var testValue = "79 361 61 61";
            //Act
            var test = customerViewModel.Check_Phone(testValue,"ch");
            //Assert
            Assert.AreEqual(false, test);
        }

        [Test]
        public void Phone_CH_Regex_Case5_Failed2()
        {
            var customerViewModel = new CustomerViewModel();
            var testValue = "0079 361 61 61";
            //Act
            var test = customerViewModel.Check_Phone(testValue,"ch");
            //Assert
            Assert.AreEqual(false, test);
        }

        //075 / 409 00 00
        [Test]
        public void Phone_CH_Regex_Case6_Sucessfull()
        {
            var customerViewModel = new CustomerViewModel();
            var testValue = "079 / 361 61 61";
            //Act
            var test = customerViewModel.Check_Phone(testValue,"ch");
            //Assert
            Assert.AreEqual(test, true);
        }

        [Test]
        public void Phone_CH_Regex_Case6_Sucessfull2()
        {
            var customerViewModel = new CustomerViewModel();
            var testValue = "079/3616161";
            //Act
            var test = customerViewModel.Check_Phone(testValue,"ch");
            //Assert
            Assert.AreEqual(test, true);
        }

        [Test]
        public void Phone_CH_Regex_Case6_Failed()
        {
            var customerViewModel = new CustomerViewModel();
            var testValue = "079 // 361 61 61";
            //Act
            var test = customerViewModel.Check_Phone(testValue,"ch");
            //Assert
            Assert.AreEqual(false, test);
        }

        [Test]
        public void Phone_CH_Regex_Case7_Sucessfull()
        {
            var customerViewModel = new CustomerViewModel();
            var testValue = "0041793616183";
            //Act
            var test = customerViewModel.Check_Phone(testValue,"ch");
            //Assert
            Assert.AreEqual(test, true);
        }

        [Test]
        public void Phone_CH_Regex_Case8_Sucessfull()
        {
            var customerViewModel = new CustomerViewModel();
            var testValue = "0793616161";
            //Act
            var test = customerViewModel.Check_Phone(testValue,"ch");
            //Assert
            Assert.AreEqual(test, true);
        }

    }
}

//+41 75 409 00 00
//+41 (0)75 409 00 00
//0041 75 409 00 00
//0041 (0)75 409 00 00
//075 409 00 00
//075 / 409 00 00
//0793616183
//0041754090000