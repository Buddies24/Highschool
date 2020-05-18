using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using School.Models;

namespace School.Controllers
{
    public class StudentGradeSubjectHistoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Helpers helpers = new Helpers();

        // GET: StudentGradeSubjectHistories
        [Authorize(Roles = "Admin,Teacher")]
        public ActionResult Index()
        {
            return View(db.StudentGradeSubjectHistories.ToList());
        }

        // GET: StudentGradeSubjectHistories
        [Authorize]
        public ActionResult MyHistory()
        {
            if (User.IsInRole("Teacher") || User.IsInRole("Admin"))
            {
                return RedirectToAction("Index");
            }
            var email = User.Identity.GetUserName();
            var student = db.Students.FirstOrDefault(a => a.Email == email);
            return View(db.StudentGradeSubjectHistories.Where(a=>a.StudentId==student.Id).ToList());
        }

        // GET: StudentGradeSubjectHistories/StudentHistory
        [Authorize(Roles ="Admin,Teacher")]
        public ActionResult StudentHistory(int id)
        {
            var student = helpers.GetStudent(id);
            if (student == null)
            {
                return RedirectToAction("Index", "Students");
            }
            ViewBag.Student = string.IsNullOrEmpty(student.MidName) ? $"{student.Name} {student.Surname}" : $"{student.Name} {student.MidName} {student.Surname}";
            return View(db.StudentGradeSubjectHistories.Where(a => a.StudentId == id).ToList());
        }

        // GET: StudentGradeSubjectHistories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentGradeSubjectHistory studentGradeSubjectHistory = db.StudentGradeSubjectHistories.Find(id);
            if (studentGradeSubjectHistory == null)
            {
                return HttpNotFound();
            }
            return View(studentGradeSubjectHistory);
        }

        // GET: StudentGradeSubjectHistories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StudentGradeSubjectHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StudentId,GradeSubectId,StudentGrade,SubjectGrade,StudentMark")] StudentGradeSubjectHistory studentGradeSubjectHistory)
        {
            if (ModelState.IsValid)
            {
                db.StudentGradeSubjectHistories.Add(studentGradeSubjectHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(studentGradeSubjectHistory);
        }

        // GET: StudentGradeSubjectHistories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentGradeSubjectHistory studentGradeSubjectHistory = db.StudentGradeSubjectHistories.Find(id);
            if (studentGradeSubjectHistory == null)
            {
                return HttpNotFound();
            }
            return View(studentGradeSubjectHistory);
        }

        // POST: StudentGradeSubjectHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentId,GradeSubectId,StudentGrade,SubjectGrade,StudentMark")] StudentGradeSubjectHistory studentGradeSubjectHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentGradeSubjectHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(studentGradeSubjectHistory);
        }

        // GET: StudentGradeSubjectHistories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentGradeSubjectHistory studentGradeSubjectHistory = db.StudentGradeSubjectHistories.Find(id);
            if (studentGradeSubjectHistory == null)
            {
                return HttpNotFound();
            }
            return View(studentGradeSubjectHistory);
        }

        // POST: StudentGradeSubjectHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentGradeSubjectHistory studentGradeSubjectHistory = db.StudentGradeSubjectHistories.Find(id);
            db.StudentGradeSubjectHistories.Remove(studentGradeSubjectHistory);
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
