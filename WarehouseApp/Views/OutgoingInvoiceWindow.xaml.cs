using System.Windows;
using WarehouseApp.Data.Services;
using WarehouseApp.ViewModels;

namespace WarehouseApp.Views
{
    public partial class OutgoingInvoiceWindow : Window
    {
        public OutgoingInvoiceWindow(
            ServiceInvoices serviceInvoice)
        {
            InitializeComponent();

            Title = $"Расходная накладная — {serviceInvoice.GetWarehouse().WhName}";

            DataContext = new OutgoingInvoiceWindowViewModel(this, serviceInvoice);
        }
    }
}