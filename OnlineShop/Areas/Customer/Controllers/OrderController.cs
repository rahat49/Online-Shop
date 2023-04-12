using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Utility;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        //Get checkout action Method
        public IActionResult Checkout()
        {
           
            return View();
        }


        //Post checkout action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order anOrder)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                foreach (var product in products)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.ProductId = product.Id;
                    //anOrder.OrderDetails = new List<OrderDetails>();
                    anOrder.OrderDetails.Add(orderDetails);
                }
            }
            anOrder.OrderNo = GetOrderNo();
            _context.Orders.Add(anOrder);
            await _context.SaveChangesAsync();
            HttpContext.Session.Set("products", new List<Products>());
            return View("Checkout");
        }


        public string GetOrderNo()
        {
            int rowCount =_context.Orders.ToList().Count+1;
            return rowCount.ToString("000");
        }
    }
}
