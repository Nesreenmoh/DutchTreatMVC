using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : Controller
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<StoreUser> _userManager;

        public OrdersController(IDutchRepository repository, 
            ILogger<OrdersController> Logger, IMapper mapper, UserManager<StoreUser> userManager)
        {
            _repository = repository;
            _logger = Logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        public IActionResult GetAllOrders(bool IncludeItems=true)
        {
            try
            {
                var user = User.Identity.Name;
                var result = _repository.GetAllOrdersByUser(user,IncludeItems);
                return Ok(_mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(result));
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get orders {ex}");
                return BadRequest("Failed to get orders");
            }
           
        }
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var order = (_repository.GetById(User.Identity.Name, id));
                if (order != null)
                    return Ok(_mapper.Map<Order,OrderViewModel>(order));
                else return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get orders {ex}");
                return BadRequest("Failed to get orders");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody]OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = _mapper.Map<OrderViewModel, Order>(model); 
                   
                    if (newOrder.OrderDate == DateTime.MinValue)
                    {
                        newOrder.OrderDate = DateTime.Now;
                    }

                    var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                    newOrder.User = currentUser;
                    _repository.AddEntity(newOrder);

                    if (_repository.SaveChanges())
                    {
                       
                        return Created($"/api/Orders/{newOrder.Id}", _mapper.Map<Order,OrderViewModel>(newOrder));
                    }
                }else
                {
                    return BadRequest(ModelState);
                }
               
               
            }catch(Exception ex)
            {
                _logger.LogInformation($"Could not post the order{ex}!");
                return BadRequest("Could not post the order");
            }
            return BadRequest("Failed to save order");
        }
    }
}
