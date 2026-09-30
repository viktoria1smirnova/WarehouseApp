using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WarehouseApp.Data.Models
{
    public class Manufacturer
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public Manufacturer() { }
    }
}
