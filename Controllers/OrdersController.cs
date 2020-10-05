using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    public class OrdersController : Controller
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;

        public OrdersController(IDutchRepository repository, ILogger<OrdersController> Logger, IMapper mapper)
        {
            _repository = repository;
            _logger = Logger;
            _mapper = mapper;
        }

        public IActionResult GetAllOrders(bool IncludeItems=true)
        {
            try
            {
                var result = _repository.GetAllOrders(IncludeItems);
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
                var order = (_repository.GetById(id));
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
        public IActionResult PostOrder([FromBody]OrderViewModel model)
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
