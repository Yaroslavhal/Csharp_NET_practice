using DataLayer.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinnesLogicLayer.Infrastructure
{
    public class ConfigurationBLL
    {
        public static void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<EmployeeContext>(opt => opt.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EmployeeDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));
        }
        public static void AddDependecy(ServiceCollection services)
        {
            services.AddSingleton(typeof(DataLayer.Interfaces.IWorkEmployee), typeof(DataLayer.WorkTemp.WorkEmployee));
        }
    }
}
