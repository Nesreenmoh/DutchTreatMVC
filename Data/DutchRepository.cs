using DutchTreat.Data.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _ctx;
        private readonly ILogger<DutchRepository> _logger;

        public DutchRepository(DutchContext dutchContext, ILogger<DutchRepository> logger)
        {
            _ctx = dutchContext;
            _logger = logger;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("We are getting the products");
                return _ctx.Products
                    .OrderBy(p => p.Title)
                    .ToList();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Fail to get all products{ex}");
                return null;
            }
           
        }

        public IEnumerable<Product> GetAllProductsByCategory(string category)
        {
            return _ctx.Products
                .Where(p => p.Category.Contains(category))
                .ToList();

        }

        public bool SaveChanges()
        {
            return _ctx.SaveChanges()>0; 
        }
    }
}
