using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControleDeEstoque.Data;
using ControleDeEstoque.Models;
using System.Collections;

namespace ControleDeEstoque.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ControleDeEstoqueContext _context;

        public OrdersController(ControleDeEstoqueContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var controleDeEstoqueContext = await _context.Orders
                .Include(o => o.Stock.Product)
                .Include(o => o.Supplier)
                .OrderBy(o=>o.Amount)
                .ToListAsync();

            return View(controleDeEstoqueContext);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Stock.Product)
                .Include(o => o.Supplier)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["StockId"] = new SelectList(_context.Stocks.Include(s=>s.Product), "Id", "Product.Name");
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SupplierId,StockId,Amount,Id,CreatedAt,UpdatedAt")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                var stock = _context.Stocks.FirstOrDefault(s => s.Id == order.StockId);
                if (stock != null)
                {
                    stock.UpdatedAt = DateTime.Now;
                    stock.Amount += order.Amount;
                    _context.Update(stock);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StockId"] = new SelectList(_context.Stocks.Include(s=>s.Product), "Id", "Product.Name", order.StockId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name", order.SupplierId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["StockId"] = new SelectList(_context.Stocks.Include(s=>s.Product), "Id", "Product.Name", order.StockId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name", order.SupplierId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("SupplierId,StockId,Amount,Id,CreatedAt,UpdatedAt")] Order order)
        {
            if (id != order.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                    var existingOrder = await _context.Orders
                        .Include(o => o.Stock)
                        .FirstOrDefaultAsync(o => o.Id == id);

                    if (existingOrder == null)
                        return NotFound();

                    if (existingOrder.StockId != order.StockId)
                    {
                        var oldStock = await _context.Stocks.FindAsync(existingOrder.StockId);
                        if (oldStock != null)
                            oldStock.Amount -= existingOrder.Amount;


                        var newStock = await _context.Stocks.FindAsync(order.StockId);
                        if (newStock != null)
                            newStock.Amount += order.Amount;
                    }
                    else
                    {
                        var stock = await _context.Stocks.FindAsync(order.StockId);
                        if (stock != null)
                        {
                            stock.UpdatedAt = DateTime.Now;
                            stock.Amount -= existingOrder.Amount;
                            stock.Amount += order.Amount;
                        }
                    }

                    // Atualiza os campos da ordem existente
                    existingOrder.SupplierId = order.SupplierId;
                    existingOrder.StockId = order.StockId;
                    existingOrder.Amount = order.Amount;
                    existingOrder.UpdatedAt = DateTime.Now;

                    await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var stocks = await _context.Stocks.Include(s => s.Product).ToListAsync();
            ViewData["StockId"] = new SelectList(stocks, "Id", "Product.Name", order.StockId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name", order.SupplierId);

            return View(order);
        }


        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Stock.Product)
                .Include(o => o.Supplier)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                var stock = await _context.Stocks.FindAsync(order.StockId);
                if (stock != null)
                {
                    stock.UpdatedAt = DateTime.Now;
                    stock.Amount -= order.Amount;
                    _context.Update(stock);
                }
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(Guid id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
