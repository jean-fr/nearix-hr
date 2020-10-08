using System.Collections.Generic;
using Nearix.HR.Core.Model;

namespace Nearix.HR.Core.Interfaces
{
    public interface IEmployeeDao
    {
        bool Save(Employee employee);
        List<Employee> Find(EmployeeSearch search);
        int FindCount(EmployeeSearch search);
        bool Exists(int id);
    }
}
