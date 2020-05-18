using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using School.Models;

namespace School.Controllers
{
    public class StreamsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Streams
        public ActionResult Index()
        {
            
            return RedirectToAction("CommingSoon", "Students"); 
        }

        // GET: Streams/Details/5
        public ActionResult Details(int? id)
        {
           return RedirectToAction("CommingSoon", "Students");
        }

        // GET: Streams/Create
        public ActionResult Create()
        {
            return RedirectToAction("CommingSoon", "Students");
        }

        // POST: Streams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StreamName,StreamSubjects,PreCompulsorySubjects")] Stream stream)
        {
            if (ModelState.IsValid)
            {
                db.Streams.Add(stream);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(stream);
        }

        // GET: Streams/Edit/5
        public ActionResult Edit(int? id)
        {
            return RedirectToAction("CommingSoon", "Students");
        }

        // POST: Streams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StreamName,StreamSubjects,PreCompulsorySubjects")] Stream stream)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stream).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stream);
        }

        // GET: Streams/Delete/5
        public ActionResult Delete(int? id)
        {
            return RedirectToAction("CommingSoon", "Students");
        }

        // POST: Streams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stream stream = db.Streams.Find(id);
            db.Streams.Remove(stream);
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
