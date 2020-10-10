using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnackPros.DataAccess.Data.Repository.IRepository;
using SnackPros.Models;
using SnackPros.Models.ViewModels;
using SnackPros.Utility;

namespace SnackPros.Pages.Admin.Order
{
    public class ManageOrderModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManageOrderModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public List<OrderDetailsVM> orderDetailsVM { get; set; };
       
        
        public void OnGet()
        {
            orderDetailsVM = new List<OrderDetailsVM>();

            List<OrderHeader> OrderHeaderList = _unitOfWork.OrderHeader.GetAll(o => o.Status == SD.StatusSubmitted
                                                                                 || o.Status == SD.StatusInProcess)
                                                                                     .OrderByDescending(u => u.PickUpTime)
                                                                                     .ToList();

                     foreach (OrderHeader item in OrderHeaderList)
            {
                OrderDetailsVM individual = new OrderDetailsVM
                {
                    // assign order header 
                    OrderHeader = item,
                    // retrieve from DataBase
                    OrderDetails = _unitOfWork.OrderDetails.GetAll(o => o.OrderId == item.Id).ToList()
                };
                // Add to orderListVM
                orderDetailsVM.Add(individual);
            }

        }
    }
}
