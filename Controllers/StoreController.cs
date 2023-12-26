using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using MagicWarehouse.Data;
using OfficeOpenXml;

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

        // GET: Store/Transfer/5
        public ActionResult Transfer(int? id)
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
            //entity framework command
            //equal to "select * from Store where id <> id"
            List<A_Store> storeList = new List<A_Store>();
            if (a_Store.StoreType == "1")
                storeList = db.A_Store.Where(x => x.ID != id).ToList();
            else
                storeList = db.A_Store.Where(x => x.ID != id && x.StoreType != "2").ToList();

            ViewBag.StoreList = storeList;
            return View(a_Store);
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase fileUpload)
        {
            if (fileUpload is null)
            {
                throw new ArgumentNullException(nameof(fileUpload));
            }

            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                try
                {
                    string fileName = Path.GetFileName(fileUpload.FileName);
                    string uploadDirectory = Server.MapPath("~/UploadedFiles");

                    if (!Directory.Exists(uploadDirectory))
                    {
                        Directory.CreateDirectory(uploadDirectory);
                    }

                    string filePath = Path.Combine(uploadDirectory, fileName);
                    fileUpload.SaveAs(filePath);

                    //------------------------------------------------------//

                    // Replace "yourFilePath" with the actual path to your Excel file
                    FileInfo fileInfo = new FileInfo(filePath);

                    using (var package = new ExcelPackage(fileInfo))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"];

                        int totalRows = worksheet.Dimension.Rows;
                        int totalColumns = worksheet.Dimension.Columns;

                        List<string> columnHeaders = new List<string>();
                        for (int col = 1; col <= totalColumns; col++)
                        {
                            string columnHeader = worksheet.Cells[1, col].Value?.ToString();
                            columnHeaders.Add(columnHeader);
                        }

                        List<Dictionary<string, string>> rowDataList = new List<Dictionary<string, string>>();
                        for (int row = 2; row <= totalRows; row++)
                        {
                            Dictionary<string, string> rowData = new Dictionary<string, string>();
                            for (int col = 1; col <= totalColumns; col++)
                            {
                                string columnHeader = columnHeaders[col - 1];
                                string cellValue = worksheet.Cells[row, col].Value?.ToString();
                                rowData.Add(columnHeader, cellValue);
                            }
                            rowDataList.Add(rowData);
                        }
                        foreach (var device in rowDataList)
                        {
                            A_Device a_device = new A_Device();
                            a_device.IMEI = device["IMEI"];
                            db.A_Device.Add(a_device);
                            db.SaveChanges();
                        }
                        TempData["Message"] = "File Upload Success";
                    }
                    //-----------------------------------------------------//
                    TempData["Message"] = "File uploaded successfully!";
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that may occur during file upload
                    TempData["Message"] = "Error uploading file: " + ex.Message;
                }
            }
            else
            {
                // Handle the case where no file was selected
                TempData["Message"] = "Please select a file to upload.";
            }

            return RedirectToAction("Create");

            //return View();
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
