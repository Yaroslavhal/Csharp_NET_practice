using DataLayer.DataContext;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.WorkTemp
{
    public class WorkEmployee : IWorkEmployee
    {
        private EmployeeContext _employeeContext;
        Repository.EmployeeRepository employeeRepository;
        public WorkEmployee(DbContextOptions<EmployeeContext> connectionString)
        {
            _employeeContext = new EmployeeContext(connectionString);
        }
        public IRepository<Employee> Employees { get { return employeeRepository = new Repository.EmployeeRepository(_employeeContext); } }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task Save()
        {
            await _employeeContext.SaveChangesAsync();
        }
    }
}
