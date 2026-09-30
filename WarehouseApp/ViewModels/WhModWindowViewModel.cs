using System.Windows;
using System.Windows.Input;
using WarehouseApp.Data.Models;
using WarehouseApp.Data.Services;

namespace WarehouseApp.ViewModels
{
    public class WhModWindowViewModel : BaseViewModel
    {
        private ServiceWhs _service;
        private Warehouse? _whs;
        private Window _window;

        private string _whName = "";
        private string _whAddress = "";
        private string _errorText = "";

        public string WhName
        {
            get { return _whName; }
            set { _whName = value; OnPropertyChanged(); }
        }

        public string WhAddress
        {
            get { return _whAddress; }
            set { _whAddress = value; OnPropertyChanged(); }
        }

        public string ErrorText
        {
            get { return _errorText; }
            set { _errorText = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public WhModWindowViewModel(Window window, ServiceWhs service, Warehouse? whs)
        {
            _window = window;
            _service = service;
            _whs = whs;

            if (_whs != null)
            {
                WhName = _whs.WhName;
                WhAddress = _whs.WhAddress;
            }

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        public void Save(object? obj)
        {
            ErrorText = "";

            string name = WhName.Trim();
            string address = WhAddress.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(address))
            {
                ErrorText = "Должны быть заполнены оба поля.";
                return;
            }

            bool saved;

            if (_whs != null)
            {
                _whs.WhName = name;
                _whs.WhAddress = address;
                saved = _service.EditWhs(_whs);
            }
            else
            {
                Warehouse newWhs = new Warehouse(name, address, _service.GetOrganization().Id);
                saved = _service.AddWhs(newWhs);
            }

            if (!saved)
            {
                MessageBox.Show(_window, "Не удалось сохранить склад.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _window.DialogResult = true;
            _window.Close();
        }

        public void Cancel(object? obj)
        {
            _window.DialogResult = false;
            _window.Close();
        }
    }
}