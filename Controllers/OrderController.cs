using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using VegEaseBackend.Models;

namespace VegEaseBackend.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly ProductDBContext _context;

        public OrderController(ProductDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            
            return View("~/Views/Profile_items/Orders.cshtml");
        }

        [HttpPost("ProceedToPay")]
        public async Task<IActionResult> ProceedToPay([FromBody] ProceedToPayRequest request)
        {
            try
            {
                List<string> itemsList = request.ProductIds
            .Split(',')
            .Select(item => item.Trim()) // Trim whitespace from each item
            .ToList();

                //order details updated
                OrderDetails objOrder = new OrderDetails();
                objOrder.UserId = 1;
                objOrder.OrderDate = DateTime.Now;
                _context.OrderDetails.Add(objOrder);
                _context.SaveChanges(true);

                // Output the list to verify
                foreach (var item in itemsList)
                {
                    //carts updated
                    var cartItem = await _context.Carts.FindAsync(Convert.ToInt32(item));
                     cartItem.IsOrder = true;
                    _context.Carts.Update(cartItem);
                    _context.SaveChanges();

                    //order product mapping
                    OrderProductMapping objOP= new OrderProductMapping();
                    objOP.CartId = cartItem.Id;
                    objOP.OrderId = objOrder.OrderId;
                    _context.OrderProductMapping.Add(objOP);
                    _context.SaveChanges();

                }
                return Ok(new { message = "Products purchased successfully." });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception occurred while updating quantity: " + ex.Message);
                return StatusCode(500, "Internal server error.");
            }
        }


        [Route("getOrderDetais")]
        [HttpGet]
        public async Task<IActionResult> getOrderDetais([FromQuery] int OrderId)
        {
            try
            {
                var orderDetails = await _context.OrderDetails
              .Where(od => od.OrderId == OrderId)
              .Join(_context.OrderProductMapping,
                  od => od.OrderId,
                  opm => opm.OrderId,
                  (od, opm) => new { od, opm })
              .Join(_context.Carts,
                  op => op.opm.CartId,
                  c => c.Id,
                  (op, c) => new
                  {

                      OrderId = op.od.OrderId,
                      ProductId = c.ProductId,
                      OrderTime = op.od.OrderDate,
                      ProductName = c.Product.Name,
                      Price = c.Product.Price,
                      Image = c.Product.Image,
                      Quantity = c.Quantity
                  })
              .ToListAsync();

                return Ok(orderDetails);
                /* return View("~/Views/Profile_items/Orders.cshtml");*/
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception occurred while updating quantity: " + ex.Message);
                return StatusCode(500, "Internal server error.");
            }
        }

        [Route("getOrderIds")]
        [HttpGet]
        public async Task<IActionResult> getOrderIds()
        {
            try
            {
                var orderIds = await _context.OrderDetails
                    .Select(od => new
                    {
                        od.OrderId,
                        OrderTime = od.OrderDate
                    })
                    .Distinct() // Ensure unique OrderIds
                    .ToListAsync();

                return Ok(orderIds);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception occurred while fetching order IDs: " + ex.Message);
                return StatusCode(500, "Internal server error.");
            }
        }

    }
}

/*
[Route("getOrderDetais")]
        [HttpGet]
        public async Task<IActionResult> getOrderDetais([FromQuery] int OrderId)
        {
            try
            {
                var orderDetails = await _context.OrderDetails
              .Where(od => od.OrderId == OrderId)
              .Join(_context.OrderProductMapping,
                  od => od.OrderId,
                  opm => opm.OrderId,
                  (od, opm) => new { od, opm })
              .Join(_context.Carts,
                  op => op.opm.CartId,
                  c => c.Id,
                  (op, c) => new
                  {
                      OrderId = op.od.OrderId,
                      ProductId = c.ProductId,
                      ProductName = c.Product.Name,
                      Price = c.Product.Price,
                      Image = c.Product.Image,
                      Quantity = c.Quantity
                  })
              .ToListAsync();

                return Ok(orderDetails);
                *//* return View("~/Views/Profile_items/Orders.cshtml");*//*
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception occurred while updating quantity: " + ex.Message);
                return StatusCode(500, "Internal server error.");
            }
        }*/