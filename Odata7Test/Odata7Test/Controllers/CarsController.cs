using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Odata7Test.Models;

namespace Odata7Test.Controllers
{

    public class CarsController : ODataController
    {
        private static IEnumerable<Car> products = new []
        {
            new Car()
            {
              
                Id = 1,
                Name = "Mazda",
            },
            new Car
            {
                Id = 2,
                Name = "BMW"
            }
        };
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            return Ok( products.AsQueryable());
        }

        public async Task<IActionResult> Get(int key)
        {
            return Ok(products.FirstOrDefault(f => f.Id == key));
        }

        public async Task<IActionResult> Patch(int key, [FromBody] Delta<Car> delta) //FromBOdy seems to be required otherwise it stays null
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(s => s.Value.Errors.Select(x => x.Exception.Message)));
            }
            var origCar = products.FirstOrDefault(f => f.Id == key);
            if (origCar == null)
            {
                return NotFound();
            }
            delta.Patch(origCar);
            TryValidateModel(origCar);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(s => s.Value.Errors.Select(x => x.Exception.Message)));
            }
            return Updated(origCar);
        }
        public async Task<IActionResult> Delete(int key)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(s => s.Value.Errors.Select(x => x.Exception.Message)));
            }
            var origCar = products.FirstOrDefault(f => f.Id == key);
            if (origCar == null)
            {
                return NotFound();
            }
            products = products.Where(w => w.Id != key);
            return StatusCode((int)HttpStatusCode.NoContent);
        }

        public async Task<IActionResult> Post([FromBody] Car newCar)//FromBOdy seems to be required otherwise it stays null
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(s => s.Value.Errors.Select(x => x.Exception.Message)));
            }

            var newId = products.Max(i => i.Id) + 1;
            newCar.Id = newId;
            products = products.Concat(new[] { newCar });
            return Created(newCar);
        }
    }
}