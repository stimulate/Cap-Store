using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QualityCap.Models
{
    public class Cap
    {
        public int CapID { get; set; }
        [Display(Name = "Cap Name")] 
        [Required(ErrorMessage = "Please enter a cap name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }
        public string Image { get; set; }
        [Display(Name = "Price")]
        [Required(ErrorMessage = "Please enter a price")]
        public double Price { get; set; }
        public int SupplierID { get; set; }
        public Category Category { get; set; }
        public Supplier Supplier { get; set; }

    }
}
