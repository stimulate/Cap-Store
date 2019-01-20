using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using QualityCap.Models;
using System.Threading.Tasks;

namespace QualityCap.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
           // Look for a category
            if (context.Categories.Any())
                {
                    return;  //DB has been seeded
                }

            //var dbcreated = context.Database.EnsureCreated();

            //if (dbcreated)
            //{
            //    Console.WriteLine("new db created.");
            //}
            //else
            //{
            //    Console.WriteLine("db already created.");
            //}

            //var dbCount = -1;
            //try
            //{
            //    dbCount = context.Categories.Count();
            //}
            //catch (Exception ex)
            //{
            //    dbCount = -1;
            //    // TODO: create tables
            //}

            //if (dbCount > 0)
            //{
            //    return;
            //}           

            var suppliers = new Supplier[]
           {
                new Supplier{Name = "WoolPress",
                    PhoneNum = "09-2449955", Email="admin@woolpress.co.nz"},
                new Supplier{Name = "IceBreaker",
                    PhoneNum = "05-22429953", Email="admin@icebreaker.co.nz"},
                new Supplier{Name = "Kathmandu",
                    PhoneNum = "06-1429953", Email="admin@kathmandu.co.nz"},

           };

            foreach (Supplier s in suppliers)
            {
                context.Suppliers.Add(s);
            }
            context.SaveChanges();

            var categories = new Category[]
            {
                new Category {Name = "COWBOY HATS", Description = "The cowboy hat is a high-crowned, wide-brimmed hat best known as the defining piece of attire for the North American cowboy.",
                },
                new Category {Name = "VISORS", Description = "A visor (also spelled vizor) is a surface that protects the eyes, such as shading them from the sun or other bright light or protecting them from objects.",
                },
                new Category {Name = "Boater Hat", Description = "A boater is a kind of summer hat worn by men, regarded as somewhat formal, and particularly popular in the late 19th century and early 20th century.",
                },
                new Category {Name = "BEANIES", Description = "A beanie is a knit cap, originally of wool (though now often of synthetic fibers) is designed to provide warmth in cold weather.",
                },
                 new Category {Name = "CAP", Description = "A cap is a form of headgear. Caps have crowns that fit very close to the head. They are typically designed for warmth and, when including a visor, blocking sunlight from the eyes. They come in many shapes and sizes.",
                },
                new Category {Name = "Fedora", Description = "A fedora is a hat with a soft brim and indented crown. It is typically creased lengthwise down the crown and \"pinched\" near the front on both sides.Fedoras can also be creased with teardrop crowns, diamond crowns, center dents, and others, and the positioning of pinches can vary. The typical crown height is 4.5 inches (11 cm).",
                },
                new Category {Name = "Stetson", Description = "Stetson created a rugged hat for himself made from thick beaver felt while panning for gold in Colorado. According to legend, Stetson invented the hat while on a hunting trip while showing his companions how he could make cloth out of fur without tanning. Fur felt hats are lighter, they maintain their shape, and withstand weather and renovation better.",
                },
            };

            foreach (Category c in categories)
            {
                context.Categories.Add(c);
            }
            context.SaveChanges();

            var caps = new Cap[]
            {
                new Cap{Name="Scala Classico Men's Crushable Felt Outback Hat",Description="100% Wool Felt" + Environment.NewLine
                + "Wool hat featuring wide brim and faux-leather band with feather accent" + Environment.NewLine
                + "Water-repellent crushable construction",Price = 27.2, Image = "~/images/cap/c01.jpg", 
                CategoryID = categories.Single(c => c.Name == "COWBOY HATS" ).CategoryID,
                SupplierID = suppliers.Single(s => s.Name == "WoolPress" ).SupplierID},

                new Cap{Name="UwantC Mens Faux Felt Western Cowboy Hat Fedora Outdoor Wide Brim Hat with Strap",Description="The new fashion Faux Felt cowboy caps men western hats." + Environment.NewLine
                + "Size: one size,can be adjusted size" + Environment.NewLine
                + "Good for Tourist, Fashion Hipster, Cosplay.",Price = 11.89, Image = "~/images/cap/c02.jpg",
                CategoryID = categories.Single(c => c.Name == "COWBOY HATS" ).CategoryID,
                SupplierID = suppliers.Single(s => s.Name == "WoolPress" ).SupplierID},

                new Cap{Name="Fedora Outdoor Hat ",Description="The new fashion Faux Felt cowboy caps men western hats." + Environment.NewLine
                + "Size: one size,can be adjusted size" + Environment.NewLine
                + "Good for Tourist, Fashion Hipster, Cosplay.",Price = 12.0, Image = "~/images/cap/c05.jpeg",
                CategoryID = 1, SupplierID = 1},
                 new Cap{Name="Fedora Outdoor Hat ",Description="The new fashion Faux Felt cowboy caps men western hats." + Environment.NewLine
                + "Size: one size,can be adjusted size" + Environment.NewLine
                + "Good for Tourist, Fashion Hipster, Cosplay.",Price = 12.0, Image = "~/images/cap/boater1.jpeg",
                CategoryID = 2, SupplierID = 1},
                  new Cap{Name="Fedora Outdoor Hat ",Description="The new fashion Faux Felt cowboy caps men western hats." + Environment.NewLine
                + "Size: one size,can be adjusted size" + Environment.NewLine
                + "Good for Tourist, Fashion Hipster, Cosplay.",Price = 12.0, Image = "~/images/cap/boater2.jpeg",
                CategoryID = 3, SupplierID = 1},
                   new Cap{Name="Fedora Outdoor Hat ",Description="The new fashion Faux Felt cowboy caps men western hats." + Environment.NewLine
                + "Size: one size,can be adjusted size" + Environment.NewLine
                + "Good for Tourist, Fashion Hipster, Cosplay.",Price = 12.0, Image = "~/images/cap/c03.jpeg",
                CategoryID = 2, SupplierID = 1},
                   new Cap{Name="Fedora Outdoor Hat ",Description="The new fashion Faux Felt cowboy caps men western hats." + Environment.NewLine
                + "Size: one size,can be adjusted size" + Environment.NewLine
                + "Good for Tourist, Fashion Hipster, Cosplay.",Price = 12.0, Image = "~/images/cap/c04.jpeg",
                CategoryID = 4, SupplierID = 1},
                   new Cap{Name="Fedora Outdoor Hat ",Description="The new fashion Faux Felt cowboy caps men western hats." + Environment.NewLine
                + "Size: one size,can be adjusted size" + Environment.NewLine
                + "Good for Tourist, Fashion Hipster, Cosplay.",Price = 12.0, Image = "~/images/cap/c06.jpeg",
                CategoryID = 3, SupplierID = 1},
                   new Cap{Name="Fedora Outdoor Hat ",Description="The new fashion Faux Felt cowboy caps men western hats." + Environment.NewLine
                + "Size: one size,can be adjusted size" + Environment.NewLine
                + "Good for Tourist, Fashion Hipster, Cosplay.",Price = 12.0, Image = "~/images/cap/c07.jpeg",
                CategoryID = 2, SupplierID = 1},
                   new Cap{Name="Fedora Outdoor Hat ",Description="The new fashion Faux Felt cowboy caps men western hats." + Environment.NewLine
                + "Size: one size,can be adjusted size" + Environment.NewLine
                + "Good for Tourist, Fashion Hipster, Cosplay.",Price = 12.0, Image = "~/images/cap/cap1.jpg",
                CategoryID = 6, SupplierID = 1},
                   new Cap{Name="Fedora Outdoor Hat ",Description="The new fashion Faux Felt cowboy caps men western hats." + Environment.NewLine
                + "Size: one size,can be adjusted size" + Environment.NewLine
                + "Good for Tourist, Fashion Hipster, Cosplay.",Price = 12.0, Image = "~/images/cap/hat.jpeg",
                CategoryID = 7, SupplierID = 1},
                   new Cap{Name="Fedora Outdoor Hat ",Description="The new fashion Faux Felt cowboy caps men western hats." + Environment.NewLine
                + "Size: one size,can be adjusted size" + Environment.NewLine
                + "Good for Tourist, Fashion Hipster, Cosplay.",Price = 12.0, Image = "~/images/cap/hat2.jpeg",
                CategoryID = 5, SupplierID = 1},
                   new Cap{Name="Fedora Outdoor Hat ",Description="The new fashion Faux Felt cowboy caps men western hats." + Environment.NewLine
                + "Size: one size,can be adjusted size" + Environment.NewLine
                + "Good for Tourist, Fashion Hipster, Cosplay.",Price = 12.0, Image = "~/images/cap/leather_fiddler.jpg",
                CategoryID = 3, SupplierID = 1},
        };
            foreach (Cap c in caps)
            {
                context.Caps.Add(c);
            }
            context.SaveChanges();

        }
    }
}
