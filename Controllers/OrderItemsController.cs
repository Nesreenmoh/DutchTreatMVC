using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace DutchTreat.Controllers
{
    [Route("/api/Orders/{Orderid}/items")]
    public class OrderItemsController : Controller
    {
        private readonly IDutchRepository _repository;
       // private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public OrderItemsController(IDutchRepository repository, IMapper mapper)
        {
            _repository = repository;
           // _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllItems(int Orderid)
        {
            var order = _repository.GetById(Orderid);
            if (order != null)
            {
                return Ok(_mapper.Map<IEnumerable<OrderItem>,
                    IEnumerable<OrderItemViewModel>>(order.Items));
            }
            return NotFound();
        }


        // get item details by id 
        [HttpGet("{id}")]
        public IActionResult GetItemById(int Orderid,  int id )
        {
            var order = _repository.GetById(Orderid);
            if (order != null)
            {
                var item = order.Items.Where(i => i.Id == id).FirstOrDefault();
                if(item!=null)
                {
                    return Ok(_mapper.Map<OrderItem,
                    OrderItemViewModel>(item));
                }
                
            }
            return NotFound();
        }

    }
}
