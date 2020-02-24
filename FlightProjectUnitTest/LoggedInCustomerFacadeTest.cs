using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlightProject;
using FlightProject.Facades;
using FlightProject.POCOs;
using FlightProject.Exceptions;

namespace FlightProjectTest
{
    [TestClass]
    public class LoggedInCustomerFacadeTest
    {
        private static FlyingCenterSystem flyingCenterSystem = FlyingCenterSystem.GetInstance();

        [TestInitialize]
        public void TestInitializer()
        {
            flyingCenterSystem.isTestMode = true;
            flyingCenterSystem.StartTest();
            flyingCenterSystem.isTestMode = false;
        }

        [TestMethod]
        public void GetAllMyFlights_UserHasSeveralFlights_ReturnsListWithAllUsersFlights()
        {
            LoggedInCustomerFacade customerFacade = GetCustomerFacade("doo", "68421");

            List<Flight> flights = (List<Flight>)customerFacade.GetAllMyFlights(customerFacade.LoginToken);

            Assert.AreEqual(3, flights.Count);
            foreach (Flight flight in flights)
            {
                Assert.IsNotNull(flight);
            }
        }

        [TestMethod]
        public void GetAllMyFlights_UserHasOneFlight_ReturnsListWithAllUsersFlights()
        {
            LoggedInCustomerFacade customerFacade = GetCustomerFacade("goo", "684321");

            List<Flight> flights = (List<Flight>)customerFacade.GetAllMyFlights(customerFacade.LoginToken);

            Assert.AreEqual(1, flights.Count);
            foreach (Flight flight in flights)
            {
                Assert.IsNotNull(flight);
            }
        }

        [TestMethod]
        public void GetAllMyFlights_UserHasNoFlights_ReturnsNullList()
        {
            LoggedInCustomerFacade customerFacade = GetCustomerFacade("coo", "6542");

            List<Flight> flights = (List<Flight>)customerFacade.GetAllMyFlights(customerFacade.LoginToken);

            Assert.AreEqual(0, flights.Count);
        }

        [TestMethod]
        public void PurchaseTicket_ThereAreTicketsAvailable_ReturnsticketInfo()
        {
            LoggedInCustomerFacade customerFacade = GetCustomerFacade("doo", "68421");

            Ticket ticket = customerFacade.PurchaseTicket(customerFacade.LoginToken, 3);

            Assert.IsNotNull(ticket);
        }

        [TestMethod]
        public void PurchaseTicket_UserAleradyPurchasedTicket_ThrowsException()
        {
            LoggedInCustomerFacade customerFacade = GetCustomerFacade("doo", "68421");

            Assert.ThrowsException<UnauthorizedActionException>(new Action(() => { Ticket ticket = customerFacade.PurchaseTicket(customerFacade.LoginToken, 2); }));
        }

        [TestMethod]
        public void PurchaseTicket_NoAvailableTicketsOnFlight_ThrowsException()
        {
            LoggedInCustomerFacade customerFacade = GetCustomerFacade("doo","68421");

            Assert.ThrowsException<UnauthorizedActionException>(new Action(() => { Ticket ticket = customerFacade.PurchaseTicket(customerFacade.LoginToken, 5); }));
        }

        [TestMethod]
        public void CancelTicket_UserCancelsExistingTicket()
        {
            LoggedInCustomerFacade customerFacade = GetCustomerFacade("doo", "68421");
            Ticket ticket = new Ticket(2, customerFacade.LoginToken.User.Id);

            customerFacade.CancelTicket(customerFacade.LoginToken, ticket);

        }

        [TestMethod]
        public void CancelTicket_UserTriesToCancelNonExistantTicket_ThrowsException()
        {
            LoggedInCustomerFacade customerFacade = GetCustomerFacade("coo", "6542");
            Ticket ticket = new Ticket(4, customerFacade.LoginToken.User.Id);

            Assert.ThrowsException<NullReferenceException>(new Action(() => {customerFacade.CancelTicket(customerFacade.LoginToken, ticket); }));
        }

        public LoggedInCustomerFacade GetCustomerFacade(string username, string password)
        {
            int facadeIndex = flyingCenterSystem.UserLogin(username, password);
            LoggedInCustomerFacade customerFacade = (LoggedInCustomerFacade)FlyingCenterSystem.FacadeList[facadeIndex];
            return customerFacade;
        }

    }
}
