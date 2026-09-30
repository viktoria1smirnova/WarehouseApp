using System.Windows.Controls;
using System.Windows.Input;
using WarehouseApp.Data.Services;
using WarehouseApp.ViewModels;

namespace WarehouseApp.Controls
{
    public partial class UserControllWareHouses : UserControl
    {
        public UserControllWareHouses(WarehousesViewModel wvm)
        {
            InitializeComponent();

            DataContext = wvm;
        }

        private void DgProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is WarehousesViewModel wvm)
            {
                wvm.OpenProductCommand.Execute(
                    DgProducts.SelectedItem);
            }
        }
    }
}