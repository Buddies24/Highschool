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
    public class TeachersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Helpers helpers = new Helpers();
        // GET: Teachers
        public ActionResult Index()
        {
            return View(db.Teachers.ToList());
        }

        [Authorize(Roles ="Teacher")]
        public ActionResult MyStudents()
        {
            var email = User.Identity.GetUserName();
            var teacher = db.Teachers.FirstOrDefault(a => a.Email == email);
            return View(db.Students.Where(a=>a.gradeId==teacher.gradeId));
        }

        // GET: Teachers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // GET: Teachers/Create
        public ActionResult Create(string email)
        {
            if (email == null)
            {
                return RedirectToAction("Register", "Account");
            }
            var subjects = helpers.GetSubjectWithoutTeacher();
            var selectList = new List<SelectListItem>();
            if (subjects != null)
            {
                foreach (var item in subjects.OrderBy(a => a.GradeNumber))
                {
                    var selectable = new SelectListItem()
                    {
                        Text = $"{item.SubjectName} - Grade {item.GradeNumber}",
                        Value = item.Id.ToString()
                    };
                    selectList.Add(selectable);
                }
            }

            ViewBag.subjects = selectList;
            var grades = helpers.GetGradesWithoutTeacher();
            ViewBag.grades = new SelectList(grades, "Id", "GradeName");
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,Name,MidName,Surname,IdNum,phone,gradeId,SubjectId")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Teachers.Add(teacher);
                db.SaveChanges();
                if (!helpers.UpdateGradeAndSubjectTeacher(teacher.SubjectId, teacher.gradeId, teacher.Id)
                .Equals(OperationStatus.Updated))
                {
                    db.Teachers.Remove(teacher);
                    db.SaveChanges();

                    var subjectsAfterRemove = helpers.GetSubjectWithoutTeacher();
                    var selectListAfterRemove = new List<SelectListItem>();
                    if (subjectsAfterRemove != null)
                    {
                        foreach (var item in subjectsAfterRemove.OrderBy(a => a.GradeNumber))
                        {
                            var selectable = new SelectListItem()
                            {
                                Text = $"{item.SubjectName} - Grade {item.GradeNumber}",
                                Value = item.Id.ToString()
                            };
                            selectListAfterRemove.Add(selectable);
                        }
                    }
                    ViewBag.subjects = selectListAfterRemove;
                    var gradesAfterRemove = helpers.GetGradesWithoutTeacher();
                    ViewBag.grades = new SelectList(gradesAfterRemove, "Id", "GradeName");
                    return View(teacher);
                }
                return RedirectToAction("Index");
            }

            var subjects = helpers.GetSubjectWithoutTeacher();
            var selectList = new List<SelectListItem>();
            if (subjects != null)
            {
                foreach (var item in subjects.OrderBy(a => a.GradeNumber))
                {
                    var selectable = new SelectListItem()
                    {
                        Text = $"{item.SubjectName} - Grade {item.GradeNumber}",
                        Value = item.Id.ToString()
                    };
                    selectList.Add(selectable);
                }
            }
            ViewBag.subjects = selectList;
            var grades = helpers.GetGradesWithoutTeacher();
            ViewBag.grades = new SelectList(grades, "Id", "GradeName");
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,Name,MidName,Surname,IdNum,phone,gradeId,SubjectId")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Teacher teacher = db.Teachers.Find(id);
            db.Teachers.Remove(teacher);
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
