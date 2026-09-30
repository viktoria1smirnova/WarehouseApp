using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;

namespace WarehouseApp.Data.Models
{
    public class Organization
    {
        public static BigInteger Counter = BigInteger.Zero;
        public BigInteger Id { get; set; }
        public string OrgName { get; set; }
        public ObservableCollection<Warehouse> warehouses { get; set; }
            = new ObservableCollection<Warehouse>();

        public Organization(string name)
        {
            Id = ++Counter;
            OrgName = name;
        }

    }
}
