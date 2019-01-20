using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QualityCap.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemID { get; set; }
        public int OrderID { get; set; }

        [Required]
        public int Quantity { get; set; }
        public int CapID { get; set; }
        public Order Order { get; set; }
        public Cap Cap { get; set; }
    }
}
