using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlightProject;
using FlightProject.Facades;
using FlightProject.POCOs;
using FlightProject.Exceptions;
using System.Data.SqlClient;

namespace FlightProjectTest
{
    [TestClass]
    public class LoggedInAdministratorFacadeTest
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
        public void CreateNewAirline_AllParametersLegal()
        {
            LoggedInAdministratorFacade administratorFacade = GetAdministratorFacade("Admin","9999");
            AirlineCompany airlineCompany = new AirlineCompany("zem", "tek", "5558", 6);

            administratorFacade.CreateNewAirline(administratorFacade.LoginToken, airlineCompany);
            int facadeIndex = UserLogin("tek", "5558");
            LoggedInAirlineFacade airlineFacade = (LoggedInAirlineFacade)FlyingCenterSystem.FacadeList[facadeIndex];

            Assert.IsNotNull(airlineFacade.LoginToken);
        }

        [TestMethod]
        public void CreateNewAirline_NonExistantCountryCode_ThrowsException()
        {
            LoggedInAdministratorFacade administratorFacade = GetAdministratorFacade("Admin", "9999");
            AirlineCompany airlineCompany = new AirlineCompany("zem", "tek", "5558", 9);

            Assert.ThrowsException<SqlException>(new Action(() => administratorFacade.CreateNewAirline(administratorFacade.LoginToken, airlineCompany)));
        }

        [TestMethod]
        public void CreateNewAirline_UsernameAlreadybelongsToAnother_ThrowsException()
        {
            LoggedInAdministratorFacade administratorFacade = GetAdministratorFacade("Admin", "9999");
            AirlineCompany airlineCompany = new AirlineCompany("zem", "coo", "5558", 6);

            Assert.ThrowsException<UserAlreadyExistsException>(new Action(() => { administratorFacade.CreateNewAirline(administratorFacade.LoginToken, airlineCompany); }));
        }

        [TestMethod]
        public void CreateNewCustomer_AllParametersLegal()
        {
            LoggedInAdministratorFacade administratorFacade = GetAdministratorFacade("Admin", "9999");
            Customer customer = new Customer("foo", "goo", "bek", "555", "ter", 66464, 654321);

            administratorFacade.CreateNewCustomer(administratorFacade.LoginToken, customer);
            int facadeIndex = UserLogin("bek", "555");
            LoggedInCustomerFacade customerFacade = (LoggedInCustomerFacade)FlyingCenterSystem.FacadeList[facadeIndex];

            Assert.IsNotNull(customerFacade.LoginToken);
        }

        [TestMethod]
        public void CreateNewCustomer_UsernameAlreadybelongsToAnother_ThrowsException()
        {
            LoggedInAdministratorFacade administratorFacade = GetAdministratorFacade("Admin", "9999");
            Customer customer = new Customer("foo", "goo", "coo", "555", "ter", 66464, 654321);

            Assert.ThrowsException<UserAlreadyExistsException>(new Action(() => administratorFacade.CreateNewCustomer(administratorFacade.LoginToken, customer)));
        }

        [TestMethod]
        public void CreateNewAdministrator_UserIsHeadAdmin_RegisterAdmin()
        {
            LoggedInAdministratorFacade administratorFacade = GetAdministratorFacade("Admin", "9999");
            Administrator administrator = new Administrator("tem", "888");

            administratorFacade.CreateNewAdministrator(administratorFacade.LoginToken, administrator);
            int facadeIndex = UserLogin("tem", "888");
            LoggedInAdministratorFacade newAdministratorFacade = (LoggedInAdministratorFacade)FlyingCenterSystem.FacadeList[facadeIndex];

            Assert.AreEqual("tem", newAdministratorFacade.LoginToken.User.Username);
            Assert.IsNotNull(newAdministratorFacade.LoginToken.User.Id);
        }

        [TestMethod]
        public void CreateNewAdministrator_UserIsRegularAdmin_ThrowsException()
        {
            LoggedInAdministratorFacade administratorFacade = GetAdministratorFacade("boo", "1111");
            Administrator administrator = new Administrator("tem", "888");

            Assert.ThrowsException<UnauthorizedActionException>(new Action(() => administratorFacade.CreateNewAdministrator(administratorFacade.LoginToken, administrator)));
        }

        [TestMethod]
        public void CreateNewAdministrator_UserIsHeadAdmin_UsernameAleradyExists_ThrowException()
        {
            LoggedInAdministratorFacade administratorFacade = GetAdministratorFacade("Admin", "9999");
            Administrator administrator = new Administrator("boo", "888");

            Assert.ThrowsException<UserAlreadyExistsException>(new Action(() => administratorFacade.CreateNewAdministrator(administratorFacade.LoginToken, administrator)));
        }

        [TestMethod]
        public void RemoveAirline_RemoveRequestForExistingAirline_LoginAttempThrowsException()
        {
            LoggedInAdministratorFacade administratorFacade = GetAdministratorFacade("Admin", "9999");
            AirlineCompany airline = new AirlineCompany("roo", "6546");

            administratorFacade.RemoveAirline(administratorFacade.LoginToken, airline);

            Assert.ThrowsException<UserNotFoundException>(new Action(() => UserLogin("roo", "6546")));
        }

        [TestMethod]
        public void RemoveAirline_RemoveRequestForNonExistingAirline_ThrowsException()
        {
            LoggedInAdministratorFacade administratorFacade = GetAdministratorFacade("Admin", "9999");
            AirlineCompany airline = new AirlineCompany("bek", "7788");

            Assert.ThrowsException<UserNotFoundException>(new Action(() => { administratorFacade.RemoveAirline(administratorFacade.LoginToken, airline); }));
        }

        [TestMethod]
        public void RemoveCustomer_RemoveRequestForExistingCustomer_LoginAttemptThrowsException()
        {
            LoggedInAdministratorFacade administratorFacade = GetAdministratorFacade("Admin", "9999");
            Customer customer = new Customer("coo", "6542");

            administratorFacade.RemoveCustomer(administratorFacade.LoginToken, customer);

            Assert.ThrowsException<UserNotFoundException>(new Action(() => { UserLogin("coo", "6542"); }));
        }

        [TestMethod]
        public void RemoveCustomer_RemoveRequestForNonExistingCustomer_ThrowsException()
        {
            LoggedInAdministratorFacade administratorFacade = GetAdministratorFacade("Admin", "9999");
            Customer customer = new Customer("bek", "1231");

            Assert.ThrowsException<UserNotFoundException>(new Action(() => administratorFacade.RemoveCustomer(administratorFacade.LoginToken, customer)));
        }

        [TestMethod]
        public void RemoveAdministrator_TryingToRemoveRegularAdmin_LoginAttemptThrowsException()
        {
            LoggedInAdministratorFacade administratorFacade = GetAdministratorFacade("Admin", "9999");
            Administrator administrator = new Administrator("boo", "1111");

            administratorFacade.RemoveAdministrator(administratorFacade.LoginToken, administrator);

            Assert.ThrowsException<UserNotFoundException>(new Action(() => UserLogin("boo", "1111")));
        }

        [TestMethod]
        public void RemoveAdministrator_TryingToRemoveHeadAdministrator_ThrowsException()
        {
            LoggedInAdministratorFacade administratorFacade = GetAdministratorFacade("boo", "1111");
            Administrator administrator = new Administrator("Admin", "9999");

            Assert.ThrowsException<UnauthorizedActionException>(new Action(() => administratorFacade.RemoveAdministrator(administratorFacade.LoginToken, administrator)));
        }

        [TestMethod]
        public void RemoveAdministrator_UserTryingToRemoveSelf_ThrowsException()
        {
            LoggedInAdministratorFacade administratorFacade = GetAdministratorFacade("boo", "1111");
            Administrator administrator = new Administrator("boo", "1111");

            Assert.ThrowsException<UnauthorizedActionException>(new Action(() => administratorFacade.RemoveAdministrator(administratorFacade.LoginToken, administrator)));
        }

        [TestMethod]
        public void UpdateAdministrator_TokenUsernameIsAdmin_LoginAttemptWithNewInfoSucceeds()
        {
            LoggedInAdministratorFacade administratorFacade = GetAdministratorFacade("Admin", "9999");
            Administrator administrator = new Administrator("boo", "1212");

            administratorFacade.UpdateAdministrator(administratorFacade.LoginToken, administrator);
            int facadeIndex = UserLogin("boo", "1212");
            LoggedInAdministratorFacade loggedInAdministratorFacade = (LoggedInAdministratorFacade)FlyingCenterSystem.FacadeList[facadeIndex];

            Assert.AreEqual(administrator.Password, loggedInAdministratorFacade.LoginToken.User.Password);
        }

        [TestMethod]
        public void UpdateAdministrator_TokenUsernameIsNotAdmin_ThrowsException()
        {
            LoggedInAdministratorFacade administratorFacade = GetAdministratorFacade("boo", "1111");
            Administrator administrator = new Administrator("fuo", "8898");

            Assert.ThrowsException<UnauthorizedActionException>(new Action(() => administratorFacade.UpdateAdministrator(administratorFacade.LoginToken, administrator)));
        }

        [TestMethod]
        public void UpdateAirline_AirlineFound_LoginAttemptWithNewInfoSucceeds()
        {
            LoggedInAdministratorFacade administratorFacade = GetAdministratorFacade("Admin", "9999");
            AirlineCompany airlineCompany = new AirlineCompany("tek", "roo", "5558", 6);

            administratorFacade.UpdateAirlineDetails(administratorFacade.LoginToken, airlineCompany);
            int facadeIndex = UserLogin("roo", "5558");
            LoggedInAirlineFacade airlineFacade = (LoggedInAirlineFacade)FlyingCenterSystem.FacadeList[facadeIndex];

            Assert.AreEqual(airlineCompany.AirlineName, airlineFacade.LoginToken.User.AirlineName);
        }

        [TestMethod]
        public void UpdateAirline_AirlineNotFound()
        {
            LoggedInAdministratorFacade administratorFacade = GetAdministratorFacade("Admin", "9999");
            AirlineCompany airlineCompany = new AirlineCompany("tek", "brek", "5558", 6);

            Assert.ThrowsException<UnauthorizedActionException>(new Action(() => administratorFacade.UpdateAirlineDetails(administratorFacade.LoginToken, airlineCompany)));
        }

        [TestMethod]
        public void UpdateCustomer_CustomerFound()
        {
            LoggedInAdministratorFacade administratorFacade = GetAdministratorFacade("admin", "9999");
            Customer customer = new Customer("foo", "goo", "coo", "555", "ter", 66464, 654321);

            administratorFacade.UpdateCustomerDetails(administratorFacade.LoginToken, customer);
            int facadeIndex = UserLogin("coo", "555");
            LoggedInCustomerFacade customerFacade = (LoggedInCustomerFacade)FlyingCenterSystem.FacadeList[facadeIndex];

            Assert.AreEqual(customer.Address, customerFacade.LoginToken.User.Address);
            Assert.AreEqual(customer.CreditCardNumber, customerFacade.LoginToken.User.CreditCardNumber);
            Assert.AreEqual(customer.FirstName, customerFacade.LoginToken.User.FirstName);
            Assert.AreEqual(customer.LastName, customerFacade.LoginToken.User.LastName);
            Assert.AreEqual(customer.Password, customerFacade.LoginToken.User.Password);
            Assert.AreEqual(customer.PhoneNo, customerFacade.LoginToken.User.PhoneNo);
        }

        [TestMethod]
        public void UpdateCustomer_CustomerNotFound_ThrowsException()
        {
            LoggedInAdministratorFacade administratorFacade = GetAdministratorFacade("admin", "9999");
            Customer customer = new Customer("foo", "goo", "poo", "555", "ter", 66464, 654321);

            Assert.ThrowsException<UnauthorizedActionException>(new Action(() => administratorFacade.UpdateCustomerDetails(administratorFacade.LoginToken, customer)));
        }

        public LoggedInAdministratorFacade GetAdministratorFacade(string username, string password)
        {
            int facadeIndex = UserLogin(username, password);
            LoggedInAdministratorFacade administratorFacade = (LoggedInAdministratorFacade)FlyingCenterSystem.FacadeList[facadeIndex];
            return administratorFacade;
        }

        public int UserLogin(string username, string password)
        {
            FlyingCenterSystem flyingCenterSystem = FlyingCenterSystem.GetInstance();
            return flyingCenterSystem.UserLogin(username, password);
        }
    }
}
