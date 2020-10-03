using DutchTreat.Data;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        private readonly INullMailService _nullMailService;
        private readonly IDutchRepository _repository;
       

        public AppController(INullMailService nullMailService, IDutchRepository repository)
        {
            this._nullMailService = nullMailService;
            this._repository = repository;
           
        }

        public IActionResult Index()
        {
            var results = _repository.GetAllProducts();
            return View(results);
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {
            ViewBag.Title = "Contact";
            return View();
        }


        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                _nullMailService.SendMail(model.Name, model.Subject, model.Message);
                ViewBag.Message = "Message has been sent!";
                ModelState.Clear();
            }
            else
            {

            }

            return View();

        }

        [HttpGet("about")]
        public IActionResult About()
        {
            ViewBag.Title = "About";
            return View();
        }

        public IActionResult Shop()
        {
            var result = _repository.GetAllProducts()
                .OrderBy(p=>p.Category)
                .ToList();
            return View(result);
        }
    }
}
