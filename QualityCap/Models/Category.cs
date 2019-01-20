using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QualityCap.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Cap> Caps { get; set; }
    }
}
