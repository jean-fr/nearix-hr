using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Nearix.HR.Core.Interfaces;
using Nearix.HR.Core.Model;

namespace Nearix.HR.Data
{
    public class EmployeeDao : IEmployeeDao
    {
        private List<string> ComplexFields = new List<string> { "ageinyears", "weightinkgs", "yearofjoining", "monthofjoining", "dayofjoining", "ageincompanyinyears", "salary", "lasthike" };

        private readonly ILoggingService _loggingService;
        /*For the sake of this test, keep it simple*/
        public EmployeeDao(string connectionString, ILoggingService loggingService)
        {
            this._connectionString = connectionString;
            this._loggingService = loggingService;

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException($"{connectionString} argument must be provided");
            }
        }

        private readonly string _connectionString;

        public List<Employee> Find(EmployeeSearch search)
        {
            List<Employee> result = new List<Employee>();
            try
            {
                using SqlConnection connection = new SqlConnection(this._connectionString);
                using SqlCommand cmd = new SqlCommand();
                cmd.Parameters.AddWithValue("@skip", search.Skip);
                cmd.Parameters.AddWithValue("@take", search.Take);
                cmd.Parameters.AddWithValue("@sort_by", string.IsNullOrWhiteSpace(search.SortBy) ? null : SortByMapping.Map.GetValueOrDefault(search.SortBy));
                cmd.Parameters.AddWithValue("@sort_order", search.SortOrder == Core.SortOrder.Asc ? "asc" : "desc");

                this.AssignFindCommandParameters(search, cmd);

                cmd.CommandText = "sp_employee_search";
                cmd.Connection = connection;

                connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Employee employee = new Employee();
                        this.FillEmployee(reader, employee);
                        result.Add(employee);
                    }
                }
                connection.Close();
                return result;
            }
            catch (Exception ex)
            {
                this._loggingService.Error(ex);
                return result;
            }
        }

        public bool Save(Employee employee)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(this._connectionString);
                using SqlCommand cmd = new SqlCommand("sp_employee_save", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employee_id", employee.EmployeeId);
                cmd.Parameters.AddWithValue("@first_name", employee.FirstName);
                cmd.Parameters.AddWithValue("@last_name", employee.LastName);
                cmd.Parameters.AddWithValue("@user_name", employee.UserName);
                cmd.Parameters.AddWithValue("@password", employee.Password);
                cmd.Parameters.AddWithValue("@name_prefix", employee.NamePrefix);
                cmd.Parameters.AddWithValue("@middle_initial", employee.MiddleInitial);
                cmd.Parameters.AddWithValue("@gender", employee.Gender);
                cmd.Parameters.AddWithValue("@email", employee.Email);
                cmd.Parameters.AddWithValue("@father_name", employee.FatherName);
                cmd.Parameters.AddWithValue("@mother_maiden_name", employee.MotherMaidenName);
                cmd.Parameters.AddWithValue("@mother_name", employee.MotherName);
                cmd.Parameters.AddWithValue("@date_of_birth", employee.DateOfBirth);
                cmd.Parameters.AddWithValue("@time_of_birth", employee.TimeOfBirth);
                cmd.Parameters.AddWithValue("@age_in_years", employee.AgeInYears);
                cmd.Parameters.AddWithValue("@weight_in_kgs", employee.WeightInKgs);
                cmd.Parameters.AddWithValue("@date_of_joining", employee.DateOfJoining);
                cmd.Parameters.AddWithValue("@quarter_of_joining", employee.QuarterOfJoining);
                cmd.Parameters.AddWithValue("@half_of_joining", employee.HalfOfJoining);
                cmd.Parameters.AddWithValue("@year_of_joining", employee.YearOfJoining);
                cmd.Parameters.AddWithValue("@month_of_joining", employee.MonthOfJoining);
                cmd.Parameters.AddWithValue("@month_name_of_joining", employee.MonthNameOfJoining);
                cmd.Parameters.AddWithValue("@short_month", employee.ShortMonth);
                cmd.Parameters.AddWithValue("@day_of_joining", employee.DayOfJoining);
                cmd.Parameters.AddWithValue("@dow_of_joining", employee.DowOfJoining);
                cmd.Parameters.AddWithValue("@short_dow", employee.ShortDow);
                cmd.Parameters.AddWithValue("@age_in_company_in_years", employee.AgeInCompanyInYears);
                cmd.Parameters.AddWithValue("@salary", employee.Salary);
                cmd.Parameters.AddWithValue("@last_hike", employee.LastHike);
                cmd.Parameters.AddWithValue("@ssn", employee.Ssn);
                cmd.Parameters.AddWithValue("@phone_number", employee.PhoneNumber);
                cmd.Parameters.AddWithValue("@place_name", employee.PlaceName);
                cmd.Parameters.AddWithValue("@county", employee.County);
                cmd.Parameters.AddWithValue("@city", employee.City);
                cmd.Parameters.AddWithValue("@state", employee.State);
                cmd.Parameters.AddWithValue("@zip", employee.Zip);
                cmd.Parameters.AddWithValue("@region", employee.Region);

                connection.Open();
                var result = cmd.ExecuteNonQuery();
                connection.Close();
                return result > 0;
            }
            catch (Exception ex)
            {
                this._loggingService.Error(ex);
                return false;
            }
        }

        private void FillEmployee(SqlDataReader reader, Employee employee)
        {
            if (reader == null || employee == null) return;

            employee.EmployeeId = reader.GetInt32("employee_id");
            employee.FirstName = reader["first_name"].ToString();
            employee.LastName = reader["last_name"].ToString();
            employee.Email = reader["email"].ToString();
            employee.UserName = reader["user_name"].ToString();
            employee.Password = reader["password"].ToString();
            employee.NamePrefix = reader["name_prefix"].ToString();
            employee.MiddleInitial = reader["middle_initial"].ToString();
            employee.Gender = reader["gender"].ToString();
            employee.FatherName = reader["father_name"].ToString();
            employee.MotherMaidenName = reader["mother_maiden_name"].ToString();
            employee.MotherName = reader["mother_name"].ToString();
            employee.DateOfBirth = reader.GetDateTime("date_of_birth").Date;
            employee.TimeOfBirth = reader["time_of_birth"].ToString();
            employee.AgeInYears = reader.GetDouble("age_in_years");
            employee.WeightInKgs = reader.GetDouble("weight_in_kgs");
            employee.DateOfJoining = reader.GetDateTime("date_of_joining");
            employee.QuarterOfJoining = reader["quarter_of_joining"].ToString();
            employee.HalfOfJoining = reader["half_of_joining"].ToString();
            employee.YearOfJoining = reader.GetInt32("year_of_joining");
            employee.MonthOfJoining = reader.GetInt32("month_of_joining");
            employee.MonthNameOfJoining = reader["month_name_of_joining"].ToString();
            employee.ShortMonth = reader["short_month"].ToString();
            employee.DayOfJoining = reader.GetInt32("day_of_joining");
            employee.DowOfJoining = reader["dow_of_joining"].ToString();
            employee.ShortDow = reader["short_dow"].ToString();
            employee.AgeInCompanyInYears = reader.GetDouble("age_in_company_in_years");
            employee.Salary = reader.GetDecimal("salary");
            employee.LastHike = reader.GetDouble("last_hike");
            employee.Ssn = reader["ssn"].ToString();
            employee.PhoneNumber = reader["phone_number"].ToString();
            employee.PlaceName = reader["place_name"].ToString();
            employee.County = reader["county"].ToString();
            employee.City = reader["city"].ToString();
            employee.State = reader["state"].ToString();
            employee.Zip = reader["zip"].ToString();
            employee.Region = reader["region"].ToString();

        }

        public int FindCount(EmployeeSearch search)
        {
            int count = 0;
            try
            {
                using SqlConnection connection = new SqlConnection(this._connectionString);
                using SqlCommand cmd = new SqlCommand();
                this.AssignFindCommandParameters(search, cmd);
                cmd.CommandText = "sp_employee_search_count";
                cmd.Connection = connection;
                connection.Open();
                count = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
                return count;
            }
            catch (Exception ex)
            {
                this._loggingService.Error(ex);
                return count;
            }
        }

        private void AssignFindCommandParameters(EmployeeSearch search, SqlCommand cmd)
        {
            if (cmd == null) return;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@query", search.Query);

            if (search.Filters.Any())
            {
                foreach (var f in search.Filters)
                {
                    cmd.Parameters.AddWithValue($"@{f.FieldName}", f.Value);
                    if (this.ComplexFields.Contains(f.FieldName))
                    {
                        cmd.Parameters.AddWithValue($"@{f.FieldName}_optr", f.Operator.ToString().ToLower());
                    }
                }
            }
        }



        public bool Exists(int id)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(this._connectionString);
                using SqlCommand cmd = new SqlCommand("sp_employee_exists", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employee_id", id);
                connection.Open();
                var exists = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                connection.Close();
                return exists;
            }
            catch (Exception ex)
            {
                this._loggingService.Error(ex);
                return false;
            }
        }
    }
}
