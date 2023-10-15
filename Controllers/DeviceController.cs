using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.WebPages;
using System.Windows.Input;
using MagicWarehouse.Data;
using MagicWarehouse.Models;
using OfficeOpenXml;
using PagedList;
using PagedList.Mvc;



namespace MagicWarehouse.Controllers
{
    public class DeviceController : Controller
    {
        private MagicEntities db = new MagicEntities();

        // GET: Device
        public ActionResult Index(int? i, string searchString)
        {
            // Your existing code for displaying the list of devices
            int pageSize = 15;
            int pageIndex = !i.HasValue ? 1 : i.Value;
            var a_Device = db.A_Device.Where(X => string.IsNullOrEmpty(searchString)
                || (!string.IsNullOrEmpty(searchString) && X.IMEI.Contains(searchString))).Include(a => a.A_DeviceType);

            ViewBag.CurrentFilter = searchString;
            return View(a_Device.ToList().ToPagedList(pageIndex, pageSize));
        }

        // GET: Device/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_Device a_Device = db.A_Device.Find(id);
            if (a_Device == null)
            {
                return HttpNotFound();
            }
            return View(a_Device);
        }

        // GET: Device/Create
        public ActionResult Create()
        {
            ViewBag.ID = new SelectList(db.A_DeviceType, "ID", "DeviceName");
            return View();
        }


        // POST: Device/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IMEI,DeviceTypeID,ReceivedDateProvider,Status")] A_Device a_Device)
        {
            if (ModelState.IsValid)
            {
                string imeiPattern = @"^\d{15}$";
                if (!Regex.IsMatch(a_Device.IMEI, imeiPattern))
                {
                    ModelState.AddModelError("IMEI", "IMEI must be exactly 15 digits long and contain only numbers.");
                    ViewBag.ID = new SelectList(db.A_DeviceType, "ID", "DeviceName", a_Device.ID);
                    return View(a_Device);
                }
                db.A_Device.Add(a_Device);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID = new SelectList(db.A_DeviceType, "ID", "DeviceName", a_Device.ID);
            return View(a_Device);

        }

        // GET: Device/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_Device a_Device = db.A_Device.Find(id);
            if (a_Device == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.A_DeviceType, "ID", "DeviceName", a_Device.ID);
            return View(a_Device);
            if (ModelState.IsValid)
            {
                // Validate the IMEI format using a regular expression
                string imeiPattern = @"^\d{15}$";
                if (!Regex.IsMatch(a_Device.IMEI, imeiPattern))
                {
                    ModelState.AddModelError("IMEI", "IMEI must be exactly 15 digits long and contain only numbers.");
                    ViewBag.ID = new SelectList(db.A_DeviceType, "ID", "DeviceName", a_Device.ID);
                    return View(a_Device);
                }

                db.Entry(a_Device).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID = new SelectList(db.A_DeviceType, "ID", "DeviceName", a_Device.ID);
            return View(a_Device);

        }

        // POST: Device/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string uploadDirectory = Server.MapPath("~/UploadedFiles");

                    if (!Directory.Exists(uploadDirectory))
                    {
                        Directory.CreateDirectory(uploadDirectory);
                    }

                    string filePath = Path.Combine(uploadDirectory, fileName);
                    file.SaveAs(filePath);

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
                            A_Device a_Device = new A_Device();
                            a_Device.IMEI = device["IMEI"];
                            a_Device.DeviceTypeID = int.Parse(device["DeviceTypeID"]);
                            a_Device.Status = device["Status"];
                            a_Device.ReceivedDateProvider = device["ReceivedDateProvider"].AsDateTime();
                            db.A_Device.Add(a_Device);
                            db.SaveChanges();
                        }
                        TempData["Message"] = "File Upload Success";
                        //var parameters = new List<object>();
                        //parameters.Add(new SqlParameter("device", a_Devices));

                        // Replace "yourFilePath" with the actual path to your Excel file
                        // var Reuslt = db.Database.SqlQuery<A_Device>("exec InsertDeviceData @device ", parameters).ToList<A_Device>();



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
        // GET: Device/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_Device a_Device = db.A_Device.Find(id);
            if (a_Device == null)
            {
                return HttpNotFound();
            }
            return View(a_Device);
        }

        // POST: Device/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            A_Device a_Device = db.A_Device.Find(id);
            db.A_Device.Remove(a_Device);
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