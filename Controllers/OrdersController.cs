using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    public class OrdersController : Controller
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IDutchRepository repository, ILogger<OrdersController> Logger)
        {
            this._repository = repository;
            _logger = Logger;
        }

        public IActionResult GetAllOrders()
        {
            try
            {
                return Ok(_repository.GetAllOrders());
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
                    return Ok(order);
                else return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get orders {ex}");
                return BadRequest("Failed to get orders");
            }
        }
    }
}
