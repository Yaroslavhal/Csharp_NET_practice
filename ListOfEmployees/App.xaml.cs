using BusinnesLogicLayer.DTO;
using BusinnesLogicLayer.Infrastructure;
using BusinnesLogicLayer.Interface;
using BusinnesLogicLayer.Service;
using ListOfEmployees.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ListOfEmployees
{
    public partial class App : Application
    {
        private IServiceProvider serviceProvider;
        public App()
        {
            var service = new ServiceCollection();
            ConfigureServices(service);
            serviceProvider = service.BuildServiceProvider();

        }
        private void ConfigureServices(ServiceCollection collection)
        {
            collection.AddSingleton(typeof(IService<EmployeeDTO>), typeof(ServiceEmployee));
            collection.AddSingleton(typeof(EmployeeViewModel));
            collection.AddSingleton(typeof(View.MainWindow));
            ConfigurationBLL.ConfigureServices(collection);
            ConfigurationBLL.AddDependecy(collection);
        }
        private void OnStarup(object sender, StartupEventArgs arg)
        {
            var mainWindow = (Window)serviceProvider.GetService<ListOfEmployees.View.MainWindow>();
            mainWindow.Show();
        }
    }
}
