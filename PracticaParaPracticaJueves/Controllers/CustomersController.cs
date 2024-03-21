using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PracticaParaPracticaJueves.Models;

namespace PracticaParaPracticaJueves.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return _context.Customers != null ?
                        View(await _context.Customers.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Customers'  is null.");
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
               .Include(s => s.Phones)
               .FirstAsync(s => s.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Details";
            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            var customer = new Customer();
            customer.Phones = new List<Phone>();
            customer.Phones.Add(new Phone
            {
                PhoneNumber = "",
                Description = ""
            });
            ViewBag.Accion = "Create";
            return View(customer);
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,Address,Phones")] Customer customer, IFormFile imagen)
        {
            int Mb_1 = 1048576;
            if (imagen != null && imagen.Length < Mb_1) // Guardar Si Tienen Menos de 1 Mb
            {
                if (imagen != null && imagen.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagen.CopyToAsync(memoryStream);
                        customer.Imagen = memoryStream.ToArray();
                    }
                }

            }
            else
            {
                ModelState.AddModelError("imagen", "La imagen debe ser menor a 1 Mb");
            }
            _context.Add(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #region DETALLES
        [HttpPost]
        public ActionResult AgregarDetalles([Bind("Id,FirstName,LastName,Email,Address,Phones")] Customer customer, IFormFile imagen, string accion)
        {
            customer.Phones.Add(new Phone
            {
                PhoneNumber = "",
                Description = ""
            });
            ViewBag.Accion = accion;
            return View(accion, customer);
        }

        public ActionResult EliminarDetalles([Bind("Id,FirstName,LastName,Email,Address,Phones")] Customer customer, IFormFile imagen, string accion, int index)
        {
            var det = customer.Phones[index];
            if (accion == "Edit" && det.Id > 0)
            {
                det.Id = det.Id * -1;
            }
            else
            {
                customer.Phones.RemoveAt(index);
            }

            ViewBag.Accion = accion;
            return View(accion, customer);
        }
        #endregion


        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers
              .Include(s => s.Phones)
              .FirstAsync(s => s.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Edit";
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,Address,Phones")] Customer customer, IFormFile imagen)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }
            try
            {
                if (imagen != null && imagen.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagen.CopyToAsync(memoryStream);
                        customer.Imagen = memoryStream.ToArray();
                    }
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Obtener los datos de la base de datos que van a ser modificados
                    var customerUpdate = await _context.Customers
                            .Include(s => s.Phones)
                            .FirstAsync(s => s.Id == customer.Id);
                    if (customerUpdate?.Imagen?.Length > 0)
                        customer.Imagen = customerUpdate.Imagen;
                    customerUpdate.FirstName = customer.FirstName;
                    customerUpdate.LastName = customer.LastName;
                    customerUpdate.Email = customer.Email;
                    customerUpdate.Address = customer.Address;

                    // Obtener todos los detalles que seran nuevos y agregarlos a la base de datos
                    var detNew = customer.Phones.Where(s => s.Id == 0);
                    foreach (var d in detNew)
                    {
                        customerUpdate.Phones.Add(d);
                    }
                    // Obtener todos los detalles que seran modificados y actualizar a la base de datos
                    var detUpdate = customer.Phones.Where(s => s.Id > 0);
                    foreach (var d in detUpdate)
                    {
                        var det = customerUpdate.Phones.FirstOrDefault(s => s.Id == d.Id);
                        det.PhoneNumber = d.PhoneNumber;
                        det.Description = d.Description;
                    }
                    // Obtener todos los detalles que seran eliminados y actualizar a la base de datos
                    var delDetIds = customer.Phones.Where(s => s.Id < 0).Select(s => -s.Id).ToList();
                    if (delDetIds != null && delDetIds.Count > 0)
                    {
                        foreach (var detalleId in delDetIds) // Cambiado de 'id' a 'detalleId'
                        {
                            var det = await _context.Phones.FindAsync(detalleId); // Cambiado de 'id' a 'detalleId'
                            if (det != null)
                            {
                                _context.Phones.Remove(det);
                            }
                        }
                    }
                    // Aplicar esos cambios a la base de datos
                    _context.Update(customerUpdate);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(customer.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
              .Include(s => s.Phones)
              .FirstAsync(s => s.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Delete";
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
