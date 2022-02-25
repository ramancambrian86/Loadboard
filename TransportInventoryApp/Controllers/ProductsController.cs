using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransportInventoryApp.Data;
using TransportInventoryApp.Models;

namespace TransportInventoryApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly TransportInventoryContext _context;

        public ProductsController(TransportInventoryContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Index()
        {
            if(HttpContext.Session.Get("username") == null)
            {
                return RedirectToAction("Index", "Account");
            }

            var result = await _context.Products.ToListAsync();
            return View(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var result = await _context.Products.ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var result = await _context.Products.FindAsync(id);
            return Ok(result);
        }

        public async Task<IActionResult> AddOrEdit(int? Id)
        {
            ViewBag.PageName = Id == null ? "Create Product" : "Edit Product";
            ViewBag.IsEdit = Id == null ? false : true;
            if (Id == null)
            {
                return View();
            }
            else
            {
                var product = await _context.Products.FindAsync(Id);

                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int Id, [Bind("Id,Name,Quantity,Price")] Product data)
        {
            bool IsProductExist = false;

            Product product = await _context.Products.FindAsync(Id);

            if (product != null)
            {
                IsProductExist = true;
            }
            else
            {
                product = new Product();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product.Name = data.Name;
                    product.Quantity = data.Quantity;
                    product.Price = data.Price;

                    if (IsProductExist)
                    {
                        _context.Update(product);
                    }
                    else
                    {
                        _context.Add(product);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(data);
        }

        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(Id);

            return View(product);
        }

        // POST: Products/Delete/1
        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            var product = await _context.Products.FindAsync(Id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
