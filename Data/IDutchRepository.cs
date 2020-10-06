using DutchTreat.Data.Entities;
using System;
using System.Collections.Generic;

namespace DutchTreat.Data
{
    public interface IDutchRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetAllProductsByCategory(string category);
        bool SaveChanges();
        IEnumerable<Order> GetAllOrders(bool IncludeItems);
        IEnumerable<Order> GetAllOrdersByUser(string user, bool includeItems);
        Order GetById(string username, int id);
        void AddEntity(Object model);
       
    }
}