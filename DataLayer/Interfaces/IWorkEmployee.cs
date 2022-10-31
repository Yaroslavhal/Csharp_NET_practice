using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface IWorkEmployee : IWork
    {
        public IRepository<Models.Employee> Employees { get; }
    }
}
