using System;
using System.Collections.ObjectModel;
using System.Linq;
using WarehouseApp.Data.Context;
using WarehouseApp.Data.Models;
using WarehouseApp.Data.Services;
using Xunit;

namespace WarehouseApp.Tests
{
    public class ServiceWhsTests
    {
        private readonly ApplicationContext _context;
        private readonly Organization _org;
        private readonly ServiceWhs _service;

        public ServiceWhsTests()
        {
            _context = new ApplicationContext
            {
                Orgs = new ObservableCollection<Organization>(),
                Whs = new ObservableCollection<Warehouse>()
            };

            _org = new Organization("Test Organization")
            {
                Id = 1
            };

            _context.Orgs.Add(_org);

            _service = new ServiceWhs(_context, _org);
        }

        [Fact]
        public void GetAll_ReturnsOnlyOrganizationWarehouses()
        {
            var wh1 = new Warehouse("WH1", "Address1", _org.Id);
            var wh2 = new Warehouse("WH2", "Address2", _org.Id);
            var wh3 = new Warehouse("WH3", "Address3", 999);

            _context.Whs.Add(wh1);
            _context.Whs.Add(wh2);
            _context.Whs.Add(wh3);

            var result = _service.GetAll();

            Assert.Equal(2, result.Count);
            Assert.Contains(wh1, result);
            Assert.Contains(wh2, result);
            Assert.DoesNotContain(wh3, result);
        }

        [Fact]
        public void AddWhs_ValidWarehouse_ReturnsTrue()
        {
            var wh = new Warehouse("Main WH", "Some address", _org.Id);

            var result = _service.AddWhs(wh);

            Assert.True(result);
            Assert.Contains(wh, _context.Whs);
        }

        [Fact]
        public void AddWhs_NullWarehouse_ReturnsFalse()
        {
            var result = _service.AddWhs(null);

            Assert.False(result);
        }

        [Fact]
        public void AddWhs_EmptyName_ReturnsFalse()
        {
            var wh = new Warehouse
            {
                Id = 1,
                WhName = "",
                WhAddress = "Address",
                OrgId = _org.Id
            };

            var result = _service.AddWhs(wh);

            Assert.False(result);
        }

        [Fact]
        public void AddWhs_DuplicateWarehouse_ReturnsFalse()
        {
            var wh = new Warehouse("Main WH", "Address", _org.Id);

            _context.Whs.Add(wh);

            var result = _service.AddWhs(wh);
            Assert.False(result);
        }

        [Fact]
        public void DelWhs_ExistingWarehouse_ReturnsTrue()
        {
            var wh = new Warehouse("Main WH", "Address", _org.Id);

            _context.Whs.Add(wh);
            var result = _service.DelWhs(wh);

            Assert.True(result);
            Assert.DoesNotContain(wh, _context.Whs);
        }

        [Fact]
        public void DelWhs_NotExistingWarehouse_ReturnsFalse()
        {
            var wh = new Warehouse("Main WH", "Address", _org.Id);

            var result = _service.DelWhs(wh);

            Assert.False(result);
        }

        [Fact]
        public void EditWhs_ExistingWarehouse_ReturnsTrue()
        {
            var oldWh = new Warehouse("Old Name", "Old Address", _org.Id);

            _context.Whs.Add(oldWh);

            var updatedWh = new Warehouse
            {
                Id = oldWh.Id,
                WhName = "New Name",
                WhAddress = "New Address",
                OrgId = _org.Id
            };

            var result = _service.EditWhs(updatedWh);

            Assert.True(result);
            Assert.Equal("New Name", _context.Whs[0].WhName);
            Assert.Equal("New Address", _context.Whs[0].WhAddress);
        }

        [Fact]
        public void EditWhs_NotExistingWarehouse_ReturnsFalse()
        {
            var wh = new Warehouse
            {
                Id = 999,
                WhName = "Test",
                WhAddress = "Address",
                OrgId = _org.Id
            };

            var result = _service.EditWhs(wh);

            Assert.False(result);
        }

        [Fact]
        public void GetContext_ReturnsContext()
        {
            var result = _service.GetContext();
            Assert.Equal(_context, result);
        }

        [Fact]
        public void GetOrganization_ReturnsOrganization()
        {
            var result = _service.GetOrganization();
            Assert.Equal(_org, result);
        }
    }
}
