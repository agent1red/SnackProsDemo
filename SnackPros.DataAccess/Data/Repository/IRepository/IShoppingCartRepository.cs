using Microsoft.AspNetCore.Mvc.Rendering;
using SnackPros.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnackPros.DataAccess.Data.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        //Define variables for Incrementing and Decromenting Counter 
        int IncrementCount(ShoppingCart shoppingCart, int count);
        int DecrementCount(ShoppingCart shoppingCart, int count);
    }
}
