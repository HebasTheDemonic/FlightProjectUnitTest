using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlightProject;
using FlightProject.Facades;
using FlightProject.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace FlightProjectTest
{
    [TestClass]
    public class FlyingCenterSystemTest
    {
        private static FlyingCenterSystem flyingCenterSystem = FlyingCenterSystem.GetInstance();

        [TestInitialize]
        public void TestStart()
        {
            flyingCenterSystem.isTestMode = true;
            flyingCenterSystem.StartTest();
            flyingCenterSystem.isTestMode = false;
        }

        [TestMethod]
        public void GetInstance_TheradSafeCheck()
        {
            FlyingCenterSystem systemOne = null;
            FlyingCenterSystem systemTwo = null;

            Task.Run(() => { systemOne = FlyingCenterSystem.GetInstance(); });
            Task.Run(() => { systemTwo = FlyingCenterSystem.GetInstance(); });

            Thread.Sleep(50);

            Assert.IsNotNull(systemOne);
            Assert.AreEqual(systemOne, systemTwo);
        }

        [TestMethod]
        public void GetInstance_UserIsAny_LoginWithUnkownUsername_ThrowsUnregisteredUserException()
        {
            Assert.ThrowsException<UserNotFoundException>(new Action(() => UserLogin("moo", "9999")));
        }

        [TestMethod]
        public void GetInstance_UserIsAny_LoginWithWrongPassword_ThrowsWrongPasswordException()
        {
            Assert.ThrowsException<WrongPasswordException>(() => UserLogin("Admin", "000"));
        }

        [TestMethod]
        public void GetInstance_UserIsAdmin_ReturnsAdminFacade()
        {
            LoggedInAdministratorFacade administratorFacade = (LoggedInAdministratorFacade)UserLogin("Admin", "9999");

            Assert.IsInstanceOfType(administratorFacade, typeof(LoggedInAdministratorFacade));
        }

        [TestMethod]
        public void GetInstance_UserIsAirlineCompany_ReturnsAirlineCompanyFacade()
        {
            LoggedInAirlineFacade loggedInAirlineFacade = (LoggedInAirlineFacade)UserLogin("roo", "6546");

            Assert.IsInstanceOfType(loggedInAirlineFacade, typeof(LoggedInAirlineFacade));
        }

        [TestMethod]
        public void GetInstance_UserIsCustomer_ReturnsCustomerFacade()
        {
            LoggedInCustomerFacade loggedInCustomerFacade = (LoggedInCustomerFacade)UserLogin("coo", "6542");

            Assert.IsInstanceOfType(loggedInCustomerFacade, typeof(LoggedInCustomerFacade));
        }

        [TestMethod]
        public void GetFacade_UserIsAnonymous_FacadeExists()
        {
            FlyingCenterSystem flyingCenterSystem = FlyingCenterSystem.GetInstance();

            AnonymousUserFacade anonymousUser = (AnonymousUserFacade)FlyingCenterSystem.FacadeList[0];

            Assert.IsInstanceOfType(anonymousUser, typeof(AnonymousUserFacade));
        }

        [TestMethod]
        public void GetFacade_UserIsAdmin_LoginTokenNotNull()
        {
            LoggedInAdministratorFacade administratorFacade = (LoggedInAdministratorFacade)UserLogin("Admin", "9999");

            Assert.IsNotNull(administratorFacade.LoginToken);
        }

        [TestMethod]
        public void GetFacade_UserIsAirlineCompany_LoginTokenIsNotNull()
        {
            LoggedInAirlineFacade loggedInAirlineFacade = (LoggedInAirlineFacade)UserLogin("roo", "6546");

            Assert.IsNotNull(loggedInAirlineFacade);
        }

        [TestMethod]
        public void GetFacade_UserIsCustomer_LoginTokenIsNotNull()
        {
            LoggedInCustomerFacade loggedInCustomerFacade = (LoggedInCustomerFacade)UserLogin("coo", "6542");

            Assert.IsNotNull(loggedInCustomerFacade.LoginToken);
        }

        public FacadeBase UserLogin(string username, string password)
        {
            int facadeIndex = flyingCenterSystem.UserLogin(username, password);
            return FlyingCenterSystem.FacadeList[facadeIndex];
        }
        
    }
}
