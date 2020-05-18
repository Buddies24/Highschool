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
    [Authorize(Roles ="Admin")]
    public class GradesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Helpers helpers = new Helpers();

        // GET: Grades
        public ActionResult Index()
        {
            return View(db.Grades.ToList());
        }

        // GET: Grades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grade grade = db.Grades.Find(id);
            if (grade == null)
            {
                return HttpNotFound();
            }
            return View(grade);
        }

        // GET: Grades/Create
        public ActionResult Create()
        {
            var roomsAvailable = helpers.GetUnselectedClassroom();
            if (roomsAvailable.Count.Equals(0))
            {
                return RedirectToAction("Create", "Classrooms");
            }
            ViewBag.roomsAvailable = new SelectList(roomsAvailable, "Id", "RoomName");
            return View();
        }

        // POST: Grades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,GradeNum,GradeLetter,GradeName,ClassroomId,NumOfStudentsInClass,MaxNumOfStudsInClass,teacherId")] Grade grade)
        {
            var roomsAvailable = helpers.GetUnselectedClassroom();
            ViewBag.roomsAvailable = new SelectList(roomsAvailable, "Id", "RoomName");

            if (helpers.GradeAlreadyExists(grade.GradeNum, grade.GradeLetter))
            {
                ViewBag.Response = $"Grade {grade.GradeNum} {grade.GradeLetter} Already Exists";
                return View(grade);
            }

            if (grade.GradeNum.Equals(GradeEnum.None))
            {
                ViewBag.Response = "Please Select grade";
                return View(grade);
            }

            grade.GradeName = $"{grade.GradeNum} {grade.GradeLetter}";
            var classroom = helpers.GetClassroom(grade.ClassroomId);
            grade.MaxNumOfStudsInClass = classroom.MaxCapacity;
            
            if (ModelState.IsValid) 
            {
                db.Grades.Add(grade);
                db.SaveChanges();
                classroom.gradeId = grade.Id;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(grade);
        }

        // GET: Grades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grade grade = db.Grades.Find(id);
            if (grade == null)
            {
                return HttpNotFound();
            }
            ViewBag.roomsAvailable = helpers.GetUnselectedClassroom();
            return View(grade);
        }

        // POST: Grades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,GradeNum,GradeLetter,GradeName,ClassroomId,NumOfStudentsInClass,MaxNumOfStudsInClass,teacherId")] Grade grade)
        {
            ViewBag.roomsAvailable = helpers.GetUnselectedClassroom();
            if (helpers.GradeAlreadyExists(grade.GradeNum, grade.GradeLetter))
            {
                ViewBag.Response = $"Grade {grade.GradeNum} {grade.GradeLetter} Already Exists";
                return View(grade);
            }

            grade.GradeName = $"{grade.GradeNum} {grade.GradeLetter}";

            if (ModelState.IsValid)
            {
                db.Entry(grade).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(grade);
        }

        // GET: Grades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grade grade = db.Grades.Find(id);
            if (grade == null)
            {
                return HttpNotFound();
            }
            return View(grade);
        }

        // POST: Grades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Grade grade = db.Grades.Find(id);
            db.Grades.Remove(grade);
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
