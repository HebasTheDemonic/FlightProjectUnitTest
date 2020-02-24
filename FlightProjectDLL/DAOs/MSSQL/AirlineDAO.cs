﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightProject.POCOs;

namespace FlightProject.DAOs
{
    internal class AirlineDAO : IAirlineDAO
    {

        public void Add(AirlineCompany t)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MSSQLConnectionString"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "ADD_AIRLINE_COMPANY",
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter usernameParameter = new SqlParameter
                {
                    SqlDbType = SqlDbType.Char,
                    SqlValue = t.Username,
                    ParameterName = "@USER"
                };

                SqlParameter passwordParameter = new SqlParameter
                {
                    SqlDbType = SqlDbType.Char,
                    SqlValue = t.Password,
                    ParameterName = "@PASS"
                };

                SqlParameter companyNameParameter = new SqlParameter
                {
                    SqlDbType = SqlDbType.Char,
                    SqlValue = t.AirlineName,
                    ParameterName = "@NAME"
                };

                SqlParameter originCountryParameter = new SqlParameter
                {
                    SqlDbType = SqlDbType.Char,
                    SqlValue = t.OriginCountry,
                    ParameterName = "@CODE"
                };

                sqlCommand.Parameters.Add(usernameParameter);
                sqlCommand.Parameters.Add(passwordParameter);
                sqlCommand.Parameters.Add(companyNameParameter);
                sqlCommand.Parameters.Add(originCountryParameter);

                connection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                connection.Close();
            }
        }

        public int DoesAirlineCompanyExist(AirlineCompany airlineCompany)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MSSQLConnectionString"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "DOES_AIRLINE_COMPANY_EXIST",
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter usernameParameter = new SqlParameter
                {
                    SqlDbType = SqlDbType.Char,
                    SqlValue = airlineCompany.Username,
                    ParameterName = "@USER"
                };

                SqlParameter returnValueParameter = new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output,
                    ParameterName = "@VALUE"
                };

                sqlCommand.Parameters.Add(usernameParameter);
                sqlCommand.Parameters.Add(returnValueParameter);

                connection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                result = (int)returnValueParameter.Value;
                connection.Close();
            }
            return result;
        }

        public AirlineCompany Get(int id)
        {
            AirlineCompany airlineCompany = new AirlineCompany();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MSSQLConnectionString"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "GET_AIRLINE_COMPANY_BY_ID",
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter idParameter = new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    SqlValue = id,
                    ParameterName = "@ID"
                };

                sqlCommand.Parameters.Add(idParameter);

                connection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read() == true)
                {
                    airlineCompany = new AirlineCompany((int)sqlDataReader["ID"],
                                                        (string)sqlDataReader["NAME"],
                                                        (string)sqlDataReader["USER"],
                                                        (string)sqlDataReader["PASS"],
                                                        (int)sqlDataReader["COUNTRY"]);
                }
                connection.Close();
            }
            return airlineCompany;
        }

        public AirlineCompany GetAirlineCompanybyUsername(string userName)
        {
            AirlineCompany airlineCompany = new AirlineCompany();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MSSQLConnectionString"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "GET_AIRLINE_COMPANY_BY_USERNAME",
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter usernameParameter = new SqlParameter
                {
                    SqlDbType = SqlDbType.Char,
                    SqlValue = userName,
                    ParameterName = "@USER"
                };

                sqlCommand.Parameters.Add(usernameParameter);

                connection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read() == true)
                {
                    airlineCompany = new AirlineCompany((int)sqlDataReader["ID"],
                                                         (string)sqlDataReader["NAME"],
                                                         (string)sqlDataReader["USER"],
                                                         (string)sqlDataReader["PASS"],
                                                         (int)sqlDataReader["COUNTRY"]);
                }
                connection.Close();
            }
            return airlineCompany;
        }

        public IList<AirlineCompany> GetAll()
        {
            List<AirlineCompany> airlineCompanies = new List<AirlineCompany>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MSSQLConnectionString"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "GET_ALL_AIRLINE_COMPANIES",
                    CommandType = CommandType.StoredProcedure
                };

                connection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read() == true)
                {
                    AirlineCompany airlineCompany = new AirlineCompany((int)sqlDataReader["ID"],
                                                        (string)sqlDataReader["NAME"],
                                                        (string)sqlDataReader["USER"],
                                                        (string)sqlDataReader["PASS"],
                                                        (int)sqlDataReader["COUNTRY"]);
                    airlineCompanies.Add(airlineCompany);
                }
                connection.Close();
            }
            return airlineCompanies;
        }

        public IList<AirlineCompany> GetAllAirlinesByCountry(int countryId)
        {
            List<AirlineCompany> airlineCompanies = new List<AirlineCompany>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MSSQLConnectionString"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "GET_ALL_AIRLINE_COMPANIES_BY_COUNTRY",
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter countryIdParameter = new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    SqlValue = countryId,
                    ParameterName = "@ID"
                };

                sqlCommand.Parameters.Add(countryIdParameter);

                connection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read() == true)
                {
                    AirlineCompany airlineCompany = new AirlineCompany((int)sqlDataReader["ID"],
                                                        (string)sqlDataReader["NAME"],
                                                        (string)sqlDataReader["USER"],
                                                        (string)sqlDataReader["PASS"],
                                                        (int)sqlDataReader["COUNTRY"]);
                    airlineCompanies.Add(airlineCompany);
                }
                connection.Close();
            }
            return airlineCompanies;
        }

        public void Remove(AirlineCompany t)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MSSQLConnectionString"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "REMOVE_AIRLINE_COMPANY",
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter idParameter = new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    SqlValue = t.Id,
                    ParameterName = "@ID"
                };

                sqlCommand.Parameters.Add(idParameter);

                connection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                connection.Close();
            }
        }

        public void Update(AirlineCompany t)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MSSQLConnectionString"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "UPDATE_AIRLINE_COMPANY",
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter usernameParameter = new SqlParameter
                {
                    SqlDbType = SqlDbType.Char,
                    SqlValue = t.Username,
                    ParameterName = "@USER"
                };

                SqlParameter passwordParameter = new SqlParameter
                {
                    SqlDbType = SqlDbType.Char,
                    SqlValue = t.Password,
                    ParameterName = "@PASS"
                };

                SqlParameter companyNameParameter = new SqlParameter
                {
                    SqlDbType = SqlDbType.Char,
                    SqlValue = t.AirlineName,
                    ParameterName = "@NAME"
                };

                SqlParameter originCountryParameter = new SqlParameter
                {
                    SqlDbType = SqlDbType.Char,
                    SqlValue = t.OriginCountry,
                    ParameterName = "@CODE"
                };

                SqlParameter idParameter = new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    SqlValue = t.Id,
                    ParameterName = "@ID"
                };

                sqlCommand.Parameters.Add(usernameParameter);
                sqlCommand.Parameters.Add(passwordParameter);
                sqlCommand.Parameters.Add(companyNameParameter);
                sqlCommand.Parameters.Add(originCountryParameter);
                sqlCommand.Parameters.Add(idParameter);

                connection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                connection.Close();
            }
        }
    }
}
