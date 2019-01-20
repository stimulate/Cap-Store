using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using QualityCap.Data;
using QualityCap.Models;

namespace QualityCap.Controllers
{

       [Authorize(Roles = "Admin,Member")]
        public class OrdersController : Controller
        {
            private readonly ApplicationDbContext _context;
            private UserManager<ApplicationUser> _userManager;

            public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
            {
                _context = context;
                _userManager = userManager;
            }

            // GET: Orders
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> Index()
            {
                return View(await _context.Orders.Include(i => i.User).AsNoTracking().ToListAsync());
            }

            // GET: Orders/Create
            [Authorize(Roles = "Member")]
            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            [Authorize(Roles = "Member")]
            public async Task<IActionResult> Create([Bind("City,State,FirstName,LastName,Phone,PostalCode,Address")] Order order)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);

                if (true)
                {
                    ShoppingCart cart = ShoppingCart.GetCart(this.HttpContext);
                    List<CartItem> items = cart.GetCartItems(_context);
                    List<OrderItem> details = new List<OrderItem>();
                    foreach (CartItem item in items)
                    {
                        OrderItem detail = CreateOrderItemForThisItem(item);
                        detail.Order = order;
                        details.Add(detail);
                        _context.Add(detail);
                    }

                    order.User = user;
                    order.OrderDate = DateTime.Today.ToString("dd-MM-yyyy");
                    order.Status = Status.Current;
                    order.Subtotal = ShoppingCart.GetCart(this.HttpContext).GetTotal(_context);
                    order.GST = ShoppingCart.GetCart(this.HttpContext).GetTotal(_context) * 0.15;
                    order.GrandTotal = ShoppingCart.GetCart(this.HttpContext).GetTotal(_context) * 1.15;
                    order.OrderItems = details;
                    _context.SaveChanges();

                    return RedirectToAction("Purchased", new RouteValueDictionary(
                    new { action = "Purchased", id = order.OrderID }));
                }

                return View(order);
            }

            private OrderItem CreateOrderItemForThisItem(CartItem item)
            {

                OrderItem detail = new OrderItem();

                detail.Quantity = item.Count;
                detail.Cap = item.Cap;
                detail.Cap.Price = item.Cap.Price;

                return detail;

            }
            public async Task<IActionResult> Purchased(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var order = await _context.Orders.Include(i => i.User).AsNoTracking().SingleOrDefaultAsync(m => m.OrderID == id);
                if (order == null)
                {
                    return NotFound();
                }

                var details = _context.OrderItems.Where(detail => detail.Order.OrderID == order.OrderID).Include(detail => detail.Cap).ToList();

                order.OrderItems = details;
                ShoppingCart.GetCart(this.HttpContext).EmptyCart(_context);
                return View(order);
            }

            public async Task<IActionResult> Details(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }
                var order = await _context.Orders.Include(i => i.User).AsNoTracking().SingleOrDefaultAsync(m => m.OrderID == id);
                if (order == null)
                {
                    return NotFound();
                }

                var details = _context.OrderItems.Where(detail => detail.Order.OrderID == order.OrderID).Include(detail => detail.Cap).ToList();

                order.OrderItems = details;

                return View(order);
            }


            public async Task<IActionResult> Ship(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var order = await _context.Orders.SingleOrDefaultAsync(m => m.OrderID == id);

                if (order == null)
                {
                    return NotFound();
                }

                //if (!order.Status.Equals("shipped"))
                //{

                //}
                order.Status = Status.Shipped;
                _context.Update(order);
                await _context.SaveChangesAsync();


                return RedirectToAction("Index");
            }



            // GET: Orders/Delete/5
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var order = await _context.Orders.Include(i => i.User).AsNoTracking().SingleOrDefaultAsync(m => m.OrderID == id);
                if (order == null)
                {
                    return NotFound();
                }

                var details = _context.OrderItems.Where(detail => detail.Order.OrderID == order.OrderID).Include(detail => detail.Cap).ToList();

                order.OrderItems = details;



                return View(order);
            }

            // POST: Orders/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var order = await _context.Orders.SingleOrDefaultAsync(m => m.OrderID == id);
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }
}
