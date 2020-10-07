using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnackPros.DataAccess.Data.Repository.IRepository;
using SnackPros.Models;
using SnackPros.Utility;

namespace SnackPros.Pages.Customer.Home
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DetailsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public ShoppingCart ShopingCartObj { get; set; }
        public void OnGet(int id)
        {
            ShopingCartObj = new ShoppingCart()
            {
                MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(includeProperties: "Category,SnackType", filter: c => c.Id == id),
                MenuItemId = id
            };
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                // Retrieve the user id of the logged in user 
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                ShopingCartObj.ApplicationUserId = claim.Value;
                //access the database for shopping cart 

                ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.ApplicationUserId == ShopingCartObj.ApplicationUserId &&
                                          c.MenuItemId == ShopingCartObj.MenuItemId);
                
                if (cartFromDb == null) // add new item to cart 
                {
                    _unitOfWork.ShoppingCart.Add(ShopingCartObj);
                    
                }
                else // if object exists increment count of object 
                {
                    
                    _unitOfWork.ShoppingCart.IncrementCount(cartFromDb, ShopingCartObj.Count);
                }
                _unitOfWork.Save();
                //increase the session 
                var count = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == ShopingCartObj.ApplicationUserId).ToList().Count;
                HttpContext.Session.SetInt32(SD.ShoppingCart, count);
                return RedirectToPage("Index");
            }
            else
            {
               ShopingCartObj.MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(includeProperties: "Category,SnackType", filter: c => c.Id == ShopingCartObj.MenuItemId);
               return Page();
            }
        }
    }
}
