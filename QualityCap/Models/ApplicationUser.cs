using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityCap.Models
{
    public class ApplicationUser: IdentityUser
    {
        public bool Enabled { get; set; }

        public string LastName { get; set; }

        public string FirstMidName { get; set; }

        public string FullName
        {
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }

        public string Address { get; set; }
    }
}
