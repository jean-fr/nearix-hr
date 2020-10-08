using System.Collections.Generic;

namespace Nearix.HR.Service
{
    public class EmployeeMapping
    {
        public static Dictionary<string, string> Map
        {
            get
            {
                var map = new Dictionary<string, string>
                {
                    { "employeeid", "Employee Id" },
                    { "firstname", "First Name" },
                    { "lastname", "Last Name" },
                    { "username", "User Name" },
                    { "password", "Password" },
                    { "nameprefix", "Name Prefix" },
                    { "middleinitial", "Middle Initial" },
                    { "gender", "Gender" },
                    { "email", "Email" },
                    { "fathername", "Father Name" },
                    { "mothername", "Mother Name" },
                    { "mothermaidenname", "Mother Maiden Name" },
                    { "dateofbirth", "Date Of Birth" },
                    { "timeofbirth", "Time Of Birth" },
                    { "ageinyears", "Age In Years" },
                    { "weightinkgs", "Weight In Kgs" },
                    { "dateofjoining", "Date Of Joining" },
                    { "quarterofjoining", "Quarter Of Joining" },
                    { "halfofjoining", "Half Of Joining" },
                    { "yearofjoining", "Year Of Joining" },
                    { "monthofjoining", "Month Of Joining" },
                    { "monthnameofjoining", "Month Name Of Joining" },
                    { "shortmonth", "Short Month" },
                    { "dayofjoining", "Day Of Joining" },
                    { "dowofjoining", "DOW Of Joining" },
                    { "shortdow", "Short DOW" },
                    { "ageincompanyinyears", "Age In Company In Years" },
                    { "salary", "Salary" },
                    { "lasthike", "Last Hike" },
                    { "ssn", "SSN" },
                    { "phonenumber", "Phone Number" },
                    { "placename", "Place Name" },
                    { "county", "County" },
                    { "city", "City" },
                    { "state", "State" },
                    { "zip", "Zip" },
                    { "region", "Region" }
                };
                return map;
            }
        }
    }
}
