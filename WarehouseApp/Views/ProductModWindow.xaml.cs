using System.Windows;
using WarehouseApp.Data.Models;
using WarehouseApp.Data.Services;
using WarehouseApp.ViewModels;

namespace WarehouseApp.Views
{
    public partial class ProductModWindow : Window
    {
        public ProductModWindow(
            ServiceProducts serviceProducts)
        {
            InitializeComponent();

            Title = "Новый товар";

            DataContext = new ProductModWindowViewModel(this, serviceProducts);
        }

        public ProductModWindow(Product productToEdit, ServiceProducts serviceProducts)
        {
            InitializeComponent();

            Title = "Редактирование товара";

            DataContext = new ProductModWindowViewModel(this, serviceProducts, productToEdit);
        }

        public Product? CreatedProduct =>
            (DataContext as ProductModWindowViewModel)?.CreatedProduct;
    }
}