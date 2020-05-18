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
    public class GradeSubjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Helpers helpers = new Helpers();

        // GET: GradeSubjects
        public ActionResult Index()
        {
            return View(db.GradeSubjects.ToList());
        }

        // GET: GradeSubjects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GradeSubject gradeSubject = db.GradeSubjects.Find(id);
            if (gradeSubject == null)
            {
                return HttpNotFound();
            }
            return View(gradeSubject);
        }

        // GET: GradeSubjects/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: GradeSubjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SubjectName,MinPassMark,GradeNumber,ListOfStreamsIdRequiredAt,teacherId")] GradeSubject gradeSubject)
        {
            if (gradeSubject.GradeNumber.Equals(GradeEnum.None))
            {
                ViewBag.Response = "Please Select grade";
                return View(gradeSubject);
            }

            if (helpers.GradeSubjectAlreadyExist(gradeSubject.GradeNumber,gradeSubject.SubjectName))
            {
                ViewBag.GradeSubjectAlreadyExist = "Subject has been added for the Grade Already";
                return View(gradeSubject);
            }

            gradeSubject.MinPassMark = 30;
            if (ModelState.IsValid)
            {
                db.GradeSubjects.Add(gradeSubject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gradeSubject);
        }

        // GET: GradeSubjects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GradeSubject gradeSubject = db.GradeSubjects.Find(id);
            if (gradeSubject == null)
            {
                return HttpNotFound();
            }
            return View(gradeSubject);
        }

        // POST: GradeSubjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SubjectName,MinPassMark,GradeNumber,ListOfStreamsIdRequiredAt,teacherId")] GradeSubject gradeSubject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gradeSubject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gradeSubject);
        }

        // GET: GradeSubjects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GradeSubject gradeSubject = db.GradeSubjects.Find(id);
            if (gradeSubject == null)
            {
                return HttpNotFound();
            }
            return View(gradeSubject);
        }

        // POST: GradeSubjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GradeSubject gradeSubject = db.GradeSubjects.Find(id);
            db.GradeSubjects.Remove(gradeSubject);
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
