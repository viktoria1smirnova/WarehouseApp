using System.Windows;
using WarehouseApp.Data.Models;
using WarehouseApp.Data.Services;
using WarehouseApp.ViewModels;

namespace WarehouseApp.Views
{
    public partial class AddInvoiceItemWindow : Window
    {
        public InvoiceItem? ResultItem =>
            (DataContext as AddInvoiceItemViewModel)?.ResultItem;

        public AddInvoiceItemWindow(ServiceProducts service)
        {
            InitializeComponent();

            DataContext = new AddInvoiceItemViewModel(this, service);
        }
    }
}