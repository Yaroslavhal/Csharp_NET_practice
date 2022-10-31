using BusinnesLogicLayer.DTO;
using BusinnesLogicLayer.Interface;
using ListOfEmployees.Command;
using ListOfEmployees.View;
using ListOfEmployees.ViewModels.Base;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ListOfEmployees.ViewModels
{
    public class EmployeeViewModel : ViewModel
    {
        public readonly IService<EmployeeDTO> _service;
        public ObservableCollection<EmployeeDTO> Employees { get; set; }
        private List<EmployeeDTO> AllEmployees { get; set; }
        public EmployeeViewModel(IService<EmployeeDTO> service)
        {
            _service = service;
            AllEmployees = new List<EmployeeDTO>(service.GetEmployees());
            Employees = new ObservableCollection<EmployeeDTO>();
            CopyEmployee();
        }
        private void CopyEmployee()
        {
            Employees.Clear();
            foreach (var item in AllEmployees)
            {
                Employees.Add(item);
            }
        }
        ///////////////////////////Open Window
        static AddEmployeeWindow _window;
        private ICommand _OpenWindow;
        public ICommand CreateOpenWindow => _OpenWindow ??= new LambdaCommand(OnOpenWindowExecude, CanOpenWindowExecude);
        private static bool CanOpenWindowExecude(object p) => _window == null;

        public void OnOpenWindowExecude(object p)
        {
            WindowParametrs(p);
            var window = new AddEmployeeWindow(this)
            {
                Owner = Application.Current.MainWindow
            };
            _window = window;
            window.Closed += OnWindowClosed;
            window.ShowDialog();
        }
        private void OnWindowClosed(object sender, EventArgs e)
        {
            ((Window)sender).Closed -= OnWindowClosed;
            _window = null;
        }
        ///////////////////////////////Window parametr
        private void WindowParametrs(object p)
        {
            if (p != null)
            {
                EmployeeView = (EmployeeDTO)p;
                ButtonContent = "Edit";
                SurnameView = EmployeeView.Surname;
                NameView = EmployeeView.Name;
                PatronymicView = EmployeeView.Patronymic;
                AgeView = EmployeeView.Age;
                SalaryView = EmployeeView.Salary;
            }
            else
            {
                ButtonContent = "Add";
            }
        }
        ///////////////////////////////Close Window
        private ICommand _CloseApplication;
        public ICommand CloseApplicationCommand => _CloseApplication ??= new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
        private bool CanCloseApplicationCommandExecute(object p) => true;
        private void OnCloseApplicationCommandExecuted(object p)
        {
            _window.Close();
            ItemNull();
        }
        ///////////////////////////////Add new employee and Edit item
        public string ButtonContent { get; set; }

        public EmployeeDTO EmployeeView;
        public string SurnameView { get; set; }
        public string NameView { get; set; }
        public string PatronymicView { get; set; }
        public int? AgeView { get; set; }
        public float? SalaryView { get; set; }

        private RelayCommand _AddEmployee;
        public RelayCommand CreateNewEmployeeCommand
        {
            get
            {
                return _AddEmployee ?? new RelayCommand(obj =>
                {
                    Window wnd = obj as Window;
                    if (!ErrorItem(wnd))
                    {
                        if (ButtonContent == "Edit")
                            SaveEditEmployee();
                        else
                            SaveAddEmployee();
                        ItemNull();
                        wnd.Close();
                    }   
                }
                );
            }
        }
        private void SetRedBlockControll(Window wnd, string blockName)
        {
            Control block = wnd.FindName(blockName) as Control;
            block.BorderBrush = Brushes.Red;
        }
        private bool ErrorItem(Window wnd)
        {
            bool errorItem = false;
            if (SurnameView == null || SurnameView.Replace(" ", "").Length == 0)
            {
                SetRedBlockControll(wnd, "Surname");
                errorItem = true;
            }
            if (NameView == null || NameView.Replace(" ", "").Length == 0)
            {
                SetRedBlockControll(wnd, "Name");
                errorItem = true;
            }
            if (PatronymicView == null || PatronymicView.Replace(" ", "").Length == 0)
            {
                SetRedBlockControll(wnd, "Patronymic");
                errorItem = true;
            }
            if (AgeView == null || AgeView < 0)
            {
                SetRedBlockControll(wnd, "Age");
                errorItem = true;
            }
            if (SalaryView == null || SalaryView < 0)
            {
                SetRedBlockControll(wnd, "Salary");
                errorItem = true;
            }
            return errorItem;
        }
        private void SaveAddEmployee()
        {
            EmployeeView = new EmployeeDTO();
            SaveEmployeeView();
            _service.AddItemAsync(EmployeeView);
            AllEmployees.Add(EmployeeView);
            CopyEmployee();
        }
        private void SaveEditEmployee()
        {
            if (EmployeeView.Id == 0)
            {
                var temp = AllEmployees.First(e => e.Surname == EmployeeView.Surname && e.Name == EmployeeView.Name && e.Patronymic == EmployeeView.Patronymic && e.Age == EmployeeView.Age && e.Salary == EmployeeView.Salary);
                int i = AllEmployees.IndexOf(temp);
                EmployeeView.Id = _service.GetIdDTO(AllEmployees[i]);
                AllEmployees[i] = EmployeeView;
                SaveEmployeeView();
            }
            else
            {
                SaveEmployeeView();
                var temp = AllEmployees.First(e => e.Id == EmployeeView.Id);
                int i = AllEmployees.IndexOf(temp);
                AllEmployees[i] = EmployeeView;           
            }
            _service.UpdateDTO(EmployeeView);
            CopyEmployee();
        }
        private void SaveEmployeeView()
        {
            EmployeeView.Surname = SurnameView;
            EmployeeView.Name = NameView;
            EmployeeView.Patronymic = PatronymicView;
            EmployeeView.Age = AgeView;
            EmployeeView.Salary = SalaryView;
        }
        ///////////////////////////////refresh item
        private void ItemNull()
        {
            EmployeeView = null;
            SurnameView = null;
            NameView = null;
            PatronymicView = null;
            AgeView = null;
            SalaryView = null;
        }
        ///////////////////////////////Find item
        private string searchString = string.Empty;
        public string SearchString
        {
            get
            {
                return searchString;
            }
            set
            {
                if (searchString == value)
                {
                    return;
                }
                searchString = value;
                CopyEmployee();
                foreach (var item in AllEmployees)
                {
                    if (!item.Surname.ToString().ToLower().Contains(value.ToLower()))
                    {
                        Employees.Remove(item);
                    }
                }
            }
        }
        //////////////////////////////Delete item
        private RelayCommand _DeleteEmployee;
        public RelayCommand DeleteEmployee
        {
            get
            {
                return _DeleteEmployee ?? (_DeleteEmployee =
                    new RelayCommand(obj =>
                    {
                        if (MessageBoxQuestion("Delete this employee?", "Delete Form Closing") == MessageBoxResult.Yes)
                        {
                            _service.DeleteDTO((EmployeeDTO)obj);
                            AllEmployees.Remove((EmployeeDTO)obj);
                            CopyEmployee();
                        }
                    }));
            }
        }
        private MessageBoxResult MessageBoxQuestion(string message, string caption)
        {
            var result = MessageBox.Show(message, caption, MessageBoxButton.YesNo);
            return result;
        }
        ///////////////////////////Save To Json File
        private RelayCommand _SaveToJsonFile;
        public RelayCommand SaveToJsonFile
        {
            get
            {
                return _SaveToJsonFile ?? (_SaveToJsonFile =
                    new RelayCommand(obj =>
                    {
                        if (MessageBoxQuestion("Save to Json file?", "Save Form Closing") == MessageBoxResult.Yes)
                        {
                            var res = _service.GetEmployees();
                            var strJson = JsonConvert.SerializeObject(res);
                            File.WriteAllText(Directory.GetCurrentDirectory() + "EmployeeJson.txt", strJson);
                            MessageBox.Show("File saved");
                        }     
                    }));
            }
        }
        ////////////////////////Loading From JsonFile
        private RelayCommand _LoadingFromJsonFile;
        public RelayCommand LoadingFromJsonFile
        {
            get
            {
                return _LoadingFromJsonFile ?? (_LoadingFromJsonFile =
                    new RelayCommand(obj =>
                    {
                        Op del = Ad;
                        del += Mul;
                        del += Mul;
                        del += Ad;
                        del -= Mul;
                        int res = del(6, 5);
                        if (MessageBoxQuestion("Downloded Json file?", "Downloded Form Closing") == MessageBoxResult.Yes)
                        {
                            var strJSONRESULT = File.ReadAllText(Directory.GetCurrentDirectory() + "EmployeeJson.txt");
                            var employees = JsonConvert.DeserializeObject<List<EmployeeDTO>>(strJSONRESULT);
                            _service.AddList(employees);
                            foreach (var item in employees)
                            {
                                AllEmployees.Add(item);
                            }
                            CopyEmployee();
                            MessageBox.Show("File downloaded");
                        }
                    }));
            }
        }
        ////////////////////
        private static int Ad(int x, int y) { return x + y; }
        private static int Mul(int x, int y) { return x * y; }
        delegate int Op(int x, int y);
    }
}
