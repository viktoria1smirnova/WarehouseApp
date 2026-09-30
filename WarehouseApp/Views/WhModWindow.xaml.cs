using System.Windows;
using WarehouseApp.Data.Models;
using WarehouseApp.Data.Services;
using WarehouseApp.ViewModels;

namespace WarehouseApp.Views
{
    public partial class WhModWindow : Window
    {
        public WhModWindow(string title, ServiceWhs service, Warehouse? whs)
        {
            InitializeComponent();

            Title = title;

            DataContext = new WhModWindowViewModel(this, service, whs);
        }
    }
}