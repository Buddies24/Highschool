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
    public class StudentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Helpers helpers = new Helpers();

        // GET: Students
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Students.ToList());
        }
        // GET: Students
        [Authorize]
        public ActionResult Personal()
        {
            var stud = User.Identity.GetUserName();
            return View(db.Students.Where(a=>a.Email==stud).ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Manage()
        {
            return View();
        }

        public ActionResult CommingSoon()
        {
            return View();
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create(string email)
        {
            if (email == null)
            {
                return RedirectToAction("Register", "Account");
            }
            ViewBag.Email = email;
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,Name,MidName,Surname,idNum,phone,PrevSchoolReport,country,street_number,route,administrative_area_level_1,locality,postal_code,adress,CurrentGrade,gradeId,gradeName,dateRegistered")] Student student)
        {
            ViewBag.Email = student.Email;
            if (student.CurrentGrade.Equals(GradeEnum.None))
            {
                ViewBag.SelectGrade = $"{student.Name} select grade you are applying for";
                return View(student);
            }
            student.dateRegistered = DateTime.Now.ToString();
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(student);
        }
        //Login
        public ActionResult Login()
        {

            return View();

        }
        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,Name,MidName,Surname,idNum,phone,PrevSchoolReport,country,street_number,route,administrative_area_level_1,locality,postal_code,adress,CurrentGrade,gradeId,gradeName,dateRegistered")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/UnAssigned
        [Authorize(Roles = "Admin")]
        public ActionResult UnAssigned()
        {
            return View(helpers.UnAssignedStudents());
        }
        // GET: Students/Assigned
        [Authorize(Roles = "Admin")]
        public ActionResult Assigned()
        {
            return View(helpers.AssignedStudents());
        }
        // GET: Students/AssignToGrade
        [Authorize(Roles = "Admin")]
        public ActionResult AssignToGrade(string id)
        {
            if (id == null)
            {
                return RedirectToAction("UnAssigned");
            }
            var student = helpers.GetStudent(Convert.ToInt32(id));
            ViewBag.student = new List<SelectListItem>() {
                                    new SelectListItem {
                                        Text = string.IsNullOrEmpty(student.MidName) ? $"{student.Name} {student.Surname}" : $"{student.Name} {student.MidName} {student.Surname}",
                                        Value = student.Id.ToString() }};
            var grades = helpers.GetGradesWithSpaces(Convert.ToInt32(id));
            ViewBag.grades = new SelectList(grades, "Id", "GradeName");
            return View();
        }

        // GET: Students/AssignToGrade
        [HttpPost]
        public ActionResult AssignToGrade(string studentId, string gradeId)
        {
            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(gradeId))
            {
                return RedirectToAction("UnAssigned");
            }
            var student = helpers.GetStudent(Convert.ToInt32(studentId));
            var grade = helpers.GetGradeByID(Convert.ToInt32(gradeId));
            var classroom = helpers.GetClassroom(grade.ClassroomId);
            if (grade.MaxNumOfStudsInClass >= grade.NumOfStudentsInClass)
            {
                student.gradeId = grade.Id;
                student.gradeName = grade.GradeName;
                grade.NumOfStudentsInClass += 1;
                if (grade.NumOfStudentsInClass.Equals(grade.MaxNumOfStudsInClass))
                {
                    classroom.IsAvailable = false;
                }
                var gradeSubjects = helpers.GetGradeSubjects(grade.Id);
                if (gradeSubjects!=null)
                {
                    var addHistoryResponse= helpers.AddStudSubjectsToHistroy(grade.Id, student.Id, SubjectStatusEnum.NewToSchoolAndGrade, gradeSubjects);
                    if (addHistoryResponse.Equals(OperationStatus.Created))
                    {
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("UnAssigned");
            }
            else
            {
                classroom.IsAvailable = false;
                db.SaveChanges();
                return RedirectToAction("UnAssigned");
            }
        }


        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
