using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WarehouseApp.Controls;
using WarehouseApp.Data.Services;
using WarehouseApp.Views;

namespace WarehouseApp.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly ServiceWhs _serviceWhs;
        private object? _currentPage;
        private WarehousesViewModel _warehousesViewModel;
        private Window _window;

        public object? CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; OnPropertyChanged(); }
        }

        public ICommand ExitCommand { get; set; }
        public ICommand AddIncomingInvoiceCommand { get; set; }
        public ICommand AddOutgoingInvoiceCommand { get; set; }

        public MainWindowViewModel(Window window, ServiceWhs serviceWhs)
        {
            _window = window;
            _serviceWhs = serviceWhs;
            _warehousesViewModel = new WarehousesViewModel(_serviceWhs);
            CurrentPage = new UserControllWareHouses(_warehousesViewModel);

            ExitCommand = new RelayCommand(Exit);
            AddIncomingInvoiceCommand = new RelayCommand(AddIncomingInvoice);
            AddOutgoingInvoiceCommand = new RelayCommand(AddOutgoingInvoice);
        }

        public void Exit(object? obj)
        {
            _window.Close();
        }

        public void AddIncomingInvoice(object? obj)
        {
            if (_warehousesViewModel.SelectedWhs == null)
            {
                MessageBox.Show("Склад не выбран", "Предупреждение");
                return;
            }

            ServiceInvoices serviceInvoices = new ServiceInvoices(_serviceWhs.GetContext(), _warehousesViewModel.SelectedWhs);
            IncomingInvoiceWindow window = new IncomingInvoiceWindow(serviceInvoices);

            if (window.ShowDialog() == true)
            {
                _warehousesViewModel.RefreshProducts();
            }
        }

        public void AddOutgoingInvoice(object? obj)
        {
            if (_warehousesViewModel.SelectedWhs == null)
            {
                MessageBox.Show("Склад не выбран", "Предупреждение");
                return;
            }

            ServiceInvoices serviceInvoices = new ServiceInvoices(_serviceWhs.GetContext(), _warehousesViewModel.SelectedWhs);
            OutgoingInvoiceWindow window = new OutgoingInvoiceWindow(serviceInvoices);

            if (window.ShowDialog() == true)
            {
                _warehousesViewModel.RefreshProducts();
            }
        }
    }
}