using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightProject.POCOs;

namespace FlightProjectTest
{
    static class LoggedInAirlineFacadeTest_Constants
    {
        //Flight Id needs to belong to an existing flight where the airline matches the login token

        public static readonly string CancelFlight_FlightFound_LOGINTOKEN_USERNAME = "foo";
        public static readonly string CancelFlight_FlightFound_LOGINTOKEN_PASSWORD = "8898";
        public static readonly int CancelFlight_FlightFound_FLIGHT_ID = 1;

        //Flight Id needs to belong to an existing flight where the airline is not the login token

        public static readonly string CancelFlight_FlightBelongsToAnotherUser_ThrowsException_LOGINTOKEN_USERNAME = "foo";
        public static readonly string CancelFlight_FlightBelongsToAnotherUser_ThrowsException_LOGINTOKEN_PASSWORD = "8898";
        public static readonly int CancelFlight_FlightBelongsToAnotherUser_ThrowsException_FLIGHT_ID = 3;

        // First int in new flight must match login token id

        public static readonly string CreateFlight_NewFlight_FlagDropsIfListContainsCreatedFlight_LOGINTOKEN_USERNAME = "foo";
        public static readonly string CreateFlight_NewFlight_FlagDropsIfListContainsCreatedFlight_LOGINTOKEN_PASSWORD = "8898";
        public static readonly Flight CreateFlight_NewFlight_FlagDropsIfListContainsCreatedFlight_NEW_FLIGHT = new Flight(1, 3, 5, new DateTime(2020, 10, 20), new DateTime(2020, 10, 21), 100);

        // First int in new flight must match login token id

        public static readonly string CreateFlight_FlightAlreadyExists_ThrowsException_LOGINTOKEN_USERNAME = "foo";
        public static readonly string CreateFlight_FlightAlreadyExists_ThrowsException_LOGINTOKEN_PASSWORD = "8898";
        public static readonly Flight CreateFlight_FlightAlreadyExists_ThrowsException_EXISTING_FLIGHT = new Flight(1, 2, 1, new DateTime(2020, 01, 18, 12, 45, 54), new DateTime(2020, 01, 18, 18, 45, 54), 100);

        // first three ints must match the existing record in DB

        public static readonly string UpdateFlight_FlightFound_MakesRequestedChanges_LOGINTOKEN_USERNAME = "foo";
        public static readonly string UpdateFlight_FlightFound_MakesRequestedChanges_LOGINTOKEN_PASSWORD = "8898";
        public static readonly Flight UpdateFlight_FlightFound_MakesRequestedChanges_UPDATED_FLIGHT_DETAILS = new Flight(1, 2, 3, new DateTime(2020, 01, 29, 7, 45, 54), new DateTime(2020, 01, 29, 10, 45, 00), 120);
        public static readonly int UpdateFlight_FlightFound_MakesRequestedChanges_FLIGHT_ID = 2;

        // Flight Id must be higher than the number of registered flights

        public static readonly string UpdateFlight_FlightNotFound_ThrowsException_LOGINTOKEN_USERNAME = "foo";
        public static readonly string UpdateFlight_FlightNotFound_ThrowsException_LOGINTOKEN_PASSWORD = "8898";
        public static readonly Flight UpdateFlight_FlightNotFound_ThrowsException_UPDATED_FLIGHT_DETAILS = new Flight(1, 2, 3, new DateTime(2020, 01, 29, 7, 45, 54), new DateTime(2020, 01, 29, 10, 45, 00), 120);
        public static readonly int UpdateFlight_FlightNotFound_ThrowsException_FLIGHT_ID = 9;

        public static readonly string ChangeMyPassword_CorrectPasswordEntered_ThrowsExceptionWhenOldPasswordEntered_LOGINTOKEN_USERNAME = "foo";
        public static readonly string ChangeMyPassword_CorrectPasswordEntered_ThrowsExceptionWhenOldPasswordEntered_LOGINTOKEN_PASSWORD = "8898";
        public static readonly string ChangeMyPassword_CorrectPasswordEntered_ThrowsExceptionWhenOldPasswordEntered_NEW_PASSWORD = "7786";

        public static readonly string ChangeMyPassword_WrongPasswordEntered_ThrowsException_LOGINTOKEN_USERNAME = "foo";
        public static readonly string ChangeMyPassword_WrongPasswordEntered_ThrowsException_LOGINTOKEN_PASSWORD = "8898";
        public static readonly string ChangeMyPassword_WrongPasswordEntered_ThrowsException_WRONG_OLD_PASSWORD = "ASDAD";
        public static readonly string ChangeMyPassword_WrongPasswordEntered_ThrowsException_NEW_PASSWORD = "12313";

        public static readonly string ModifyAirlineCompany_CorrectPasswordEntered_LoginTokenContainsUpdatedInfo_LOGINTOKEN_USERNAME = "foo";
        public static readonly string ModifyAirlineCompany_CorrectPasswordEntered_LoginTokenContainsUpdatedInfo_LOGINTOKEN_PASSWORD = "8898";
        public static readonly AirlineCompany ModifyAirlineCompany_CorrectPasswordEntered_LoginTokenContainsUpdatedInfo_UPDATED_AIRLINE_INFO = new AirlineCompany("zek", "foo", "8898", 5);

        public static readonly string ModifyAirlineCompany_UnregisteredCountryCodeEntered_ThrowsException_LOGINTOKEN_USERNAME = "foo";
        public static readonly string ModifyAirlineCompany_UnregisteredCountryCodeEntered_ThrowsException_LOGINTOKEN_PASSWORD = "8898";
        public static readonly AirlineCompany ModifyAirlineCompany_UnregisteredCountryCodeEntered_ThrowsException_UPDATED_AIRLINE_INFO = new AirlineCompany("zek", "foo", "8898", 9);

        public static readonly string ModifyAirlineCompany_WrongPasswordEntered_ThrowsException_LOGINTOKEN_USERNAME = "foo";
        public static readonly string ModifyAirlineCompany_WrongPasswordEntered_ThrowsException_LOGINTOKEN_PASSWORD = "8898";
        public static readonly AirlineCompany ModifyAirlineCompany_WrongPasswordEntered_ThrowsException_UPDATED_AIRLINE_INFO = new AirlineCompany("zek", "foo", "7789", 5);

        public static readonly string ModifyAirlineCompany_AttemptingToChangeUsername_LOGINTOKEN_USERNAME = "foo";
        public static readonly string ModifyAirlineCompany_AttemptingToChangeUsername_LOGINTOKEN_PASSWORD = "8898";
        public static readonly AirlineCompany ModifyAirlineCompany_AttemptingToChangeUsername_UPDATED_AIRLINE_INFO = new AirlineCompany("zek", "loo", "8898", 5);

        public static readonly string GetAllFlights_CompanyHasFlights_ReturnNonNullList_LOGINTOKEN_USERNAME = "foo";
        public static readonly string GetAllFlights_CompanyHasFlights_ReturnNonNullList_LOGINTOKEN_PASSWORD = "8898";

        public static readonly string GetAllFlights_CompanyHasNoFlights_ReturnNullList_LOGINTOKEN_USERNAME = "roo";
        public static readonly string GetAllFlights_CompanyHasNoFlights_ReturnNullList_LOGINTOKEN_PASSWORD = "6546";

        public static readonly string GetAllTicketsByFlight_FlightHasTickets_LOGINTOKEN_USERNAME = "foo";
        public static readonly string GetAllTicketsByFlight_FlightHasTickets_LOGINTOKEN_PASSWORD = "8898";
        public static readonly int GetAllTicketsByFlight_FlightHasTickets_FLIGHT_ID = 2;

        public static readonly string GetAllTicketsByFlight_FlightNotFound_ThrowsException_LOGINTOKEN_USERNAME = "foo";
        public static readonly string GetAllTicketsByFlight_FlightNotFound_ThrowsException_LOGINTOKEN_PASSWORD = "8898";
        public static readonly int GetAllTicketsByFlight_FlightNotFound_ThrowsException_FLIGHT_ID = 9;

        public static readonly string GetTicketsByFlight_FlightHasNoTickets_ReturnNullList_LOGINTOKEN_USERNAME = "too";
        public static readonly string GetTicketsByFlight_FlightHasNoTickets_ReturnNullList_LOGINTOKEN_PASSWORD = "36846";
        public static readonly int GetTicketsByFlight_FlightHasNoTickets_ReturnNullList_FLIGHT_ID = 3;

        public static readonly string GetTicketsByFlight_FlightBelongsToAnotherCompany_ThrowsException_LOGINTOKEN_USERNAME = "foo";
        public static readonly string GetTicketsByFlight_FlightBelongsToAnotherCompany_ThrowsException_LOGINTOKEN_PASSWORD = "8898";
        public static readonly int GetTicketsByFlight_FlightBelongsToAnotherCompany_ThrowsException_FLIGHT_ID = 3;
    }
}
