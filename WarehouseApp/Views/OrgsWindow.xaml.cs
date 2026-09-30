using System.Windows;
using WarehouseApp.Data.Services;
using WarehouseApp.ViewModels;

namespace WarehouseApp.Views
{
    public partial class OrgsView : Window
    {
        public OrgsView(ServiceOrgs service)
        {
            InitializeComponent();

            DataContext = new OrgsViewModel(service);
        }
    }
}