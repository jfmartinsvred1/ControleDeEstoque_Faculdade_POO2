using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControleDeEstoque.Data;
using ControleDeEstoque.Models;

namespace ControleDeEstoque.Controllers
{
    public class StocksController : Controller
    {
        private readonly ControleDeEstoqueContext _context;

        public StocksController(ControleDeEstoqueContext context)
        {
            _context = context;
        }

        // GET: Stocks
        public async Task<IActionResult> Index()
        {
            var stocks = await _context.Stocks.Include(s => s.Product).ToListAsync();

            var totalBySuppliers = await _context.Orders
                .Include(o => o.Stock)
                    .ThenInclude(s => s.Product)
                .Include(o => o.Supplier)
                .GroupBy(o => new { ProductName = o.Stock.Product.Name, SupplierName = o.Supplier.Name })
                .Select(g => new TotalBySupplier
                {
                    ProductName = g.Key.ProductName,
                    SupplierName = g.Key.SupplierName,
                    Amount = g.Sum(x => x.Amount)
                })
                .ToListAsync();

            ViewBag.TotalBySuppliers = totalBySuppliers;

            return View(stocks);
        }


        // GET: Stocks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks
                .Include(s => s.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stock == null)
            {
                return NotFound();
            }

            var totalBySuppliers = await _context.Orders
               .Include(o => o.Stock)
               .ThenInclude(s => s.Product)
               .Include(o => o.Supplier)
               .Where(s=>s.Stock.Product.Id==stock.Product.Id)
               .GroupBy(o => new { ProductName = o.Stock.Product.Name, SupplierName = o.Supplier.Name })
               .Select(g => new TotalBySupplier
               {
                   ProductName = g.Key.ProductName,
                   SupplierName = g.Key.SupplierName,
                   Amount = g.Sum(x => x.Amount)
               })
               .ToListAsync();

            ViewBag.TotalBySuppliersDetails = totalBySuppliers;

            return View(stock);
        }

        // GET: Stocks/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        // POST: Stocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Amount,ProductId,Id,CreatedAt,UpdatedAt")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                stock.Amount = 0;
                _context.Add(stock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", stock.ProductId);
            return View(stock);
        }

        // GET: Stocks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", stock.ProductId);
            return View(stock);
        }

        // POST: Stocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Amount,ProductId,Id,CreatedAt,UpdatedAt")] Stock stock)
        {
            if (id != stock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockExists(stock.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", stock.ProductId);
            return View(stock);
        }

        // GET: Stocks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks
                .Include(s => s.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // POST: Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock != null)
            {
                _context.Stocks.Remove(stock);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockExists(Guid id)
        {
            return _context.Stocks.Any(e => e.Id == id);
        }
    }
}
