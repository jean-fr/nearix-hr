using System.Collections.Generic;

namespace Nearix.HR.Data
{
    public  class SortByMapping
    {
        public static Dictionary<string, string> Map
        {
            get
            {
                var map = new Dictionary<string, string>
                {
                    { "employeeid", "employee_id" },
                    { "firstname", "first_name" },
                    { "lastname", "last_name" },
                    { "username", "user_name" },
                    { "password", "password" },
                    { "nameprefix", "name_prefix" },
                    { "middleinitial", "middle_initial" },
                    { "gender", "gender" },
                    { "email", "email" },
                    { "fathername", "father_name" },
                    { "mothermaidenname", "mother_maiden_name" },
                     { "mothername", "mother_name" },
                    { "dateofbirth", "date_of_birth" },
                    { "timeofbirth", "time_of_birth" },
                    { "ageinyears", "age_in_years" },
                    { "weightinkgs", "weight_in_kgs" },
                    { "dateofjoining", "date_of_joining" },
                    { "quarterofjoining", "quarter_of_joining" },
                    { "halfofjoining", "half_of_joining" },
                    { "yearofjoining", "year_of_joining" },
                    { "monthofjoining", "month_of_joining" },
                    { "monthnameofjoining", "month_name_of_joining" },
                    { "shortmonth", "short_month" },
                    { "dayofjoining", "day_of_joining" },
                    { "dowofjoining", "dow_of_joining" },
                    { "shortdow", "short_dow" },
                    { "ageincompany", "age_in_company_in_years" },
                    { "salary", "salary" },
                    { "lasthike", "last_hike" },
                    { "ssn", "ssn" },
                    { "phonenumber", "phone_number" },
                    { "placename", "place_name" },
                    { "county", "county" },
                    { "city", "city" },
                    { "state", "state" },
                    { "zip", "zip" },
                    { "region", "region" }
                };
                return map;
            }
        }
    }
}
