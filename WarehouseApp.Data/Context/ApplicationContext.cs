using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WarehouseApp.Data.Models;

namespace WarehouseApp.Data.Context
{
    public class ApplicationContext
    {
        public ObservableCollection<Organization> Orgs;
        public ObservableCollection<Warehouse> Whs;
        public ObservableCollection<Product> Prdcts;
        public ObservableCollection<Invoice> Invcs;

        public ApplicationContext()
        {
            Orgs = new ObservableCollection<Organization>();
            Whs = new ObservableCollection<Warehouse>();
            Prdcts = new ObservableCollection<Product>();
            Invcs = new ObservableCollection<Invoice>();

            OrgsFill();
        }

        private void OrgsFill()
        {
            Organization org1 = new Organization("Рога и копыта, ООО");
            Organization org2 = new Organization("Пупкин и сыновья, ООО");

            Warehouse wh1 = new Warehouse("Склад Рогов и Копыт №1", "Там, за лесом", org1.Id);
            Warehouse wh2 = new Warehouse("Склад Рогов и Копыт №2", "Там, за лесом", org1.Id);
            Warehouse wh3 = new Warehouse("Склад Пупкина №1", "Там, за горой", org2.Id);
            Warehouse wh4 = new Warehouse("Склад Пупкина №2", "Там, за горой", org2.Id);

            Product product1 = new Product()
            {
                Article = "Артикль",
                Name = "Рога",
                WhsId = wh1.Id,
                Price = 120,
                StockQuantity = 100,
                DiscountPercent = 10,
                Description = "Описание",
                Supplier = new Supplier() { Name = "Supplier" },
                Manufacturer = new Manufacturer() { Name = "Manufacturer" },
                Category = new Category() { Name = "Категория" }
            };

            Product product2 = new Product()
            {
                Article = "Артикль 2",
                Name = "Копыта",
                WhsId = wh1.Id,
                Price = 120,
                StockQuantity = 100,
                DiscountPercent = 10,
                Description = "Описание 2",
                Supplier = new Supplier() { Name = "Supplier 2" },
                Manufacturer = new Manufacturer() { Name = "Manufacturer 2" },
                Category = new Category() { Name = "Категория 2" }
            };

            Orgs.Add(org1);
            Orgs.Add(org2);
            Whs.Add(wh1);
            Whs.Add(wh2);
            Whs.Add(wh3);
            Whs.Add(wh4);
            Prdcts.Add(product1);
            Prdcts.Add(product2);
        }
    }
}
