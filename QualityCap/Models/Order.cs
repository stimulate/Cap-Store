using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QualityCap.Models
{
    public enum Status { Current, Paid, Shipped }
    public class Order
    {
       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        public string PostalCode { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string Phone { get; set; }
        public int OrderID { get; set; }
        
        [DisplayFormat(NullDisplayText = "No order")]        
        public Status? Status { get; set; }
        public String OrderDate { get; set; }

        
        public double Subtotal { get; set; }
        
        public double GST { get; set; }
       
        public double GrandTotal { get; set; }        
        public ICollection<OrderItem> OrderItems { get; set; }
        public ApplicationUser User { get; set; }
    }
}
