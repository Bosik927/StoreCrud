﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Warzywniak;

namespace Warzywniak.Controllers
{
	public class ProductsController : Controller
	{
		private WarzywniakEntities db = new WarzywniakEntities();

		// GET: Products
		public ActionResult Index()
		{
			return View(db.Products.ToList());
		}

		// GET: Products/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Product product = db.Products.Find(id);
			if (product == null)
			{
				return HttpNotFound();
			}
			return View(product);
		}

		// GET: Products/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Products/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "ProductId,ProductName,ProductPrice,ProductUnit,Vat")] Product product)
		{

			ViewBag.Comunicate = null;
			try
			{
				if (ModelState.IsValid)
				{
					product.ForDelete = false;
					db.Products.Add(product);
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

			return RedirectToAction("Index");
		}

		// GET: Products/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Product product = db.Products.Find(id);
			if (product == null)
			{
				return HttpNotFound();
			}
			return View(product);
		}

		// POST: Products/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "ProductId,ProductName,ProductPrice,ProductUnit,Vat,ForDelete,RowVersion")] Product product)
		{
			ViewBag.Comunicate = null;
			try
			{
				if (ModelState.IsValid)
			{
				db.Entry(product).State = EntityState.Modified;
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
			return View(product);
		}

		// GET: Products/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Product product = db.Products.Find(id);
			if (product == null)
			{
				return HttpNotFound();
			}
			return View(product);
		}

		// POST: Products/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Product product = db.Products.Find(id);
			db.Products.Remove(product);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

        public ActionResult BestSellingProduct(int amount)
        {
            bool sold = false;
            var bestSellingProduct = db.BestSellingProduct(amount, sold);
            return View(bestSellingProduct);

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
