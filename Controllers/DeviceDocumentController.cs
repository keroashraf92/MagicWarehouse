using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MagicWarehouse.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
namespace MagicWarehouse.Controllers
{
    
    public class DeviceDocumentController : Controller
    {
        private MagicEntities db = new MagicEntities();

        // GET: DeviceDocument
        public ActionResult Index()
        {
            return View(db.A_DeviceDocument.ToList());
        }

        // GET: DeviceDocument/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_DeviceDocument a_DeviceDocument = db.A_DeviceDocument.Find(id);
            if (a_DeviceDocument == null)
            {
                return HttpNotFound();
            }
            return View(a_DeviceDocument);
        }

        // GET: DeviceDocument/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeviceDocument/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DocumentPath,DeviceID,CreatedOn,CreatedBy")] A_DeviceDocument a_DeviceDocument)
        {
            if (ModelState.IsValid)
            {
                db.A_DeviceDocument.Add(a_DeviceDocument);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(a_DeviceDocument);
        }

        [HttpPost]
        public ActionResult UploadPhoto(PhotoUploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.PhotoFile != null && model.PhotoFile.ContentLength > 0)
                {
                    // Save the uploaded photo to a location
                    var fileName = Path.GetFileName(model.PhotoFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);

                    model.PhotoFile.SaveAs(path);

                    // Optionally, you can save the file path to a database for later retrieval
                    // deviceDocumentService.SaveFilePath(path);

                    return RedirectToAction("Index", "Home"); // Redirect to a different page after upload
                }
            }

            // If the model is not valid or there was an issue with the upload, return to the upload page
            return View("Upload", model);
        }


        // GET: DeviceDocument/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_DeviceDocument a_DeviceDocument = db.A_DeviceDocument.Find(id);
            if (a_DeviceDocument == null)
            {
                return HttpNotFound();
            }
            return View(a_DeviceDocument);
        }

        // POST: DeviceDocument/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DocumentPath,DeviceID,CreatedOn,CreatedBy")] A_DeviceDocument a_DeviceDocument)
        {
            if (ModelState.IsValid)
            {
                db.Entry(a_DeviceDocument).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(a_DeviceDocument);
        }

        // GET: DeviceDocument/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_DeviceDocument a_DeviceDocument = db.A_DeviceDocument.Find(id);
            if (a_DeviceDocument == null)
            {
                return HttpNotFound();
            }
            return View(a_DeviceDocument);
        }

        // POST: DeviceDocument/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            A_DeviceDocument a_DeviceDocument = db.A_DeviceDocument.Find(id);
            db.A_DeviceDocument.Remove(a_DeviceDocument);
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
