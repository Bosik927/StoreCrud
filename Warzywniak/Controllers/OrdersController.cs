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
	public class OrdersController : Controller
	{
		private WarzywniakEntities db = new WarzywniakEntities();

		// GET: Orders
		public ActionResult Index(string sortOrder)
		{
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Order date" ? "date_desc" : "Order date";

            var orders = db.Orders.Include(o => o.User);

            switch (sortOrder)
            {
                case "name_desc":
                    orders = orders.OrderByDescending(o => o.User.Nick);
                    break;
                case "Order date":
                    orders = orders.OrderBy(o => o.OrdareDate);
                    break;
                case "date_desc":
                    orders = orders.OrderByDescending(o => o.OrdareDate);
                    break;
                default:
                    orders = orders.OrderBy(o => o.User.Nick);
                    break;
            }
			return View(orders.ToList());
		}

        public ActionResult SaleSummary(string param)
        {
            DateTime todayDate = DateTime.Today;
            var beginDate = new DateTime(todayDate.Year, todayDate.Month, 1);
            var endDate = beginDate.AddMonths(1).AddDays(-1);

            ViewBag.BeginDate = beginDate.ToString("dd.MM.yyyy");
            ViewBag.EndDate = endDate.ToString("dd.MM.yyyy");

            var summary = db.SaleSummaryN(beginDate, endDate);
            decimal total = 0;
            List<SaleSummaryN_Result> saleSummary = new List<SaleSummaryN_Result>();
            foreach (var q in summary)
            {
                total += (decimal)q.Summ;
                saleSummary.Add(q);
            }
            summary = db.SaleSummaryN(beginDate, endDate);
            ViewBag.Total = total;
            return View(saleSummary);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult SaleSummary([Bind(Include = "BeginDate, EndDate")] SaleSummaryN_Result param)
        //{
        //    ViewBag.Comunicate = null;

        //    var beginDate = param.BeginDate;
        //    var endDate = param.EndDate.AddDays(1);

        //    ViewBag.BeginDate = beginDate.ToString("dd.MM.yyyy");
        //    ViewBag.EndDate = endDate.AddDays(-1).ToString("dd.MM.yyyy");

        //    var summary = db.SaleSummaryN(beginDate, endDate);
        //    return View(summary);
        //    return View();
        //}

            // GET: Orders/Details/5
            public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Order order = db.Orders.Find(id);

            List < OrderProduct > opList = new List<OrderProduct>();
            foreach(var op in order.OrderProducts)
            {
                opList.Add(new OrderProduct(op, op.Quantity * op.Product.ProductPrice));
            }
            order.OrderProducts = opList;
			if (order == null)
			{
				return HttpNotFound();
			}
			return View(order);
		}

		// GET: Orders/Create
		public ActionResult Create()
		{
			ViewBag.UserId = new SelectList(db.Users, "UserId", "Nick");
			return View();
		}



		// POST: Orders/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "OrderId,UserId,OrdareDate,Realized,RowVersion")] Order order)
		{
			ViewBag.Comunicate = null;
			ViewBag.UserId = new SelectList(db.Users, "UserId", "Nick", order.UserId);

			try
			{
				if (ModelState.IsValid)
				{
                    order.Realized = false;
					db.Orders.Add(order);
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
			catch (Exception e)
			{
				ViewBag.Comunicate = "Unknown error!";
				return View();
			}

			return View(order);
		}

		// [HttpPost]
		// [ValidateAntiForgeryToken]
		public ActionResult AddProducts(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			OrderProduct orderProduct = new OrderProduct();
			orderProduct.OrderId = id.Value;
			if (orderProduct == null)
			{
				return HttpNotFound();
			}
			ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName");
			return View(orderProduct);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddProducts([Bind(Include = "OrderProductId,ProductId,OrderId,Quantity,RowVersion")] OrderProduct orderProduct)
		{
			ViewBag.Comunicate = null;

            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "OrderId", orderProduct.OrderId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", orderProduct.ProductId);

            try
			{
				if (ModelState.IsValid)
				{
					db.OrderProducts.Add(orderProduct);
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
            catch (DataException e)
            {
                ViewBag.Comunicate = e.InnerException.InnerException != null ?
                      e.InnerException.InnerException.Message : e.InnerException.Message;

                return View();
            }
            catch (Exception e)
			{
				ViewBag.Comunicate = "Unknown error!";
				return View();
			}
			return View(orderProduct);
		}


		// GET: Orders/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Order order = db.Orders.Find(id);
			if (order == null)
			{
				return HttpNotFound();
			}
			ViewBag.UserId = new SelectList(db.Users, "UserId", "Nick", order.UserId);
			return View(order);
		}

		// POST: Orders/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "OrderId,UserId,OrdareDate,Realized,RowVersion")] Order order)
		{
			ViewBag.Comunicate = null;
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Nick", order.UserId);

            try
            {
				if (ModelState.IsValid)
				{
					db.Entry(order).State = EntityState.Modified;
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
			catch (Exception e)
			{
				ViewBag.Comunicate = "Unknown error!";
				return View();
			}
			return View(order);
		}

		// GET: Orders/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Order order = db.Orders.Find(id);
			if (order == null)
			{
				return HttpNotFound();
			}
			return View(order);
		}

		// POST: Orders/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Order order = db.Orders.Find(id);
			db.Orders.Remove(order);
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
