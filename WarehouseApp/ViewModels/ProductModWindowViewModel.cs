using System.Windows;
using System.Windows.Input;
using WarehouseApp.Data.Models;
using WarehouseApp.Data.Services;

namespace WarehouseApp.ViewModels
{
    public class ProductModWindowViewModel : BaseViewModel
    {
        private ServiceProducts _serviceProducts;
        private Product? _editingProduct;
        private Window _window;

        private bool IsEdit => _editingProduct != null;

        public Product? CreatedProduct { get; private set; }

        public string Article { get; set; } = "";
        public string Name { get; set; } = "";
        public string Unit { get; set; } = "";
        public string Price { get; set; } = "";
        public string Discount { get; set; } = "0";
        public string CategoryName { get; set; } = "";
        public string ManufacturerName { get; set; } = "";
        public string SupplierName { get; set; } = "";
        public string PhotoPath { get; set; } = "";
        public string Description { get; set; } = "";
        public string Quantity { get; set; } = "";

        public bool QuantityReadOnly => IsEdit;

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ProductModWindowViewModel(Window window, ServiceProducts serviceProducts, Product? product = null)
        {
            _window = window;
            _serviceProducts = serviceProducts;
            _editingProduct = product;

            if (IsEdit)
                LoadProductData();

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void LoadProductData()
        {
            if (_editingProduct == null) return;

            Article = _editingProduct.Article;
            Name = _editingProduct.Name;
            Unit = _editingProduct.Unit;
            Price = _editingProduct.Price.ToString("0.##");
            Discount = _editingProduct.DiscountPercent.ToString("0.##");
            SupplierName = _editingProduct.SupplierName;
            PhotoPath = _editingProduct.PhotoPath ?? "";
            Description = _editingProduct.Description ?? "";
            Quantity = _editingProduct.StockQuantity.ToString();
            CategoryName = _editingProduct.Category?.Name ?? "";
            ManufacturerName = _editingProduct.Manufacturer?.Name ?? "";
        }

        public void Save(object? obj)
        {
            if (string.IsNullOrWhiteSpace(Article) || string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Price) || string.IsNullOrWhiteSpace(Quantity))
            {
                MessageBox.Show("Все поля, помеченные символом \"*\", должны быть заполнены", "Ошибка");
                return;
            }

            int quantity = 1;
            decimal price = 0;
            decimal discount = 0;

            if (!decimal.TryParse(Price, out price) ||
                !decimal.TryParse(Discount, out discount) ||
                !int.TryParse(Quantity, out quantity) || quantity <= 0)
            {
                MessageBox.Show("Поля \"Цена\", \"Скидка\", \"Количество\" должны быть заполнены корректно", "Ошибка");
                return;
            }

            Product product = _editingProduct ?? new Product();

            product.Article = Article;
            product.Name = Name;
            product.Unit = Unit;
            product.Price = price;
            product.DiscountPercent = discount;
            product.SupplierName = SupplierName;
            product.PhotoPath = PhotoPath;
            product.Description = Description;
            product.StockQuantity = quantity;
            product.WhsId = _serviceProducts.GetWarehouse().Id;

            product.Category = new Category() { Name = CategoryName };
            product.Manufacturer = new Manufacturer() { Name = ManufacturerName };
            product.Supplier = new Supplier() { Name = SupplierName };

            bool success = !IsEdit || _serviceProducts.EditProduct(product);

            if (!success)
            {
                MessageBox.Show("Не удалось сохранить товар.", "Ошибка");
                return;
            }

            CreatedProduct = product;
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