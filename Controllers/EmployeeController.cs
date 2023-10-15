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
    public class EmployeeController : Controller
    {
        private MagicEntities db = new MagicEntities();

        // GET: Employee
        public ActionResult Index(string searchString) // The parameter name should match the input field name
        {
            var a_Employee = db.A_Employee.Include(a => a.A_Store);

            // Filter employees by name if searchString is not empty
            if (!string.IsNullOrEmpty(searchString))
            {
                a_Employee = a_Employee.Where(e => e.Name.Contains(searchString));
            }

            return View(a_Employee.ToList());
        }
        // GET: Employee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_Employee a_Employee = db.A_Employee.Find(id);
            if (a_Employee == null)
            {
                return HttpNotFound();
            }
            return View(a_Employee);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            ViewBag.Store_ID = new SelectList(db.A_Store, "ID", "Name");
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Phone,JobTitle,Store_ID,JoinningDate,UserName,Password,CreatedOn")] A_Employee a_Employee)
        {
            if (ModelState.IsValid)
            {
                db.A_Employee.Add(a_Employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Store_ID = new SelectList(db.A_Store, "ID", "Name", a_Employee.Store_ID);
            return View(a_Employee);
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_Employee a_Employee = db.A_Employee.Find(id);
            if (a_Employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.Store_ID = new SelectList(db.A_Store, "ID", "Name", a_Employee.Store_ID);
            return View(a_Employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Phone,JobTitle,Store_ID,JoinningDate,UserName,Password,CreatedOn")] A_Employee a_Employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(a_Employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Store_ID = new SelectList(db.A_Store, "ID", "Name", a_Employee.Store_ID);
            return View(a_Employee);
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_Employee a_Employee = db.A_Employee.Find(id);
            if (a_Employee == null)
            {
                return HttpNotFound();
            }
            return View(a_Employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            A_Employee a_Employee = db.A_Employee.Find(id);
            db.A_Employee.Remove(a_Employee);
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
