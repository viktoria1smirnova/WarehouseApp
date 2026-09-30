using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;

namespace WarehouseApp.Data.Models
{
    public class Warehouse
    {
        public static BigInteger Counter = BigInteger.Zero;
        public BigInteger Id { get; set; }
        public string WhName { get; set; }
        public string WhAddress { get; set; }
        public BigInteger OrgId { get; set; }
        Organization Organization { get; set; }
        public List<Product> products { get; set; }
            = new List<Product>();

        public Warehouse() { }

        public Warehouse(string name, string address, BigInteger orgid)
        {
            this.Id = ++Counter;
            this.WhName = name;
            this.WhAddress = address;
            this.OrgId = orgid;
        }
    }
}
