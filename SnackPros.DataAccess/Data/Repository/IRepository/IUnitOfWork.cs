﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SnackPros.DataAccess.Data.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        // All the work in a single transaction or batch with a save function to push to database
        ICategoryRepository Category { get; }
        ISnackTypeRepository SnackType { get; }
        IMenuItemRepository MenuItem { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailsRepository OrderDetails { get; }

        void Save();
    }
}
