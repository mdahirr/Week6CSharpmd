﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindAPI.Models;
using NorthwindAPI.Models.DTO;

namespace NorthwindAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public SuppliersController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: api/Suppliers
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers()
        //{
        //    return await _context.Suppliers.ToListAsync();
        //}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierDTO>>> GetSuppliers()
        {
            var suppliers = await _context.Suppliers
                .Include(s => s.Products)
                .Select(x => Utils.SupplierToDTO(x))
                .ToListAsync();
            return suppliers;
        }

        // GET: api/Suppliers/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Supplier>> GetSupplier(int id)
        //{
        //    var supplier = await _context.Suppliers.FindAsync(id);

        //    if (supplier == null)
        //    {
        //        return NotFound();
        //    }

        //    return supplier;
        //}


        public async Task<ActionResult<SupplierDTO>> GetSupplier(int id)
        {
        if (!SupplierExists(id))
        {
            return NotFound();
        }
        return Utils.SupplierToDTO(await _context.Suppliers
            .Where(s => s.SupplierId == id)
            .Include(s => s.Products)
            .FirstAsync());
        }


        [HttpGet("{id}/products/{productsId}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetSupplierWithProducts(int id)
        {
        if (!SupplierExists(id))
        {
            return NotFound();
        }
        return await _context.Products
            .Where(p => p.SupplierId == id)
            .Select(p => Utils.ProductToDTO(p))
            .ToListAsync();

        }

        // PUT: api/Suppliers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutSupplier(int id, Supplier supplier)
        //{
        //    if (id != supplier.SupplierId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(supplier).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!SupplierExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Suppliers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Supplier>> PostSupplier(Supplier supplier)
        //{
        //    _context.Suppliers.Add(supplier);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetSupplier", new { id = supplier.SupplierId }, supplier);
        //}

        [HttpPost]
        public async Task<ActionResult<SupplierDTO>> PostSupplier(SupplierDTO supplierDto)
        {
            List<Product> products = new List<Product>();



            supplierDto.Products
                .ToList()
                .ForEach(x => products.Add(new Product()
                {
                    ProductName = x.ProductName,
                    UnitPrice = x.UnitPrice
                }));
            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();



            Supplier supplier = new Supplier
            {
                SupplierId = supplierDto.SupplierId,
                CompanyName = supplierDto.CompanyName,
                ContactName = supplierDto.ContactName,
                ContactTitle = supplierDto.ContactTitle,
                Country = supplierDto.Country,
                Products = products
            };
            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();



            // Update IDs DTO
            supplierDto = await _context.Suppliers
                .Where(s => s.SupplierId == supplier.SupplierId)
                .Include(x => x.Products)
                .Select(x => Utils.SupplierToDTO(x))
                .FirstAsync();



            return CreatedAtAction(
                nameof(GetSupplier),
                new { id = supplier.SupplierId },
                supplierDto);
        }

        // DELETE: api/Suppliers/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteSupplier(int id)
        //{
        //    var supplier = await _context.Suppliers.FindAsync(id);
        //    if (supplier == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Suppliers.Remove(supplier);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        [HttpDelete("{id}")]
        public async Task<ActionResult<SupplierDTO>> DeleteSupplier(int id)
        {
            if (!SupplierExisits(id))
            {
                return NotFound();
            }
            var supplier = Utils.SupplierToDTO(await _context.Suppliers
                .Where(s => s.SupplierId = id)
                .Include(s => s.Products)
                .FindAsync(id));

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
        }

        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(e => e.SupplierId == id);
        }
    }
}
