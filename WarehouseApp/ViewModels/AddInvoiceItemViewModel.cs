using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WarehouseApp.Data.Models;
using WarehouseApp.Data.Services;

namespace WarehouseApp.ViewModels
{
    public class AddInvoiceItemViewModel : BaseViewModel
    {
        private ServiceProducts _serviceProducts;
        private Window _window;

        public Product? SelectedProduct { get; set; }
        public string Quantity { get; set; } = "";
        public InvoiceItem? ResultItem { get; set; }
        public ObservableCollection<Product> Products
            => _serviceProducts.GetAll();

        public ICommand AddCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public AddInvoiceItemViewModel(Window window, ServiceProducts serviceProducts)
        {
            _window = window;
            _serviceProducts = serviceProducts;

            AddCommand = new RelayCommand(Add);
            CancelCommand = new RelayCommand(Cancel);
        }

        public void Add(object? obj)
        {
            if (!int.TryParse(Quantity, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Поле \"Количество\" заполненно некорректно", "Ошибка");
                return;
            }

            if (SelectedProduct == null)
            {
                MessageBox.Show("Товар не выбран", "Предупреждение");
                return;
            }

            ResultItem = new InvoiceItem(SelectedProduct)
            {
                Quantity = quantity
            };

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