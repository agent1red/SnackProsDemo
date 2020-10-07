using Microsoft.AspNetCore.Mvc.Rendering;
using SnackPros.DataAccess.Data.Repository.IRepository;
using SnackPros.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnackPros.DataAccess.Data.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _db;

        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        //Decrement Counter method 
        public int DecrementCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count -= count;
            return shoppingCart.Count;
        }

        //Increment Counter method
        public int IncrementCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count += count;
            return shoppingCart.Count;
        }





    }
}
