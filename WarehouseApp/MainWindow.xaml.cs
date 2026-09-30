using System.Windows;
using WarehouseApp.Data.Services;
using WarehouseApp.ViewModels;

namespace WarehouseApp
{
    public partial class MainWindow : Window
    {
        public MainWindow(string orgName, ServiceWhs serviceWhs)
        {
            InitializeComponent();

            Title += $" - {orgName}";

            DataContext = new MainWindowViewModel(this, serviceWhs);
        }
    }
}