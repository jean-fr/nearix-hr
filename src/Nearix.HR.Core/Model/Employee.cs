using System;

namespace Nearix.HR.Core.Model
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string NamePrefix { get; set; }
        public string MiddleInitial { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string MotherMaidenName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string TimeOfBirth { get; set; }
        public double AgeInYears { get; set; }
        public double WeightInKgs { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string QuarterOfJoining { get; set; }
        public string HalfOfJoining { get; set; }
        public int YearOfJoining { get; set; }
        public int MonthOfJoining { get; set; }
        public string MonthNameOfJoining { get; set; }
        public string ShortMonth { get; set; }
        public int DayOfJoining { get; set; }
        public string DowOfJoining { get; set; }
        public string ShortDow { get; set; }
        public double AgeInCompanyInYears { get; set; }  //Age in Company (Years)
        public decimal Salary { get; set; }
        public double LastHike { get; set; }
        public string Ssn { get; set; }
        public string PhoneNumber { get; set; }
        public string PlaceName { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Region { get; set; }

    }
}
