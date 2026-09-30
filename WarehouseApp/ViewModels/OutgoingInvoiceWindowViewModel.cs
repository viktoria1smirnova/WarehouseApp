using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WarehouseApp.Data.Enums;
using WarehouseApp.Data.Models;
using WarehouseApp.Data.Services;
using WarehouseApp.Views;

namespace WarehouseApp.ViewModels
{
    public class OutgoingInvoiceWindowViewModel : BaseViewModel
    {
        private readonly ServiceInvoices _serviceInvoices;
        private readonly Window _window;

        public ObservableCollection<InvoiceItem> Items { get; set; } = new ObservableCollection<InvoiceItem>();

        public InvoiceItem? SelectedItem { get; set; }

        public ICommand AddItemCommand { get; set; }
        public ICommand RemoveItemCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public OutgoingInvoiceWindowViewModel(Window window, ServiceInvoices serviceInvoices)
        {
            _window = window;
            _serviceInvoices = serviceInvoices;

            AddItemCommand = new RelayCommand(AddItem);
            RemoveItemCommand = new RelayCommand(RemoveItem);
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        public void AddItem(object? obj)
        {
            AddInvoiceItemWindow addWindow = new AddInvoiceItemWindow(
                new ServiceProducts(_serviceInvoices.GetContext(), _serviceInvoices.GetWarehouse()));

            if (addWindow.ShowDialog() == true && addWindow.ResultItem != null)
                Items.Add(addWindow.ResultItem);
        }

        public void RemoveItem(object? obj)
        {
            if (SelectedItem != null)
                Items.Remove(SelectedItem);
            else
                MessageBox.Show("Объект для удаления не выбран", "Внимание");
        }

        public void Save(object? obj)
        {
            if (Items.Count == 0)
            {
                MessageBox.Show("В накладной нечего сохранять", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Invoice invoice = new Invoice()
            {
                Type = InvoiceType.Outgoing,
                WhsId = _serviceInvoices.GetWarehouse().Id,
                Items = Items.ToList()
            };

            if (!_serviceInvoices.AddInvoice(invoice))
            {
                MessageBox.Show("Ошибка при сохранении накладной", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ServiceProducts serviceProducts = new ServiceProducts(_serviceInvoices.GetContext(), _serviceInvoices.GetWarehouse());

            foreach (InvoiceItem item in Items)
            {
                serviceProducts.AddInvoiceItem(item, InvoiceType.Outgoing);
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