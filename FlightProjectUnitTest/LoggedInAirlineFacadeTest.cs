using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlightProject;
using FlightProject.Facades;
using FlightProject.POCOs;
using FlightProject.Exceptions;
using System.Collections.Generic;

namespace FlightProjectTest
{
    [TestClass]
    public class LoggedInAirlineFacadeTest
    {
        private static FlyingCenterSystem flyingCenterSystem = FlyingCenterSystem.GetInstance();

        [TestInitialize]
        public void TestStart()
        {
            flyingCenterSystem.isTestMode = true;
            flyingCenterSystem.StartTest();
            flyingCenterSystem.isTestMode = false;
        }

        // This Test Also shows that giving a non-existant flight ID returns an exception

        [TestMethod]
        public void CancelFlight_FlightFound()
        {
            LoggedInAirlineFacade airlineFacade = GetAirlineFacade(LoggedInAirlineFacadeTest_Constants.CancelFlight_FlightFound_LOGINTOKEN_USERNAME,
                                                                   LoggedInAirlineFacadeTest_Constants.CancelFlight_FlightFound_LOGINTOKEN_PASSWORD);
            int flightId = LoggedInAirlineFacadeTest_Constants.CancelFlight_FlightFound_FLIGHT_ID;

            airlineFacade.CancelFlight(airlineFacade.LoginToken, flightId);

            Assert.ThrowsException<NullReferenceException>(new Action(() => airlineFacade.CancelFlight(airlineFacade.LoginToken, flightId)));
        }

        [TestMethod]
        public void CancelFlight_FlightBelongsToAnotherUser_ThrowsException()
        {
            LoggedInAirlineFacade airlineFacade = GetAirlineFacade(LoggedInAirlineFacadeTest_Constants.CancelFlight_FlightBelongsToAnotherUser_ThrowsException_LOGINTOKEN_USERNAME,
                                                                   LoggedInAirlineFacadeTest_Constants.CancelFlight_FlightBelongsToAnotherUser_ThrowsException_LOGINTOKEN_PASSWORD);
            int flightId = LoggedInAirlineFacadeTest_Constants.CancelFlight_FlightBelongsToAnotherUser_ThrowsException_FLIGHT_ID;

            Assert.ThrowsException<UnauthorizedActionException>(new Action(() => airlineFacade.CancelFlight(airlineFacade.LoginToken, flightId)));
        }

        //This test relies on GetAllFlights working properly

        [TestMethod]
        private void CreateFlight_NewFlight_FlagRaisedIfListContainsCreatedFlight()
        {
            LoggedInAirlineFacade airlineFacade = GetAirlineFacade(LoggedInAirlineFacadeTest_Constants.CreateFlight_NewFlight_FlagDropsIfListContainsCreatedFlight_LOGINTOKEN_USERNAME,
                                                                   LoggedInAirlineFacadeTest_Constants.CreateFlight_NewFlight_FlagDropsIfListContainsCreatedFlight_LOGINTOKEN_PASSWORD);
            Flight flight = LoggedInAirlineFacadeTest_Constants.CreateFlight_NewFlight_FlagDropsIfListContainsCreatedFlight_NEW_FLIGHT;
            bool flag = false;

            airlineFacade.CreateFlight(airlineFacade.LoginToken, flight);
            List<Flight> flights = (List<Flight>)airlineFacade.GetAllFlightsByAirline(airlineFacade.LoginToken);
            foreach (Flight listFlight in flights)
            {
                if(listFlight.AirlineCompanyId == flight.AirlineCompanyId &&
                   listFlight.DepartureTime == flight.DepartureTime &&
                   listFlight.DestinationCountryId == flight.DestinationCountryId &&
                   listFlight.LandingTime == flight.LandingTime &&
                   listFlight.OriginCountryId == flight.OriginCountryId)
                {
                    flag = true;
                }
            }

            Assert.IsTrue(flag);
        }


        [TestMethod]
        public void CreateFlight_FlightAlreadyExists_ThrowsException()
        {
            LoggedInAirlineFacade airlineFacade = GetAirlineFacade(LoggedInAirlineFacadeTest_Constants.CreateFlight_FlightAlreadyExists_ThrowsException_LOGINTOKEN_USERNAME,
                                                                   LoggedInAirlineFacadeTest_Constants.CreateFlight_FlightAlreadyExists_ThrowsException_LOGINTOKEN_PASSWORD);
            Flight flight = LoggedInAirlineFacadeTest_Constants.CreateFlight_FlightAlreadyExists_ThrowsException_EXISTING_FLIGHT;

            Assert.ThrowsException<DataAlreadyExistsException>(new Action(() => airlineFacade.CreateFlight(airlineFacade.LoginToken, flight)));
        }

        //This test relies on GetAllFlights working properly

        [TestMethod]
        public void UpdateFlight_FlightFound_MakesRequestedChanges()
        {
            LoggedInAirlineFacade airlineFacade = GetAirlineFacade(LoggedInAirlineFacadeTest_Constants.UpdateFlight_FlightFound_MakesRequestedChanges_LOGINTOKEN_USERNAME,
                                                                   LoggedInAirlineFacadeTest_Constants.UpdateFlight_FlightFound_MakesRequestedChanges_LOGINTOKEN_PASSWORD);
            Flight updatedFlight = LoggedInAirlineFacadeTest_Constants.UpdateFlight_FlightFound_MakesRequestedChanges_UPDATED_FLIGHT_DETAILS;
            int flightId = LoggedInAirlineFacadeTest_Constants.UpdateFlight_FlightFound_MakesRequestedChanges_FLIGHT_ID;
            bool flag = false;

            airlineFacade.UpdateFlight(airlineFacade.LoginToken, flightId, updatedFlight);
            List<Flight> flights = (List<Flight>)airlineFacade.GetAllFlightsByAirline(airlineFacade.LoginToken);
            foreach (Flight listFlight in flights)
            {
                if (listFlight.DepartureTime == updatedFlight.DepartureTime &&
                    listFlight.LandingTime == updatedFlight.LandingTime &&
                   listFlight.TotalTickets == updatedFlight.TotalTickets)
                {
                    flag = true;
                }
            }

            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void UpdateFlight_FlightNotFound_ThrowsException()
        {
            LoggedInAirlineFacade airlineFacade = GetAirlineFacade(LoggedInAirlineFacadeTest_Constants.UpdateFlight_FlightNotFound_ThrowsException_LOGINTOKEN_USERNAME,
                                                                   LoggedInAirlineFacadeTest_Constants.UpdateFlight_FlightNotFound_ThrowsException_LOGINTOKEN_PASSWORD);
            Flight flight = LoggedInAirlineFacadeTest_Constants.UpdateFlight_FlightNotFound_ThrowsException_UPDATED_FLIGHT_DETAILS;
            int flightId = LoggedInAirlineFacadeTest_Constants.UpdateFlight_FlightNotFound_ThrowsException_FLIGHT_ID;

            Assert.ThrowsException<NullReferenceException>(new Action(() => airlineFacade.UpdateFlight(airlineFacade.LoginToken, flightId, flight)));
        }

        [TestMethod]
        public void ChangeMyPassword_CorrectPasswordEntered_LoginTokenHasNewPassword()
        {
            LoggedInAirlineFacade airlineFacade = GetAirlineFacade(LoggedInAirlineFacadeTest_Constants.ChangeMyPassword_CorrectPasswordEntered_ThrowsExceptionWhenOldPasswordEntered_LOGINTOKEN_USERNAME,
                                                                   LoggedInAirlineFacadeTest_Constants.ChangeMyPassword_CorrectPasswordEntered_ThrowsExceptionWhenOldPasswordEntered_LOGINTOKEN_PASSWORD);
            string newPassword = LoggedInAirlineFacadeTest_Constants.ChangeMyPassword_CorrectPasswordEntered_ThrowsExceptionWhenOldPasswordEntered_NEW_PASSWORD;
            string oldPassword = LoggedInAirlineFacadeTest_Constants.ChangeMyPassword_CorrectPasswordEntered_ThrowsExceptionWhenOldPasswordEntered_LOGINTOKEN_PASSWORD;

            airlineFacade.ChangeMyPassword(airlineFacade.LoginToken, oldPassword, newPassword);

            Assert.AreEqual(newPassword, airlineFacade.LoginToken.User.Password);
        }

        [TestMethod]
        public void ChangeMyPassword_WrongPasswordEntered_ThrowsException()
        {
            LoggedInAirlineFacade airlineFacade = GetAirlineFacade(LoggedInAirlineFacadeTest_Constants.ChangeMyPassword_WrongPasswordEntered_ThrowsException_LOGINTOKEN_USERNAME,
                                                                   LoggedInAirlineFacadeTest_Constants.ChangeMyPassword_WrongPasswordEntered_ThrowsException_LOGINTOKEN_PASSWORD);
            string oldPassword = LoggedInAirlineFacadeTest_Constants.ChangeMyPassword_WrongPasswordEntered_ThrowsException_WRONG_OLD_PASSWORD;
            string newPassword = LoggedInAirlineFacadeTest_Constants.ChangeMyPassword_WrongPasswordEntered_ThrowsException_NEW_PASSWORD;

            Assert.ThrowsException<WrongPasswordException>(new Action(() => airlineFacade.ChangeMyPassword(airlineFacade.LoginToken, oldPassword, newPassword)));
        }

        [TestMethod]
        public void ModifyAirlineCompany_CorrectPasswordEntered_LoginTokenContainsUpdatedInfo()
        {
            LoggedInAirlineFacade airlineFacade = GetAirlineFacade(LoggedInAirlineFacadeTest_Constants.ModifyAirlineCompany_CorrectPasswordEntered_LoginTokenContainsUpdatedInfo_LOGINTOKEN_USERNAME,
                                                                   LoggedInAirlineFacadeTest_Constants.ModifyAirlineCompany_CorrectPasswordEntered_LoginTokenContainsUpdatedInfo_LOGINTOKEN_PASSWORD);
            AirlineCompany airlineCompany = LoggedInAirlineFacadeTest_Constants.ModifyAirlineCompany_CorrectPasswordEntered_LoginTokenContainsUpdatedInfo_UPDATED_AIRLINE_INFO;

            airlineFacade.ModifyAirlineDetails(airlineFacade.LoginToken, airlineCompany);

            Assert.AreEqual(airlineCompany.AirlineName, airlineFacade.LoginToken.User.AirlineName);
            Assert.AreEqual(airlineCompany.OriginCountry, airlineFacade.LoginToken.User.OriginCountry);
        }

        [TestMethod]
        public void ModifyAirlineCompany_UnregisteredCountryCodeEntered_ThrowsException()
        {
            LoggedInAirlineFacade airlineFacade = GetAirlineFacade(LoggedInAirlineFacadeTest_Constants.ModifyAirlineCompany_UnregisteredCountryCodeEntered_ThrowsException_LOGINTOKEN_USERNAME,
                                                                  LoggedInAirlineFacadeTest_Constants.ModifyAirlineCompany_UnregisteredCountryCodeEntered_ThrowsException_LOGINTOKEN_PASSWORD);
            AirlineCompany airlineCompany = LoggedInAirlineFacadeTest_Constants.ModifyAirlineCompany_UnregisteredCountryCodeEntered_ThrowsException_UPDATED_AIRLINE_INFO;

            Assert.ThrowsException<UnregisteredDataException>(new Action(() => airlineFacade.ModifyAirlineDetails(airlineFacade.LoginToken, airlineCompany)));
        }

        [TestMethod]
        public void ModifyAirlineCompany_WrongPasswordEntered()
        {
            LoggedInAirlineFacade airlineFacade = GetAirlineFacade(LoggedInAirlineFacadeTest_Constants.ModifyAirlineCompany_WrongPasswordEntered_ThrowsException_LOGINTOKEN_USERNAME,
                                                                  LoggedInAirlineFacadeTest_Constants.ModifyAirlineCompany_WrongPasswordEntered_ThrowsException_LOGINTOKEN_PASSWORD);
            AirlineCompany airlineCompany = LoggedInAirlineFacadeTest_Constants.ModifyAirlineCompany_WrongPasswordEntered_ThrowsException_UPDATED_AIRLINE_INFO;

            Assert.ThrowsException<WrongPasswordException>(new Action(() => airlineFacade.ModifyAirlineDetails(airlineFacade.LoginToken, airlineCompany)));
        }

        [TestMethod]
        public void ModifyAirlineCompany_AttemptingToChangeUsername()
        {
            LoggedInAirlineFacade airlineFacade = GetAirlineFacade(LoggedInAirlineFacadeTest_Constants.ModifyAirlineCompany_AttemptingToChangeUsername_LOGINTOKEN_USERNAME,
                                                                  LoggedInAirlineFacadeTest_Constants.ModifyAirlineCompany_AttemptingToChangeUsername_LOGINTOKEN_PASSWORD);
            AirlineCompany airlineCompany = LoggedInAirlineFacadeTest_Constants.ModifyAirlineCompany_AttemptingToChangeUsername_UPDATED_AIRLINE_INFO;

            Assert.ThrowsException<UnauthorizedActionException>(new Action(() => airlineFacade.ModifyAirlineDetails(airlineFacade.LoginToken, airlineCompany)));
        }

        [TestMethod]
        public void GetAllFlights_CompanyHasFlights_ReturnNonNullList()
        {
            LoggedInAirlineFacade airlineFacade = GetAirlineFacade(LoggedInAirlineFacadeTest_Constants.GetAllFlights_CompanyHasFlights_ReturnNonNullList_LOGINTOKEN_USERNAME,
                                                                   LoggedInAirlineFacadeTest_Constants.GetAllFlights_CompanyHasFlights_ReturnNonNullList_LOGINTOKEN_PASSWORD);

            List<Flight> flights = (List<Flight>)airlineFacade.GetAllFlightsByAirline(airlineFacade.LoginToken);

            Assert.AreNotEqual(0, flights.Count);
            foreach(Flight flight in flights)
            {
                Assert.IsNotNull(flight);
            }
        }

        [TestMethod]
        public void GetAllFlights_CompanyHasNoFlights_ReturnNullList()
        {
            LoggedInAirlineFacade airlineFacade = GetAirlineFacade(LoggedInAirlineFacadeTest_Constants.GetAllFlights_CompanyHasNoFlights_ReturnNullList_LOGINTOKEN_USERNAME,
                                                                   LoggedInAirlineFacadeTest_Constants.GetAllFlights_CompanyHasNoFlights_ReturnNullList_LOGINTOKEN_PASSWORD);

            List<Flight> flights = (List<Flight>)airlineFacade.GetAllFlightsByAirline(airlineFacade.LoginToken);

            Assert.AreEqual(0, flights.Count);
        }

        [TestMethod]
        public void GetAllTicketsByFlight_FlightHasTickets()
        {
            LoggedInAirlineFacade airlineFacade = GetAirlineFacade(LoggedInAirlineFacadeTest_Constants.GetAllTicketsByFlight_FlightHasTickets_LOGINTOKEN_USERNAME,
                                                                   LoggedInAirlineFacadeTest_Constants.GetAllTicketsByFlight_FlightHasTickets_LOGINTOKEN_PASSWORD);
            int flightId = LoggedInAirlineFacadeTest_Constants.GetAllTicketsByFlight_FlightHasTickets_FLIGHT_ID;

            List<Ticket> tickets = (List<Ticket>)airlineFacade.GetAllTicketsByFlight(airlineFacade.LoginToken, flightId);

            Assert.AreNotEqual(0, tickets.Count);
            foreach (Ticket ticket in tickets)
            {
                Assert.IsNotNull(ticket);
            }
        }

        [TestMethod]
        public void GetAllTicketsByFlight_FlightNotFound_ThrowsException()
        {
            LoggedInAirlineFacade airlineFacade = GetAirlineFacade(LoggedInAirlineFacadeTest_Constants.GetAllTicketsByFlight_FlightNotFound_ThrowsException_LOGINTOKEN_USERNAME,
                                                                   LoggedInAirlineFacadeTest_Constants.GetAllTicketsByFlight_FlightNotFound_ThrowsException_LOGINTOKEN_PASSWORD);
            int flightId = LoggedInAirlineFacadeTest_Constants.GetAllTicketsByFlight_FlightNotFound_ThrowsException_FLIGHT_ID;
            List<Ticket> tickets;

            Assert.ThrowsException<NullReferenceException>(new Action(() => tickets = (List<Ticket>)airlineFacade.GetAllTicketsByFlight(airlineFacade.LoginToken, flightId)));

        }

        [TestMethod]
        public void GetTicketsByFlight_FlightHasNoTickets_ReturnNullList()
        {
            LoggedInAirlineFacade airlineFacade = GetAirlineFacade(LoggedInAirlineFacadeTest_Constants.GetTicketsByFlight_FlightHasNoTickets_ReturnNullList_LOGINTOKEN_USERNAME,
                                                                   LoggedInAirlineFacadeTest_Constants.GetTicketsByFlight_FlightHasNoTickets_ReturnNullList_LOGINTOKEN_PASSWORD);
            int flightId = LoggedInAirlineFacadeTest_Constants.GetTicketsByFlight_FlightHasNoTickets_ReturnNullList_FLIGHT_ID;

            List<Ticket> tickets = (List<Ticket>)airlineFacade.GetAllTicketsByFlight(airlineFacade.LoginToken, flightId);

            Assert.AreEqual(0, tickets.Count);
        }

        [TestMethod]
        public void GetTicketsByFlight_FlightBelongsToAnotherCompany_ThrowsException()
        {
            LoggedInAirlineFacade airlineFacade = GetAirlineFacade(LoggedInAirlineFacadeTest_Constants.GetTicketsByFlight_FlightBelongsToAnotherCompany_ThrowsException_LOGINTOKEN_USERNAME,
                                                                   LoggedInAirlineFacadeTest_Constants.GetTicketsByFlight_FlightBelongsToAnotherCompany_ThrowsException_LOGINTOKEN_PASSWORD);
            int flightId = LoggedInAirlineFacadeTest_Constants.GetTicketsByFlight_FlightBelongsToAnotherCompany_ThrowsException_FLIGHT_ID;
            List<Ticket> tickets;

            Assert.ThrowsException<UnauthorizedActionException>(new Action(() => tickets = (List<Ticket>)airlineFacade.GetAllTicketsByFlight(airlineFacade.LoginToken, flightId)));
        }

        public LoggedInAirlineFacade GetAirlineFacade(string username, string password)
        {
            int facadeIndex = flyingCenterSystem.UserLogin(username, password);
            LoggedInAirlineFacade airlineFacade = (LoggedInAirlineFacade)FlyingCenterSystem.FacadeList[facadeIndex];
            return airlineFacade;
        }
    }
}
