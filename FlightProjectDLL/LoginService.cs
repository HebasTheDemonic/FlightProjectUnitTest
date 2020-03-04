﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightProject.POCOs;
using FlightProject.Exceptions;

namespace FlightProject
{
    public class LoginService : ILoginService
    {
        internal DAOs.IGeneralDAO _genaralDAO;
        internal DAOs.IAdministratorDAO _administratorDAO;
        internal DAOs.IAirlineDAO _airlineDAO;
        internal DAOs.ICustomerDAO _customerDAO;
        internal int FacadeIndex { get; }

        /// <summary>
        /// Method for retrieving a login token based on user type.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public LoginService(string username, string password)
        {
            LoginEnum loginEnum = TryLogin(username);

            switch (loginEnum)
            {
                case LoginEnum.ADMINISTRATOR:
                    {
                        LoginToken<Administrator> loginToken = new LoginToken<Administrator>();
                        if (AdminLogin(username, password, out loginToken))
                        {
                            FacadeIndex = FlyingCenterSystem.GetFacade(loginToken);
                        }
                        return;
                    }
                case LoginEnum.AIRLINE:
                    {
                        LoginToken<AirlineCompany> loginToken = new LoginToken<AirlineCompany>();
                        if (AirlineLogin(username, password, out loginToken))
                        {
                            FacadeIndex = FlyingCenterSystem.GetFacade(loginToken);
                        }
                        return;
                    }
                case LoginEnum.CUSTOMER:
                    {
                        LoginToken<Customer> loginToken = new LoginToken<Customer>();
                        if (CustomerLogin(username, password, out loginToken))
                        {
                            FacadeIndex = FlyingCenterSystem.GetFacade(loginToken);
                        }
                        return;
                    }
                default:
                    {
                        return;
                    }
            }
        }

        /// <summary>
        /// Method for determining the user type.
        /// </summary>
        /// <param name="userName"></param>
        /// <exception cref="UserNotFoundException">
        /// Thrown when the given username does not exist in the database.
        /// </exception>
        /// <returns LoginEnum>
        /// An enum representing the user type
        /// </returns>
        public LoginEnum TryLogin(string userName)
        {
            int loginValue = 0;
            _genaralDAO = new DAOs.GeneralDAO();
            loginValue = _genaralDAO.TryLogin(userName);
            try
            {
                switch (loginValue)
                {
                    case 1:
                        {
                            return LoginEnum.ADMINISTRATOR;
                        }
                    case 2:
                        {
                            return LoginEnum.AIRLINE;
                        }
                    case 3:
                        {
                            return LoginEnum.CUSTOMER;
                        }
                    default:
                        {
                            throw new UserNotFoundException();
                        }
                }
            }
            catch (InvalidCastException)
            {
                throw new UserNotFoundException();
            }

        }

        /// <summary>
        /// Method for logging in an administrator.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="loginToken">
        /// This param has the "OUT" prefix due to being also returned from this method with the bool value
        /// </param>
        /// <returns bool></returns>
        public bool AdminLogin(string userName, string password, out LoginToken<Administrator> loginToken)
        {
            _administratorDAO = new DAOs.AdministratorDAO();
            Administrator administrator = _administratorDAO.GetAdministratorByUsername(userName);

            if (password == administrator.Password)
            {
                loginToken = new LoginToken<Administrator>();
                loginToken.User = administrator;
                return true;
            }

            throw new WrongPasswordException();
        }

        /// <summary>
        /// Method for logging in an airline company.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="loginToken">
        /// This param has the "OUT" prefix due to being also returned from this method with the bool value
        /// </param>
        /// <returns bool></returns>
        public bool AirlineLogin(string userName, string password, out LoginToken<AirlineCompany> loginToken)
        {
            _airlineDAO = new DAOs.AirlineDAO();
            AirlineCompany airlineCompany = _airlineDAO.GetAirlineCompanybyUsername(userName);

            if (password == airlineCompany.Password)
            {
                loginToken = new LoginToken<AirlineCompany>();
                loginToken.User = airlineCompany;
                return true;
            }

            throw new WrongPasswordException();
        }

        /// <summary>
        /// Method for logging in a customer.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="loginToken">
        /// This param has the "OUT" prefix due to being also returned from this method with the bool value
        /// </param>
        /// <returns bool></returns>
        public bool CustomerLogin(string userName, string password, out LoginToken<Customer> loginToken)
        {
            _customerDAO = new DAOs.CustomerDAO();
            Customer customer = _customerDAO.GetCustomerByUsername(userName);

            if (password == customer.Password)
            {
                loginToken = new LoginToken<Customer>();
                loginToken.User = customer;
                return true;
            }

            throw new WrongPasswordException();
        }
    }
}
