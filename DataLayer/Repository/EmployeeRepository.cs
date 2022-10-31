using DataLayer.DataContext;
using DataLayer.Interfaces;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class EmployeeRepository : IRepository<Employee>
    {
        EmployeeContext _employeeContext;
        public EmployeeRepository(EmployeeContext context)
        {
            _employeeContext = context;
        }
        public async Task Create(Employee item)
        {
            if (item != null)
            {
                await _employeeContext.Employees.AddAsync(item);
            }
        }

        public void Delete(Employee item)
        {
            _employeeContext.Employees.Remove(_employeeContext.Employees.First(e => e.Name == item.Name && e.Surname == item.Surname && e.Patronymic == item.Patronymic && e.Age == item.Age && e.Salary == item.Salary));
        }

        public async Task<Employee> Find(Employee item)
        {
            var employee = await _employeeContext.Employees.FindAsync(item);
            if (employee != null)
                return employee;
            throw new InvalidOperationException();
        }

        public IEnumerable<Employee> GetAll()
        {
            return  _employeeContext.Employees;
        }

        public void Update(Employee item)
        {
            if (item != null)
            {
                var newItem = _employeeContext.Employees.Where(x => x.Id == item.Id).First();
                newItem.Surname = item.Surname;
                newItem.Name = item.Name;
                newItem.Patronymic = item.Patronymic;
                newItem.Age = item.Age;
                newItem.Salary = item.Salary;
            }
        }
        public int GetId(Employee item)
        {
            var tempItem = _employeeContext.Employees.Where(e => e.Name == item.Name && e.Surname == item.Surname && e.Patronymic == item.Patronymic && e.Age == item.Age && e.Salary == item.Salary).First();
            return tempItem.Id;
        }
    }
}
