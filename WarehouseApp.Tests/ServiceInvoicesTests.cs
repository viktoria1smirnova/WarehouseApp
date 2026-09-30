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
    public class ServiceInvoicesTests
    {
        private readonly ApplicationContext _context;
        private readonly Warehouse _warehouse;
        private readonly ServiceInvoices _service;

        public ServiceInvoicesTests()
        {
            _context = new ApplicationContext
            {
                Invcs = new ObservableCollection<Invoice>()
            };

            _warehouse = new Warehouse("Main WH", "Address", 1);

            _service = new ServiceInvoices(_context, _warehouse);
        }

        private Invoice CreateInvoice()
        {
            var invoice = new Invoice(_warehouse)
            {
                Type = InvoiceType.Incoming
            };

            invoice.Items.Add(new InvoiceItem()
            {
                Name = "Product1",
                Article = "A001",
                Unit = "pcs",
                Quantity = 5,
                Price = 100
            });

            return invoice;
        }

        [Fact]
        public void GetAll_ReturnsWarehouseInvoices()
        {
            var invoice1 = CreateInvoice();

            var invoice2 = CreateInvoice();
            invoice2.WhsId = 999;

            _context.Invcs.Add(invoice1);
            _context.Invcs.Add(invoice2);

            var result = _service.GetAll();

            Assert.Single(result);
            Assert.Contains(invoice1, result);
        }

        [Fact]
        public void AddInvoice_ValidInvoice_ReturnsTrue()
        {
            var invoice = CreateInvoice();

            var result = _service.AddInvoice(invoice);

            Assert.True(result);
            Assert.Contains(invoice, _context.Invcs);
        }

        [Fact]
        public void AddInvoice_EmptyItems_ReturnsFalse()
        {
            var invoice = new Invoice(_warehouse);

            var result = _service.AddInvoice(invoice);

            Assert.False(result);
        }

        [Fact]
        public void DelInvoice_ExistingInvoice_ReturnsTrue()
        {
            var invoice = CreateInvoice();

            _context.Invcs.Add(invoice);

            var result = _service.DelInvoice(invoice);

            Assert.True(result);
            Assert.Empty(_context.Invcs);
        }

        [Fact]
        public void DelInvoice_NotExistingInvoice_ReturnsFalse()
        {
            var invoice = CreateInvoice();

            var result = _service.DelInvoice(invoice);

            Assert.False(result);
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
