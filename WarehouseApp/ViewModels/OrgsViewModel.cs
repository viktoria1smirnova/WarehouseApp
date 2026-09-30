using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WarehouseApp.Data.Models;
using WarehouseApp.Data.Services;

namespace WarehouseApp.ViewModels
{
    public class OrgsViewModel : BaseViewModel
    {
        private ServiceOrgs _service;
        private Organization? _selectedOrg;

        public ObservableCollection<Organization> Organizations
        {
            get { return _service.GetAll(); }
        }

        public Organization? SelectedOrg
        {
            get { return _selectedOrg; }
            set
            {
                _selectedOrg = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(StatusText));
            }
        }

        public string StatusText
        {
            get
            {
                if (SelectedOrg != null)
                    return SelectedOrg.OrgName;
                return "—";
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand OkCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public OrgsViewModel(ServiceOrgs service)
        {
            _service = service;

            AddCommand = new RelayCommand(AddOrg);
            EditCommand = new RelayCommand(EditOrg, x => SelectedOrg != null);
            DeleteCommand = new RelayCommand(DeleteOrg, x => SelectedOrg != null);
            OkCommand = new RelayCommand(OpenMainWindow, x => SelectedOrg != null);
            ExitCommand = new RelayCommand(CloseWindow);
        }

        public void AddOrg(object? obj)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Название организации:", "Новая организация", "");

            if (string.IsNullOrWhiteSpace(input))
                return;

            Organization org = new Organization(input.Trim());
            _service.AddOrg(org);
        }

        public void EditOrg(object? obj)
        {
            if (SelectedOrg == null) 
                return;

            string input = Microsoft.VisualBasic.Interaction.InputBox("Название организации:", "Редактирование организации", SelectedOrg.OrgName);

            if (string.IsNullOrWhiteSpace(input)) 
                return;

            Organization editedOrg = new Organization(input)
            {
                Id = SelectedOrg.Id,
            };

            _service.EditOrg(editedOrg);
            OnPropertyChanged(nameof(StatusText));
        }

        public void DeleteOrg(object? obj)
        {
            if (SelectedOrg == null) 
                return;

            MessageBoxResult result = MessageBox.Show($"Удалить организацию '{SelectedOrg.OrgName}'?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
                _service.DelOrg(SelectedOrg);
        }

        public void OpenMainWindow(object? obj)
        {
            if (SelectedOrg == null) 
                return;

            MainWindow mainWindow = new MainWindow(SelectedOrg.OrgName, new ServiceWhs(_service.GetContext(), SelectedOrg));
            mainWindow.Show();
        }

        public void CloseWindow(object? obj)
        {
            Application.Current.Windows[0].Close();
        }
    }
}