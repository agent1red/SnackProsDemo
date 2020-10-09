using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnackPros.DataAccess.Data.Repository.IRepository;
using SnackPros.Models;
using SnackPros.Utility;

namespace SnackPros.Pages.Customer.Cart
{
    public class SummaryModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public SummaryModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // view model to add 
        public OrderDetailsCart detailCart { get; set; }
        public IActionResult OnGet()
        {
            detailCart = new OrderDetailsCart()
            {
                OrderHeader = new Models.OrderHeader()
            };

            //Initialize OrderTotal to 0;
            detailCart.OrderHeader.OrderTotal = 0;

            //Retrieve shopping cart from db for the logged in user using claims 

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //retrieve shopping cart
            IEnumerable<ShoppingCart> cart = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == claim.Value);

            if (cart != null)
            {
                detailCart.listCart = cart.ToList();
            }

            foreach (var cartList in detailCart.listCart)
            {
                cartList.MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(m => m.Id == cartList.MenuItemId);

                //order total 

                detailCart.OrderHeader.OrderTotal += (cartList.MenuItem.Price * cartList.Count);
            }

            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(c => c.Id == claim.Value);
            detailCart.OrderHeader.PickupName = applicationUser.FullName;
            detailCart.OrderHeader.PickUpTime = DateTime.Now;
            detailCart.OrderHeader.PhoneNumber = applicationUser.PhoneNumber;
            return Page();
        }

        // stripe payment handler 
        public IActionResult OnPost(string stripeToken)
        {
            //create OrderHeader and Order Details and add inside the database
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            // Retrieve all teh items from the shopping cart 
            detailCart.listCart = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == claim.Value).ToList();

            //Update OrderHeader and change the PaymentStatus for the order status and payment and add Date/Time
            detailCart.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            detailCart.OrderHeader.OrderDate = DateTime.Now;
            detailCart.OrderHeader.UserId = claim.Value;
            detailCart.OrderHeader.Status = SD.PaymentStatusPending;
            detailCart.OrderHeader.PickUpTime = Convert.ToDateTime(detailCart
                                                                .OrderHeader
                                                                .PickUpDate
                                                                .ToShortDateString() + " " + detailCart
                                                                .OrderHeader
                                                                .PickUpTime
                                                                .ToShortTimeString()
                                                                );

            List<OrderDetails> orderDetailslist = new List<OrderDetails>();
            _unitOfWork.OrderHeader.Add(detailCart.OrderHeader);
            _unitOfWork.Save();

            //Loop through OrderDetails
            foreach (var item in detailCart.listCart)
            {
                item.MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(m => m.Id == item.MenuItemId);
                OrderDetails orderDetails = new OrderDetails
                {
                    MenuItemID = item.MenuItemId,
                    OrderId = detailCart.OrderHeader.Id,
                    Description = item.MenuItem.Description,
                    Name = item.MenuItem.Name,
                    Price = item.MenuItem.Price,
                    Count = item.Count
                };
                // Modify the Order Total 

                detailCart.OrderHeader.OrderTotal += (orderDetails.Count * orderDetails.Price);
                // add to orderDetails database 
                _unitOfWork.OrderDetails.Add(orderDetails);
            }
            // remove from cart from database using RemoveRange ref (Irepository, Repository)
            _unitOfWork.ShoppingCart.RemoveRange(detailCart.listCart);

            // End Session
            HttpContext.Session.SetInt32(SD.ShoppingCart, 0);
            _unitOfWork.Save();

            // Charge Stripe using creditcard (Stripe Logic)
        }

    }
}
