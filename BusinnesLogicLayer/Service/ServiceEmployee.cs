using AutoMapper;
using BusinnesLogicLayer.DTO;
using BusinnesLogicLayer.Interface;
using DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinnesLogicLayer.Service
{
    public class ServiceEmployee : IService<EmployeeDTO>
    {
        private readonly IWorkEmployee EmployeeDB;
        public ServiceEmployee(IWorkEmployee EmployeeDB)
        {
            this.EmployeeDB = EmployeeDB;
        }
        public async Task AddItemAsync(object item)
        {
            if (item is EmployeeDTO)
                await AddEmploye((EmployeeDTO)item);
        }
        private async Task AddEmploye(EmployeeDTO item)
        {
            var employee = new DataLayer.Models.Employee
            {
                Surname = item.Surname,
                Name = item.Name,
                Patronymic = item.Patronymic,
                Age = item.Age,
                Salary = item.Salary
            };

            await EmployeeDB.Employees.Create(employee);
            await EmployeeDB.Save();
        }
        public IEnumerable<EmployeeDTO> GetEmployees()
        {

            var firstobj = new MapperConfiguration(map => map.CreateMap<DataLayer.Models.Employee, EmployeeDTO>()).CreateMapper();
            IEnumerable<EmployeeDTO> list = firstobj.Map<IEnumerable<DataLayer.Models.Employee>, IEnumerable<EmployeeDTO>>(EmployeeDB.Employees.GetAll());
            return list;
        }

        public async Task<EmployeeDTO> FindDTO(EmployeeDTO item)
        {
            var secondtobj = new MapperConfiguration(map => map.CreateMap<DataLayer.Models.Employee, EmployeeDTO>()).CreateMapper();
            EmployeeDTO employeeDTO = secondtobj.Map<DataLayer.Models.Employee, EmployeeDTO>(await EmployeeDB.Employees.Find(MappingModels(item)));
            return employeeDTO;
        }

        public async Task UpdateDTO(EmployeeDTO item)
        {
            EmployeeDB.Employees.Update(MappingModels(item));
            await EmployeeDB.Save();
        }

        public async Task DeleteDTO(EmployeeDTO item)
        {
            EmployeeDB.Employees.Delete(MappingModels(item));
            await EmployeeDB.Save();
        }

        public async Task AddList(IEnumerable<EmployeeDTO> templist)
        {
            if(templist != null)
            {
                foreach (var item in templist)
                {
                    await AddEmploye(item);
                }
            }     
        }
        public int GetIdDTO(EmployeeDTO item)
        {   
            return EmployeeDB.Employees.GetId(MappingModels(item));
        }
        private DataLayer.Models.Employee MappingModels(EmployeeDTO employeeDTO) 
        {
            var firstobj = new MapperConfiguration(map => map.CreateMap<EmployeeDTO, DataLayer.Models.Employee>()).CreateMapper();
            DataLayer.Models.Employee employee = firstobj.Map<EmployeeDTO, DataLayer.Models.Employee>(employeeDTO);
            return employee;
        }
    }
}
