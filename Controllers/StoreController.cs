using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MagicWarehouse.Data;

namespace MagicWarehouse.Controllers
{
    public class StoreController : Controller
    {
        private MagicEntities db = new MagicEntities();

        // GET: Store
        public ActionResult Index()
        {
            return View(db.A_Store.ToList());
        }

        // GET: Store/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_Store a_Store = db.A_Store.Find(id);
            if (a_Store == null)
            {
                return HttpNotFound();
            }
            return View(a_Store);
        }

        // GET: Store/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Store/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Address,StoreType,CreatedOn,CreatedBy,Status")] A_Store a_Store)
        {
            if (ModelState.IsValid)
            {
                db.A_Store.Add(a_Store);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(a_Store);
        }

        // GET: Store/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_Store a_Store = db.A_Store.Find(id);
            if (a_Store == null)
            {
                return HttpNotFound();
            }
            return View(a_Store);
        }

        // POST: Store/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Address,StoreType,CreatedOn,CreatedBy,Status")] A_Store a_Store)
        {
            if (ModelState.IsValid)
            {
                db.Entry(a_Store).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(a_Store);
        }

        // GET: Store/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_Store a_Store = db.A_Store.Find(id);
            if (a_Store == null)
            {
                return HttpNotFound();
            }
            return View(a_Store);
        }

        // POST: Store/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            A_Store a_Store = db.A_Store.Find(id);
            db.A_Store.Remove(a_Store);
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
