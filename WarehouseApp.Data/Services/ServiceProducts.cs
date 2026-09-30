using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Text;
using WarehouseApp.Data.Context;
using WarehouseApp.Data.Enums;
using WarehouseApp.Data.Models;

namespace WarehouseApp.Data.Services
{
    public interface IServiceProducts
    {
        ObservableCollection<Product> GetAll();
        bool AddProduct(Product product);
        bool EditProduct(Product product);
        bool DelProduct(Product product);
    }

    public class ServiceProducts : IServiceProducts
    {
        private ApplicationContext _context;

        private Warehouse _whs { get; set; }

        public ServiceProducts(ApplicationContext context, Warehouse whs)
        {
            _context = context;
            this._whs = whs;
        }

        public ObservableCollection<Product> GetAll()
        {
            return new ObservableCollection<Product>(this._context.Prdcts
                .Where(x => x.WhsId == this._whs.Id)
                .ToList());
        }

        public ObservableCollection<Product> GetAll(string filterRaw)
        {
            string filter = filterRaw.Trim().ToLower();

            if (string.IsNullOrEmpty(filter))
                return this.GetAll();

            return new ObservableCollection<Product>(this._context.Prdcts
                .Where(x => x.WhsId == this._whs.Id 
                && (x.Name.ToLower().Contains(filter) 
                || (x.Manufacturer?.Name ?? string.Empty).ToLower().Contains(filter)
                || (x.Supplier?.Name ?? string.Empty).ToLower().Contains(filter)
                || (x.Category?.Name ?? string.Empty).ToLower().Contains(filter)))
                .ToList());
        }

        public bool AddProduct(Product product)
        {
            bool result = false;
            if (product != null)
            {
                if (!string.IsNullOrWhiteSpace(product.Name))
                {
                    if (!this._context.Prdcts.Any(p => p.Compare(product)))
                    {
                        this._context.Prdcts.Add(product);
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool EditProduct(Product product)
        {
            bool result = false;
            if (product != null)
            {
                if (!string.IsNullOrWhiteSpace(product.Name))
                {
                    Product? target = this._context.Prdcts
                        .FirstOrDefault(p => p.Compare(product));

                    if (target != null)
                    {
                        int index = this._context.Prdcts.IndexOf(target);
                        if (index != -1)
                        {
                            this._context.Prdcts[index] = product;
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        public bool DelProduct(Product product)
        {
            bool result = false;
            if (product != null)
            {
                if (!string.IsNullOrWhiteSpace(product.Name))
                {
                    Product? target = this._context.Prdcts
                        .FirstOrDefault(p => p.Compare(product));

                    if (target != null)
                    {
                        this._context.Prdcts.Remove(target);
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool AddInvoiceItem(InvoiceItem item, InvoiceType type)
        {
            bool result = false;
            if (!string.IsNullOrWhiteSpace(item.Name))
            {
                Product? product = _context.Prdcts
                        .FirstOrDefault(x => x.Compare(item, _whs.Id));

                if (type == InvoiceType.Outgoing)
                {
                    if (product != null)
                    {
                        product.StockQuantity -= item.Quantity;

                        if (product.StockQuantity > 0)
                        {
                            result = EditProduct(product);
                        }
                        else
                        {
                            result = DelProduct(product);
                        }
                    }
                }
                else
                {
                    if (product == null)
                    {
                        if (item.Quantity > 0)
                        {
                            product = new Product()
                            {
                                Article = item.Article,
                                Name = item.Name,
                                Unit = item.Unit,
                                Price = item.Price,
                                StockQuantity = item.Quantity,
                                DiscountPercent = item.DiscountPercent,
                                Description = item.Description,
                                PhotoPath = item.PhotoPath,
                                Category = new Category() { Name = item?.CategoryName ?? string.Empty, },
                                Manufacturer = new Manufacturer() { Name = item?.ManufacturerName ?? string.Empty, },
                                Supplier = new Supplier() { Name = item?.SupplierName ?? string.Empty, },
                                WhsId = _whs.Id,
                            };

                            result = AddProduct(product);
                        }
                    }
                    else
                    {
                        product.StockQuantity += item.Quantity;
                        if (product.StockQuantity > 0)
                        {
                            result = EditProduct(product);
                        }
                        else 
                        { 
                            result = DelProduct(product); 
                        }
                    }
                }  
            }

            return result;
        }

        public ApplicationContext GetContext()
        {
            return this._context;
        }

        public Warehouse GetWarehouse()
        {
            return this._whs;
        }
    }
}
