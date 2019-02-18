using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Warzywniak;


namespace Warzywniak.Controllers
{
    public class UsersController : Controller
    {
        private WarzywniakEntities db = new WarzywniakEntities();

        // GET: Users, only not deleted
        public ActionResult Index()
        {
            var users = from user in db.Users
                            //where user.ForDelete == false
                        select user;

            return View(users);
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            Adress adress = db.Adresses.FirstOrDefault(adr => adr.UserId == id);

            UserAdress userAddres = new UserAdress(user, adress);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(userAddres);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,Nick,Password,PhoneNumber,EmailAddress")] User user,
            [Bind(Include = "UserId,RoadName,HouseNumber")] Adress adress)
        {
            ViewBag.Comunicate = null;
            try
            {
                if (ModelState.IsValid)
                {
                    user.ForDelete = false;
                    db.Users.Add(user);
                    db.SaveChanges();

                    adress.User = user;
                    db.Adresses.Add(adress);
                    db.SaveChanges();
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
            catch (DbUpdateConcurrencyException e)
            {
                ViewBag.Comunicate = "Concurrnet exception occur!";
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

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            Adress adress = db.Adresses.FirstOrDefault(adr => adr.UserId == id);

            UserAdress userAddres = new UserAdress(user, adress);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(userAddres);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Nick,Password,PhoneNumber,EmailAddress,ForDelete, RowVersion")] User user,
            [Bind(Include = "AdressId,RoadName,HouseNumber,UserId")] Adress adress)
        {
            ViewBag.Comunicate = null;
            user.ForDelete = false;
            try
            {
                if (ModelState.IsValid)
                {
                    user.ForDelete = false;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    db.Entry(adress).State = EntityState.Modified;
                    db.SaveChanges();

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

            return RedirectToAction("Index");
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ShowAllOrders(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);

            var ordersIds = (from or in db.Orders
                             where or.UserId == id
                             select new { or.OrderId, or.OrdareDate, or.Realized }).Distinct().ToList();

            if (ordersIds == null)
            {
                return HttpNotFound();
            }

            List<OrderEntity> orderEntities = new List<OrderEntity>();
            decimal fullSum = 0;
            foreach (var orderId in ordersIds)
            {
                var entryPoint = (from or in db.Orders
                                  join op in db.OrderProducts on or.OrderId equals op.OrderId
                                  join pr in db.Products on op.ProductId equals pr.ProductId
                                  where or.OrderId == orderId.OrderId
                                  select new ProductEntity
                                  {
                                      ProductName = pr.ProductName,
                                      Quantity = op.Quantity,
                                      ProductUnit = pr.ProductUnit,
                                      Vat = pr.Vat,
                                      ProductPrice = pr.ProductPrice,
                                      Sum = op.Quantity * pr.ProductPrice,

                                  }).ToList();

                var query = from op in db.OrderProducts
                            join p in db.Products on op.ProductId equals p.ProductId
                            where op.OrderId.Value == orderId.OrderId
                            select new
                            {
                                Price = p.ProductPrice,
                                Quantity = op.Quantity,
                            };

                decimal sum = 0;
                foreach (var q in query)
                {
                    sum += q.Price * q.Quantity;

                }

                bool realized = (bool)orderId.Realized;
                orderEntities.Add(new OrderEntity(orderId.OrderId, entryPoint, orderId.OrdareDate.Value, realized, sum));
                fullSum += sum;
            }
            IList<User> u = new List<User>();
            u.Add(user);
            ViewData["user"] = u;
            ViewBag.Total = fullSum;

            //FullOrderEntity fullOrderEntity = new FullOrderEntity(orderEntities, fullSum);

            return View(orderEntities);
        }

        public ActionResult BestClients(int amount)
        {
            //amount = 10;

            var bestClients = db.BestClient(amount);
            return View(bestClients);

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
