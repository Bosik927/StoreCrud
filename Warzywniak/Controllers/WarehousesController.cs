using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Warzywniak;

namespace Warzywniak.Controllers
{
	public class WarehousesController : Controller
	{
		private WarzywniakEntities db = new WarzywniakEntities();

		// GET: Warehouses
		public ActionResult Index()
		{
            var warehouses = db.Warehouses.Include(w => w.Product);
            return View(warehouses.ToList());
		}

		// GET: Warehouses/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Warehouse warehouse = db.Warehouses.Find(id);
			if (warehouse == null)
			{
				return HttpNotFound();
			}
			return View(warehouse);
		}

		// GET: Warehouses/Create
		public ActionResult Create()
		{
			ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName");
			return View();
		}

		// POST: Warehouses/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "WarehouseId,ProductId,ExpiryDate,Quantity")] Warehouse warehouse)
		{
			ViewBag.Comunicate = null;
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName");

            try
            {

                if (ModelState.IsValid)
				{
                    
                    db.Warehouses.Add(warehouse);
                    db.SaveChanges();
                    return RedirectToAction("Index");
				}
				else
				{
					throw new ArgumentException("Invalid arguments!");
				}
			}
            catch (DbUpdateConcurrencyException e)
            {
                ViewBag.Comunicate = "Concurrnet exception occur!";
                return View();
            }
            catch (ArgumentException e)
			{
				ViewBag.Comunicate = e.Message;
                return View();
			}
			catch (DataException e)
			{
                ViewBag.Comunicate = e.InnerException.InnerException != null ?
                      e.InnerException.InnerException.Message : e.InnerException.Message;
                
                return View();
			}
			catch (Exception e)
			{
				ViewBag.Comunicate = e.Message;
				return View();
			}
			return View(warehouse);
		}

		// GET: Warehouses/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Warehouse warehouse = db.Warehouses.Find(id);
			if (warehouse == null)
			{
				return HttpNotFound();
			}
			ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", warehouse.ProductId);
			return View(warehouse);
		}

		// POST: Warehouses/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "WarehouseId,ProductId,ExpiryDate,Quantity")] Warehouse warehouse)
		{
			ViewBag.Comunicate = null;
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", warehouse.ProductId);

            try
            {
				if (ModelState.IsValid)
				{
					db.Entry(warehouse).State = EntityState.Modified;
					db.SaveChanges();
                    
					return RedirectToAction("Index");
				}
				else
				{
					throw new ArgumentException("Invalid arguments!");
				}
			}
			catch (ArgumentException e)
			{
				ViewBag.Comunicate = e.Message;
				return View();
			}
			catch (DataException e)
			{
				ViewBag.Comunicate = e.InnerException.InnerException.Message;
				return View();
			}
			catch (Exception e)
			{
				ViewBag.Comunicate = e.Message;
				return View();
			}
			return View(warehouse);
		}

		// GET: Warehouses/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Warehouse warehouse = db.Warehouses.Find(id);
			if (warehouse == null)
			{
				return HttpNotFound();
			}
			return View(warehouse);
		}

		// POST: Warehouses/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Warehouse warehouse = db.Warehouses.Find(id);
			db.Warehouses.Remove(warehouse);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
