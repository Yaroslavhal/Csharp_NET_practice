using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataContext
{
    public class EmployeeContext : DbContext
    {
        private EmployeeContext employeeContext;

        public DbSet<Employee> Employees { get; set; }
        public EmployeeContext(DbContextOptions<EmployeeContext> connectionString) : base(connectionString)
        {
            Database.EnsureCreated();
        }

        public EmployeeContext(EmployeeContext employeeContext)
        {
            this.employeeContext = employeeContext;
        }
    }
}
