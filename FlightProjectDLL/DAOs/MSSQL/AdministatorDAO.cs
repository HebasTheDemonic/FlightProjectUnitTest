using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightProject.POCOs;

namespace FlightProject.DAOs
{
    internal class AdministratorDAO : IAdministratorDAO
    {

        public void Add(Administrator t)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MSSQLConnectionString"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "ADD_ADMINISTRATOR",
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

                sqlCommand.Parameters.Add(usernameParameter);
                sqlCommand.Parameters.Add(passwordParameter);

                connection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                connection.Close();
            }
        }

        public int DoesAdministratorExist(Administrator administrator)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MSSQLConnectionString"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "DOES_ADMINISTRATOR_EXIST",
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter usernameParameter = new SqlParameter
                {
                    SqlDbType = SqlDbType.Char,
                    SqlValue = administrator.Username,
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

        public Administrator Get(int id)
        {
            Administrator administrator = new Administrator();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MSSQLConnectionString"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "GET_ADMINISTRATOR_BY_ID",
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
                    administrator = new Administrator((int)sqlDataReader["ID"],
                                                      (string)sqlDataReader["USER"],
                                                      (string)sqlDataReader["PASS"]);
                }
                connection.Close();
            }
            return administrator;
        }

        public Administrator GetAdministratorByUsername(string username)
        {
            Administrator administrator = new Administrator();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MSSQLConnectionString"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "GET_ADMINISTRATOR_BY_USERNAME",
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter usernameParameter = new SqlParameter
                {
                    SqlDbType = SqlDbType.Char,
                    SqlValue = username,
                    ParameterName = "@USER"
                };

                sqlCommand.Parameters.Add(usernameParameter);

                connection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read() == true)
                {
                    administrator = new Administrator((int)sqlDataReader["ID"],
                                                     (string)sqlDataReader["USER"],
                                                     (string)sqlDataReader["PASS"]);
                }
                connection.Close();
            }
            return administrator;
        }

        public IList<Administrator> GetAll()
        {
            List<Administrator> administrators = new List<Administrator>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MSSQLConnectionString"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "GET_ALL_ADMINISTRATORS",
                    CommandType = CommandType.StoredProcedure
                };

                connection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read() == true)
                {
                    Administrator administrator = new Administrator((int)sqlDataReader["ID"],
                                                                    (string)sqlDataReader["USER"],
                                                                    (string)sqlDataReader["PASS"]);
                    administrators.Add(administrator);
                }
                connection.Close();
            }
            return administrators;
        }

        public void Remove(Administrator t)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MSSQLConnectionString"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "REMOVE_ADMINISTRATOR",
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

        public void Update(Administrator t)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MSSQLConnectionString"].ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "UPDATE_ADMINISTRATOR",
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

                SqlParameter idParameter = new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    SqlValue = t.Id,
                    ParameterName = "@ID"
                };


                sqlCommand.Parameters.Add(usernameParameter);
                sqlCommand.Parameters.Add(passwordParameter);
                sqlCommand.Parameters.Add(idParameter);

                connection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                connection.Close();
            }
        }
    }
}
