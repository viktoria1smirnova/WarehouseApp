using System;
using System.Collections.ObjectModel;
using System.Linq;
using WarehouseApp.Data.Context;
using WarehouseApp.Data.Models;
using WarehouseApp.Data.Services;
using Xunit;

namespace WarehouseApp.Tests
{
    public class ServiceOrgsTests
    {
        private readonly ServiceOrgs _service;
        private readonly ApplicationContext _context;

        public ServiceOrgsTests()
        {
            _context = new ApplicationContext
            {
                Orgs = new ObservableCollection<Organization>()
            };

            _service = new ServiceOrgs(_context);
        }

        [Fact]
        public void GetAll_ReturnsAllOrganizations()
        {
            _context.Orgs.Add(new Organization("Org1") { Id = 1 });
            _context.Orgs.Add(new Organization("Org2") { Id = 2 });

            var result = _service.GetAll();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void AddOrg_ValidOrganization_ReturnsTrue()
        {
            var org = new Organization("TestOrg")
            {
                Id = 1,
            };

            var result = _service.AddOrg(org);

            Assert.True(result);
            Assert.Contains(org, _context.Orgs);
        }

        [Fact]
        public void AddOrg_NullOrganization_ReturnsFalse()
        {
            var result = _service.AddOrg(null);
            Assert.False(result);
        }

        [Fact]
        public void AddOrg_EmptyName_ReturnsFalse()
        {
            var org = new Organization("")
            {
                Id = 1,
            };

            var result = _service.AddOrg(org);
            Assert.False(result);
        }

        [Fact]
        public void AddOrg_DuplicateOrganization_ReturnsFalse()
        {
            var org = new Organization("Org")
            {
                Id = 1,
            };

            _context.Orgs.Add(org);

            var result = _service.AddOrg(org);
            Assert.False(result);
        }

        [Fact]
        public void DelOrg_ExistingOrganization_ReturnsTrue()
        {
            var org = new Organization("Org")
            {
                Id = 1,
            };

            _context.Orgs.Add(org);

            var result = _service.DelOrg(org);

            Assert.True(result);
            Assert.DoesNotContain(org, _context.Orgs);
        }

        [Fact]
        public void DelOrg_NotExistingOrganization_ReturnsFalse()
        {
            var org = new Organization("Org")
            {
                Id = 1,
            };

            var result = _service.DelOrg(org);

            Assert.False(result);
        }

        [Fact]
        public void EditOrg_ExistingOrganization_ReturnsTrue()
        {
            var oldOrg = new Organization("OldName")
            {
                Id = 1,
            };

            _context.Orgs.Add(oldOrg);

            var newOrg = new Organization("NewName")
            {
                Id = 1,
            };

            var result = _service.EditOrg(newOrg);

            Assert.True(result);
            Assert.Equal("NewName", _context.Orgs[0].OrgName);
        }

        [Fact]
        public void EditOrg_NotExistingOrganization_ReturnsFalse()
        {
            var org = new Organization("Org")
            {
                Id = 1,
            };

            var result = _service.EditOrg(org);

            Assert.False(result);
        }

        [Fact]
        public void GetContext_ReturnsContext()
        {
            var result = _service.GetContext();

            Assert.Equal(_context, result);
        }
    }
}
