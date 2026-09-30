using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using WarehouseApp.Data.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WarehouseApp.Data.Models
{
    public class Invoice
    {
        public static BigInteger Counter = BigInteger.Zero;
        public BigInteger Id { get; set; }
        public InvoiceType Type { get; set; }
        public BigInteger WhsId { get; set; }
        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();

        public Invoice()
        {
            Id = ++Counter;
        }

        public Invoice(Warehouse whs)
        {
            Id = ++Counter;
            this.WhsId = whs.Id;
        }
    }
}
