using System.Windows;
using WarehouseApp.Data.Services;
using WarehouseApp.ViewModels;

namespace WarehouseApp.Views
{
    public partial class IncomingInvoiceWindow : Window
    {
        public IncomingInvoiceWindow(ServiceInvoices serviceInvoice)
        {
            InitializeComponent();

            Title = $"Приходная накладная — {serviceInvoice.GetWarehouse().WhName}";

            DataContext = new IncomingInvoiceWindowViewModel(this, serviceInvoice);
        }
    }
}