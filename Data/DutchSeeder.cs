using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _ctx;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<StoreUser> _userManager;

        public DutchSeeder(DutchContext ctx, IWebHostEnvironment hostingEnvironment, UserManager<StoreUser> userManager)
        {
            _ctx = ctx;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _ctx.Database.EnsureCreated();

            StoreUser User = await _userManager.FindByEmailAsync("nesreenmhd@gmail.com");
            if (User == null)
            {
                User = new StoreUser()
                {
                    FirstName="Nesreen",
                    LastName="Al-shargabi",
                     Email ="nesreenmhd@gmail.com",
                     UserName= "nesreenmhd@gmail.com"
                };
                var result = await _userManager.CreateAsync(User, "System123456@");
                if(result!= IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in seeder");
                }
            }
            if (!_ctx.Products.Any())
            {
                var FilePath = Path.Combine(_hostingEnvironment.ContentRootPath,"Data/art.json");
                var json = File.ReadAllText(FilePath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                _ctx.Products.AddRange(products);

                var order = _ctx.Orders.Where(p => p.Id == 1).FirstOrDefault();
                if (order != null)
                {
                    order.User = User;
                    order.Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product=products.First(),
                            Quantity=5,
                            UnitPrice=products.First().Price
                        }
                    };
                }
                _ctx.SaveChanges();
            }
        }
    }
}
