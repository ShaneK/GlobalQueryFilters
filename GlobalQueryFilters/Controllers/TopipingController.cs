using System.ComponentModel.DataAnnotations;
using System.Linq;
using GlobalQueryFilters.Data;
using GlobalQueryFilters.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlobalQueryFilters.Controllers
{
    [Route("api/[controller]")]
    public class ToppingController : Controller
    {
        private readonly Context _context;

        public ToppingController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ListToppings()
        {
            return Ok(new
            {
                Success = true,
                Toppings = _context.Toppings.ToList()
            });
        }

        [HttpPost]
        public IActionResult InsertTopping([FromBody] ToppingDto newTopping)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ModelState.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage
                });
            }

            var topping = new Topping
            {
                Name = newTopping.Name
            };

            _context.Toppings.Add(topping);
            _context.SaveChanges();
            return Ok(new
            {
                Success = true,
                Toppings = _context.Toppings.ToList()
            });
        }

        [HttpPut, Route("{id}")]
        public IActionResult UpdateTopping(long id, [FromBody] ToppingDto updatedTopping)
        {
            var updatingTopping = _context.Toppings.IgnoreQueryFilters()
                .FirstOrDefault(x => x.Id == id);

            if (updatingTopping == null)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = $"Couldn't find a topping with the ID {id}!"
                });
            }

            updatingTopping.Name = updatedTopping.Name;
            updatingTopping.Active = updatedTopping.Active;
            _context.SaveChanges();

            return Ok(new
            {
                Success = true,
                Toppings = _context.Toppings.ToList()
            });
        }

        [HttpDelete, Route("{id}")]
        public IActionResult DeleteTopping(long id)
        {
            var deletedTopping = _context.Toppings.FirstOrDefault(x => x.Id == id);

            if (deletedTopping == null)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = $"Couldn't find a topping with the ID {id}!"
                });
            }

            deletedTopping.Active = false;
            _context.SaveChanges();

            return Ok(new
            {
                Success = true,
                Toppings = _context.Toppings.ToList()
            });
        }
    }

    public class ToppingDto
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}