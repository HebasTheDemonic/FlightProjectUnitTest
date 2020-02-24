using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FlightProject;
using FlightProject.Facades;
using FlightProject.POCOs;
using FlightProject.Exceptions;

namespace FlightProjectTest
{
    [TestClass]
    public class AnonymousUserFacadeTest
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
        public void GetAllFlights_ReturnsListOfAllFlights()
        {
            AnonymousUserFacade userFacade = (AnonymousUserFacade)FlyingCenterSystem.FacadeList[0];

            List<Flight> flights = (List<Flight>)userFacade.GetAllFlights();

            Assert.AreEqual(5, flights.Count);
            foreach (Flight flight in flights)
            {
                Assert.IsNotNull(flight);
            }

        }

        [TestMethod]
        public void GetAirlineCompanies_ReturnsListOfAllAirlineCompanies()
        {
            AnonymousUserFacade userFacade = (AnonymousUserFacade)FlyingCenterSystem.FacadeList[0];

            List<AirlineCompany> airlineCompanies = (List<AirlineCompany>)userFacade.GetAirlineCompanies();

            Assert.AreEqual(4, airlineCompanies.Count);
            foreach (AirlineCompany airlineCompany in airlineCompanies)
            {
                Assert.IsNotNull(airlineCompany);
            }
        }

        [TestMethod]
        public void GetFlightById_ReturnsFlightObject()
        {
            AnonymousUserFacade userFacade = (AnonymousUserFacade)FlyingCenterSystem.FacadeList[0];

            Flight flight = userFacade.GetFlightById(3);

            Assert.AreEqual(4, flight.AirlineCompanyId);
            Assert.AreEqual(3, flight.Id);
        }

        [TestMethod]
        public void GetFlightById_FlightIdDoesNotExist_ThrowsException()
        {
            AnonymousUserFacade userFacade = (AnonymousUserFacade)FlyingCenterSystem.FacadeList[0];

            Assert.ThrowsException<NullResultException>(new Action(() => { Flight flight = userFacade.GetFlightById(6); }));
        }

        [TestMethod]
        public void GetFlightsByOriginCountry_CountryHasSeveralFlights_ReturnsListOfFlights()
        {
            AnonymousUserFacade userFacade = (AnonymousUserFacade)FlyingCenterSystem.FacadeList[0];

            List<Flight> flights = (List<Flight>)userFacade.GetFlightsByOriginCountry(2);

            Assert.AreEqual(2, flights.Count);
            foreach (Flight flight in flights)
            {
                Assert.IsNotNull(flight);
                Assert.AreEqual(2, flight.OriginCountryId);
            }
        }

        [TestMethod]
        public void GetFlightsByOriginCountry_CountryHas1Flight_ReturnsListOfFlights()
        {
            AnonymousUserFacade userFacade = (AnonymousUserFacade)FlyingCenterSystem.FacadeList[0];

            List<Flight> flights = (List<Flight>)userFacade.GetFlightsByOriginCountry(5);

            Assert.AreEqual(1, flights.Count);
            foreach (Flight flight in flights)
            {
                Assert.IsNotNull(flight);
                Assert.AreEqual(5, flight.OriginCountryId);
            }
        }

        [TestMethod]
        public void GetFlightsByOriginCountry_CountryHasNoFlights_ReturnsNullList()
        {
            AnonymousUserFacade userFacade = (AnonymousUserFacade)FlyingCenterSystem.FacadeList[0];

            List<Flight> flights = (List<Flight>)userFacade.GetFlightsByOriginCountry(3);

            Assert.AreEqual(0, flights.Count);
        }

        [TestMethod]
        public void GetFlightByDepartureDate_TwoFlightsOnSameDateDiffrentTime_ReturnsTwoFlights()
        {
            AnonymousUserFacade userFacade = (AnonymousUserFacade)FlyingCenterSystem.FacadeList[0];
            DateTime dateTime = new DateTime(2020, 10, 20);

            List<Flight> flights = (List<Flight>)userFacade.GetFlightsByDepartureDate(dateTime);

            Assert.AreEqual(2, flights.Count);
            foreach (Flight flight in flights)
            {
                Assert.IsNotNull(flight);
            }
        }

        [TestMethod]
        public void GetFlightsByDepartureDate_OnlyOneFlightOnDate_ReturnsOneFlight()
        {
            AnonymousUserFacade userFacade = (AnonymousUserFacade)FlyingCenterSystem.FacadeList[0];
            DateTime dateTime = new DateTime(2020, 10, 10);

            List<Flight> flights = (List<Flight>)userFacade.GetFlightsByDepartureDate(dateTime);

            Assert.AreEqual(1, flights.Count);
            foreach (Flight flight in flights)
            {
                Assert.IsNotNull(flight);
            }
        }

        [TestMethod]
        public void GetFlightsDepartureDate_NoFlightsOnDate_ReturnsNullList()
        {
            AnonymousUserFacade userFacade = (AnonymousUserFacade)FlyingCenterSystem.FacadeList[0];
            DateTime dateTime = new DateTime(2019, 10, 10);

            List<Flight> flights = (List<Flight>)userFacade.GetFlightsByDepartureDate(dateTime);

            Assert.AreEqual(0, flights.Count);
        }

        [TestMethod]
        public void GetFlightsByLandingDate_TwoFlightsOnSameDate_ReturnsTwoFlights()
        {
            AnonymousUserFacade userFacade = (AnonymousUserFacade)FlyingCenterSystem.FacadeList[0];
            DateTime dateTime = new DateTime(2020, 10, 21);

            List<Flight> flights = (List<Flight>)userFacade.GetFlightsByLandingDate(dateTime);

            Assert.AreEqual(2, flights.Count);
            foreach (Flight flight in flights)
            {
                Assert.IsNotNull(flight);
            }
        }

        [TestMethod]
        public void GetFlightsByLandingDate_OneFlightOnDate_ReturnsOneFlight()
        {
            AnonymousUserFacade userFacade = (AnonymousUserFacade)FlyingCenterSystem.FacadeList[0];
            DateTime dateTime = new DateTime(2020, 10, 11);

            List<Flight> flights = (List<Flight>)userFacade.GetFlightsByLandingDate(dateTime);

            Assert.AreEqual(1, flights.Count);
            foreach (Flight flight in flights)
            {
                Assert.IsNotNull(flight);
            }
        }

        [TestMethod]
        public void GetFlightsByLandingDate_NoFlightOnDate_ReturnsNullList()
        {
            AnonymousUserFacade userFacade = (AnonymousUserFacade)FlyingCenterSystem.FacadeList[0];
            DateTime dateTime = new DateTime(2019, 10, 11);

            List<Flight> flights = (List<Flight>)userFacade.GetFlightsByLandingDate(dateTime);

            Assert.AreEqual(0, flights.Count);
        }

        [TestMethod]
        public void GetFlightsByDestinationCountry_TwoFlightsToDestination_ReturnsTwoFlights()
        {
            AnonymousUserFacade userFacade = (AnonymousUserFacade)FlyingCenterSystem.FacadeList[0];

            List<Flight> flights = (List<Flight>)userFacade.GetFlightsByDestinationCountry(1);

            Assert.AreEqual(2, flights.Count);
            foreach (Flight flight in flights)
            {
                Assert.IsNotNull(flight);
                Assert.AreEqual(1, flight.DestinationCountryId);
            }
        }

        [TestMethod]
        public void GetFlightsByDestinationCountry_OneFlightToDestination_ReturnsOneFlight()
        {
            AnonymousUserFacade userFacade = (AnonymousUserFacade)FlyingCenterSystem.FacadeList[0];

            List<Flight> flights = (List<Flight>)userFacade.GetFlightsByDestinationCountry(3);

            Assert.AreEqual(1, flights.Count);
            foreach (Flight flight in flights)
            {
                Assert.IsNotNull(flight);
                Assert.AreEqual(3, flight.DestinationCountryId);
            }
        }

        [TestMethod]
        public void GetFlightsByDestinationCountry_NoFlightsToDestination_ThrowsException()
        {
            AnonymousUserFacade userFacade = (AnonymousUserFacade)FlyingCenterSystem.FacadeList[0];

            List<Flight> flights = (List<Flight>)userFacade.GetFlightsByDestinationCountry(2);

            Assert.AreEqual(0, flights.Count);
        }
    }
}
