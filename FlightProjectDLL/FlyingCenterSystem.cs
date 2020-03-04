using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using FlightProject.DAOs;
using FlightProject.POCOs;
using FlightProject.Facades;
using System.Configuration;

namespace FlightProject
{
    public class FlyingCenterSystem
    {
        static AutoResetEvent resetEvent = new AutoResetEvent(true);

        private static FlyingCenterSystem instance;
        private static object key = new object();
        public static List<AnonymousUserFacade> FacadeList;
        private static int FacadeListIndex = 0;
        public bool isTestMode = false;

        /// <summary>
        /// Method for getting a FlyingCenterSystem Instance
        /// </summary>
        /// <returns FlyingCenterSystem>
        /// Returns the single existing instance of this class.
        /// </returns>
        public static FlyingCenterSystem GetInstance()
        {
            lock (key)
            {
                if (instance == null)
                {
                    instance = new FlyingCenterSystem();
                }
            }
            return instance;
        }

        /// <summary>
        /// Constructor for FlyingCenterSystem
        /// </summary>
        private FlyingCenterSystem()
        {
            FacadeList = new List<AnonymousUserFacade>();
            GetFacade();
            new Thread(FlightCleanerTimer).Start();
            new Thread(CleanFlightList).Start();
        }

        /// <summary>
        /// Method for timing the thread which cleans the flight list.
        /// </summary>
        private static void FlightCleanerTimer()
        {
            Int32.TryParse(ConfigurationManager.AppSettings["HOUR_VALUE"], out int hours);
            Int32.TryParse(ConfigurationManager.AppSettings["MINUTE_VALUE"], out int minutes);
            Int32.TryParse(ConfigurationManager.AppSettings["SECOND_VALUE"], out int seconds);
            TimeSpan timeSpan = new TimeSpan(hours, minutes, seconds);
            while (true)
            {
                Thread.Sleep(timeSpan);
                resetEvent.Set();
            }

        }

        /// <summary>
        /// Method for cleaning the flight list
        /// </summary>
        private static void CleanFlightList()
        {
            resetEvent.WaitOne();
            HiddenFacade hiddenFacade = new HiddenFacade();
            hiddenFacade.CleanFlightList();
        }

        /// <summary>
        /// Method for logging in a user into the system
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns int>
        /// Returns the location of the user's facade in the facade list.
        /// </returns>
        public int UserLogin(string username, string password)
        {
            LoginService loginService = new LoginService(username, password);
            return loginService.FacadeIndex;
        }

        /// <summary>
        /// Method for logging in as an administrator.
        /// </summary>
        /// <param name="loginToken"></param>
        /// <returns int>
        /// Returns the location of the user's facade in the facade list.
        /// </returns>
        internal static int GetFacade(LoginToken<Administrator> loginToken)
        {
            LoggedInAdministratorFacade loggedInAdministratorFacade = new LoggedInAdministratorFacade(loginToken);
            FacadeList.Add(loggedInAdministratorFacade);
            FacadeListIndex++;
            return FacadeListIndex;
        }

        /// <summary>
        /// Method for logging in as an airline company.
        /// </summary>
        /// <param name="loginToken"></param>
        /// <returns int>
        /// Returns the location of the user's facade in the facade list.
        /// </returns>
        internal static int GetFacade(LoginToken<AirlineCompany> loginToken)
        {
            LoggedInAirlineFacade loggedInAirlineFacade = new LoggedInAirlineFacade(loginToken);
            FacadeList.Add(loggedInAirlineFacade);
            FacadeListIndex++;
            return FacadeListIndex;
        }

        /// <summary>
        /// Method for logging in as a customer.
        /// </summary>
        /// <param name="loginToken"></param>
        /// <returns int>
        /// Returns the location of the user's facade in the facade list.
        /// </returns>
        internal static int GetFacade(LoginToken<Customer> loginToken)
        {
            LoggedInCustomerFacade loggedInCustomerFacade = new LoggedInCustomerFacade(loginToken);
            FacadeList.Add(loggedInCustomerFacade);
            FacadeListIndex++;
            return FacadeListIndex;
        }

        /// <summary>
        /// Method for creating an anonymous facade.
        /// </summary>
        internal static void GetFacade()
        {
            AnonymousUserFacade anonymousUserFacade = new AnonymousUserFacade();
            FacadeList.Add(anonymousUserFacade);
        }

        /// <summary>
        /// Unit testing method. Runs before each test starts.
        /// </summary>
        public void StartTest()
        {
            if (isTestMode)
            {
                HiddenFacade hiddenFacade = new HiddenFacade();
                hiddenFacade.DbTestPrep();
            }
        }

        /// <summary>
        /// Method for clearing the database.
        /// </summary>
        public void ClearDb()
        {
            HiddenFacade hiddenFacade = new HiddenFacade();
            hiddenFacade.clearDb();
        }
    }


}
