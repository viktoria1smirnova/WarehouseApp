using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WarehouseApp.Data.Models;
using WarehouseApp.Data.Services;
using WarehouseApp.Views;

namespace WarehouseApp.ViewModels
{
    public class WarehousesViewModel : BaseViewModel
    {
        private readonly ServiceWhs _serviceWhs;
        private ServiceProducts? _serviceProducts;
        private Warehouse? _selectedWhs;
        private string _searchText = "";

        public ObservableCollection<Warehouse> Warehouses
        {
            get { return _serviceWhs.GetAll(); }
        }

        public ObservableCollection<Product>? Products
        {
            get
            {
                if (_serviceProducts == null) return null;
                return _serviceProducts.GetAll(_searchText);
            }
        }

        public Warehouse? SelectedWhs
        {
            get { return _selectedWhs; }
            set
            {
                _selectedWhs = value;
                if (_selectedWhs != null)
                    _serviceProducts = new ServiceProducts(_serviceWhs.GetContext(), _selectedWhs);

                OnPropertyChanged();
                OnPropertyChanged(nameof(Products));
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Products));
            }
        }

        public ICommand AddWhCommand { get; set; }
        public ICommand EditWhCommand { get; set; }
        public ICommand DeleteWhCommand { get; set; }
        public ICommand OpenProductCommand { get; set; }

        public WarehousesViewModel(ServiceWhs serviceWhs)
        {
            _serviceWhs = serviceWhs;

            AddWhCommand = new RelayCommand(AddWh);
            EditWhCommand = new RelayCommand(EditWh, x => SelectedWhs != null);
            DeleteWhCommand = new RelayCommand(DeleteWh, x => SelectedWhs != null);
            OpenProductCommand = new RelayCommand(OpenProduct);
        }

        public void AddWh(object? obj)
        {
            WhModWindow window = new WhModWindow("Добавить склад", _serviceWhs, null);
            if (window.ShowDialog() == true)
                OnPropertyChanged(nameof(Warehouses));
        }

        public void EditWh(object? obj)
        {
            if (SelectedWhs == null) return;

            WhModWindow window = new WhModWindow("Изменить склад", _serviceWhs, SelectedWhs);
            if (window.ShowDialog() == true)
                OnPropertyChanged(nameof(Warehouses));
        }

        public void DeleteWh(object? obj)
        {
            if (SelectedWhs == null) return;

            var result = MessageBox.Show($"Вы действительно хотите удалить склад «{SelectedWhs.WhName}»?", "Подтверждение удаления", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _serviceWhs.DelWhs(SelectedWhs);
                OnPropertyChanged(nameof(Warehouses));
            }
        }

        public void OpenProduct(object? obj)
        {
            if (obj is Product product && SelectedWhs != null)
            {
                ProductModWindow editor = new ProductModWindow(product, new ServiceProducts(_serviceWhs.GetContext(), SelectedWhs));
                if (editor.ShowDialog() == true)
                {
                    OnPropertyChanged(nameof(Products));
                }
            }
        }

        public void RefreshProducts()
        {
            OnPropertyChanged(nameof(Products));
        }
    }
}