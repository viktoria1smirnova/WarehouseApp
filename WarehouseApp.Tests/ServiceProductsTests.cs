using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WarehouseApp.Data.Context;
using WarehouseApp.Data.Enums;
using WarehouseApp.Data.Models;
using WarehouseApp.Data.Services;
using Xunit;

namespace WarehouseApp.Tests
{
    public class ServiceProductsTests
    {
        private readonly ApplicationContext _context;
        private readonly Warehouse _warehouse;
        private readonly ServiceProducts _service;

        public ServiceProductsTests()
        {
            _context = new ApplicationContext
            {
                Prdcts = new ObservableCollection<Product>()
            };

            _warehouse = new Warehouse("Main WH", "Address", 1);

            _service = new ServiceProducts(_context, _warehouse);
        }

        private Product CreateProduct()
        {
            return new Product()
            {
                Article = "A001",
                Name = "Product1",
                Unit = "pcs",
                Price = 100,
                DiscountPercent = 0,
                StockQuantity = 10,
                Description = "Description",
                PhotoPath = "photo.png",
                WhsId = _warehouse.Id,
                Category = new Category() { Name = "Category1" },
                Manufacturer = new Manufacturer() { Name = "Manufacturer1" },
                Supplier = new Supplier() { Name = "Supplier1" }
            };
        }

        [Fact]
        public void GetAll_ReturnsWarehouseProducts()
        {
            var p1 = CreateProduct();

            var p2 = CreateProduct();
            p2.Id = 999;
            p2.WhsId = 999;

            _context.Prdcts.Add(p1);
            _context.Prdcts.Add(p2);

            var result = _service.GetAll();

            Assert.Single(result);
            Assert.Contains(p1, result);
        }

        [Fact]
        public void GetAll_Filter_ReturnsFilteredProducts()
        {
            var p1 = CreateProduct();

            var p2 = CreateProduct();
            p2.Id = 2;
            p2.Name = "Another";
            p2.Article = "A002";

            _context.Prdcts.Add(p1);
            _context.Prdcts.Add(p2);

            var result = _service.GetAll("product");

            Assert.Single(result);
            Assert.Equal("Product1", result[0].Name);
        }

        [Fact]
        public void AddProduct_Valid_ReturnsTrue()
        {
            var product = CreateProduct();

            var result = _service.AddProduct(product);

            Assert.True(result);
            Assert.Contains(product, _context.Prdcts);
        }

        [Fact]
        public void AddProduct_Null_ReturnsFalse()
        {
            var result = _service.AddProduct(null);

            Assert.False(result);
        }

        [Fact]
        public void AddProduct_EmptyName_ReturnsFalse()
        {
            var product = CreateProduct();
            product.Name = "";

            var result = _service.AddProduct(product);

            Assert.False(result);
        }

        [Fact]
        public void AddProduct_Duplicate_ReturnsFalse()
        {
            var product = CreateProduct();

            _context.Prdcts.Add(product);

            var result = _service.AddProduct(product);

            Assert.False(result);
        }

        [Fact]
        public void EditProduct_Existing_ReturnsTrue()
        {
            var product = CreateProduct();

            _context.Prdcts.Add(product);

            product.Name = "Updated";

            var result = _service.EditProduct(product);

            Assert.True(result);
            Assert.Equal("Updated", _context.Prdcts[0].Name);
        }

        [Fact]
        public void EditProduct_NotExisting_ReturnsFalse()
        {
            var product = CreateProduct();

            var result = _service.EditProduct(product);

            Assert.False(result);
        }

        [Fact]
        public void DelProduct_Existing_ReturnsTrue()
        {
            var product = CreateProduct();

            _context.Prdcts.Add(product);

            var result = _service.DelProduct(product);

            Assert.True(result);
            Assert.Empty(_context.Prdcts);
        }

        [Fact]
        public void DelProduct_NotExisting_ReturnsFalse()
        {
            var product = CreateProduct();

            var result = _service.DelProduct(product);

            Assert.False(result);
        }

        [Fact]
        public void AddInvoiceItem_Incoming_NewProduct_ReturnsTrue()
        {
            var item = new InvoiceItem()
            {
                Article = "A001",
                Name = "Product1",
                Unit = "pcs",
                Price = 100,
                Quantity = 5,
                DiscountPercent = 0,
                Description = "Description",
                PhotoPath = "photo.png",
                CategoryName = "Category1",
                ManufacturerName = "Manufacturer1",
                SupplierName = "Supplier1"
            };

            var result = _service.AddInvoiceItem(item, InvoiceType.Incoming);

            Assert.True(result);
            Assert.Single(_context.Prdcts);
            Assert.Equal(5, _context.Prdcts[0].StockQuantity);
        }

        [Fact]
        public void AddInvoiceItem_Outgoing_ProductReduced_ReturnsTrue()
        {
            var product = CreateProduct();

            _context.Prdcts.Add(product);

            var item = new InvoiceItem()
            {
                ProductId = product.Id,
                Article = product.Article,
                Name = product.Name,
                Unit = product.Unit,
                Price = product.Price,
                Quantity = 5,
                DiscountPercent = product.DiscountPercent,
                Description = product.Description,
                PhotoPath = product.PhotoPath,
                CategoryName = product.Category.Name,
                ManufacturerName = product.Manufacturer.Name,
                SupplierName = product.Supplier.Name
            };

            var result = _service.AddInvoiceItem(item, InvoiceType.Outgoing);

            Assert.True(result);
            Assert.Equal(5, _context.Prdcts[0].StockQuantity);
        }

        [Fact]
        public void AddInvoiceItem_Outgoing_ProductRemoved_ReturnsTrue()
        {
            var product = CreateProduct();
            product.StockQuantity = 5;

            _context.Prdcts.Add(product);

            var item = new InvoiceItem()
            {
                ProductId = product.Id,
                Article = product.Article,
                Name = product.Name,
                Unit = product.Unit,
                Price = product.Price,
                Quantity = 5,
                DiscountPercent = product.DiscountPercent,
                Description = product.Description,
                PhotoPath = product.PhotoPath,
                CategoryName = product.Category.Name,
                ManufacturerName = product.Manufacturer.Name,
                SupplierName = product.Supplier.Name
            };

            var result = _service.AddInvoiceItem(item, InvoiceType.Outgoing);

            Assert.True(result);
            Assert.Empty(_context.Prdcts);
        }

        [Fact]
        public void GetContext_ReturnsContext()
        {
            var result = _service.GetContext();

            Assert.Equal(_context, result);
        }

        [Fact]
        public void GetWarehouse_ReturnsWarehouse()
        {
            var result = _service.GetWarehouse();

            Assert.Equal(_warehouse, result);
        }
    }
}
