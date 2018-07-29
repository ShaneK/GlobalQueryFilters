using System.ComponentModel.DataAnnotations;
using System.Linq;
using GlobalQueryFilters.Data;
using GlobalQueryFilters.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlobalQueryFilters.Controllers
{
    [Route("api/[controller]")]
    public class PizzaController : Controller
    {
        private readonly Context _context;

        public PizzaController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ListPizzas()
        {
            return Ok(new
            {
                Success = true,
                Pizzas = _context.Pizzas.ToList()
            });
        }

        [HttpPost]
        public IActionResult InsertPizza([FromBody] PizzaDto newPizza)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ModelState.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage
                });
            }

            var Pizza = new Pizza
            {
                Name = newPizza.Name
            };

            _context.Pizzas.Add(Pizza);
            _context.SaveChanges();
            return Ok(new
            {
                Success = true,
                Pizzas = _context.Pizzas.ToList()
            });
        }

        [HttpPut, Route("{id}")]
        public IActionResult UpdatePizza(long id, [FromBody] PizzaDto updatedPizza)
        {
            var updatingPizza = _context.Pizzas.IgnoreQueryFilters()
                .FirstOrDefault(x => x.Id == id);

            if (updatingPizza == null)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = $"Couldn't find a Pizza with the ID {id}!"
                });
            }

            updatingPizza.Name = updatedPizza.Name;
            updatingPizza.Active = updatedPizza.Active;
            _context.SaveChanges();

            return Ok(new
            {
                Success = true,
                Pizzas = _context.Pizzas.ToList()
            });
        }

        [HttpDelete, Route("{id}")]
        public IActionResult DeletePizza(long id)
        {
            var deletedPizza = _context.Pizzas.FirstOrDefault(x => x.Id == id);

            if (deletedPizza == null)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = $"Couldn't find a Pizza with the ID {id}!"
                });
            }

            deletedPizza.Active = false;
            _context.SaveChanges();

            return Ok(new
            {
                Success = true,
                Pizzas = _context.Pizzas.ToList()
            });
        }
    }

    public class PizzaDto
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}