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
    public class DeviceTypeController : Controller
    {
        private MagicEntities db = new MagicEntities();

        // GET: DeviceType
        public ActionResult Index()
        {
            var a_DeviceType = db.A_DeviceType.Include(a => a.A_Device);
            return View(a_DeviceType.ToList());
        }

        // GET: DeviceType/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_DeviceType a_DeviceType = db.A_DeviceType.Find(id);
            if (a_DeviceType == null)
            {
                return HttpNotFound();
            }
            return View(a_DeviceType);
        }

        // GET: DeviceType/Create
        public ActionResult Create()
        {
            ViewBag.ID = new SelectList(db.A_Device, "ID", "Status");
            return View();
        }

        // POST: DeviceType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DeviceName")] A_DeviceType a_DeviceType)
        {
            if (ModelState.IsValid)
            {
                db.A_DeviceType.Add(a_DeviceType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID = new SelectList(db.A_Device, "ID", "Status", a_DeviceType.ID);
            return View(a_DeviceType);
        }

        // GET: DeviceType/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_DeviceType a_DeviceType = db.A_DeviceType.Find(id);
            if (a_DeviceType == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.A_Device, "ID", "Status", a_DeviceType.ID);
            return View(a_DeviceType);
        }

        // POST: DeviceType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DeviceName")] A_DeviceType a_DeviceType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(a_DeviceType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID = new SelectList(db.A_Device, "ID", "Status", a_DeviceType.ID);
            return View(a_DeviceType);
        }

        // GET: DeviceType/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_DeviceType a_DeviceType = db.A_DeviceType.Find(id);
            if (a_DeviceType == null)
            {
                return HttpNotFound();
            }
            return View(a_DeviceType);
        }

        // POST: DeviceType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            A_DeviceType a_DeviceType = db.A_DeviceType.Find(id);
            db.A_DeviceType.Remove(a_DeviceType);
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
