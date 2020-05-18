using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace School.Models
{
    public class Helpers
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public bool GradeAlreadyExists(GradeEnum gradeEnum, GradesSeparatorEnum gradesSeparatorEnum)
        {
            var gradeList = db.Grades.Where(a => a.GradeNum == gradeEnum && a.GradeLetter == gradesSeparatorEnum).ToList();
            if (gradeList.Count > 0)
            {
                return true;
            }
            return false;
        }

        public bool GradeSubjectAlreadyExist(GradeEnum gradeEnum, string subjectName)
        {
            var subjectExist = db.GradeSubjects.Where(s => s.GradeNumber == gradeEnum && s.SubjectName.ToLower() == subjectName.ToLower()).ToList();
            if (subjectExist.Count > 0)
            {
                return true;
            }
            return false;
        }
        public bool historyAlreadyExists(int studentId, GradeEnum gradeNumber, SubjectStatusEnum subjectStatusEnum, int gradeSubjectId)
        {
            var historyExist = db.StudentGradeSubjectHistories.Where(a => a.StudentId == studentId && a.StudentGrade == gradeNumber &&
                                            a.SubjectStatus == subjectStatusEnum && a.GradeSubectId == gradeSubjectId).ToList();
            if (historyExist.Count > 0)
            {
                return true;
            }
            return false;
        }

        public Student GetStudent(int studentId)
        {
            return db.Students.FirstOrDefault(a => a.Id == studentId);
        }

        public Teacher GetTeacher(int teacherId)
        {
            return db.Teachers.FirstOrDefault(a => a.Id == teacherId);
        }
        public List<Grade> GetGradesWithoutTeacher()
        {
            return db.Grades.Where(a => a.teacherId == 0).ToList();
        }
        public List<GradeSubject> GetSubjectWithoutTeacher()
        {
            return db.GradeSubjects.Where(a => a.teacherId == 0).ToList();
        }

        /// <summary>
        /// This adds the 
        /// </summary>
        /// <param name="gradeId"></param>
        /// <param name="studentId"></param> 
        /// <param name="subjectStatusEnum"></param>
        /// <param name="subjectsToAdd"></param>
        /// <returns></returns>
        public OperationStatus AddStudSubjectsToHistroy(int gradeId, int studentId, SubjectStatusEnum subjectStatusEnum, List<GradeSubject> subjectsToAdd)
        {
            try
            {
                foreach (var subject in subjectsToAdd)
                {
                    var gradeEnum = GetGradeEnum(gradeId);
                    var historyExist = historyAlreadyExists(studentId, gradeEnum, subjectStatusEnum, subject.Id);

                    if (!historyExist && !gradeEnum.Equals(GradeEnum.None) && gradeEnum.Equals(subject.GradeNumber))
                    {
                        var subjectHistory = new StudentGradeSubjectHistory()
                        {
                            StudentId = studentId,
                            GradeSubectId = subject.Id,
                            StudentGrade = gradeEnum,
                            SubjectGrade = subject.GradeNumber,
                        };
                        db.StudentGradeSubjectHistories.Add(subjectHistory);
                        db.SaveChanges();
                    }
                }
                return OperationStatus.Created;
            }
            catch (Exception ex)
            {
                return OperationStatus.Error;
            }
        }

        /// <summary>
        /// This updates the subjects in the subjects history by updating enums for a certain student
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="currentSubjectStatus"></param>
        /// <param name="newSubjectStatus"></param>
        /// <returns> </returns>

        /// <summary>
        /// Returns the grade Enum using the Identifier
        /// </summary>
        /// <param name="gradeId"></param>
        /// <returns>GradeEnum</returns>
        public GradeEnum GetGradeEnum(int gradeId)
        {
            return db.Grades.FirstOrDefault(a => a.Id == gradeId).GradeNum;
        }
        public List<Student> GetTeachersStudents(int teacherId)
        {
            var teacher = db.Teachers.FirstOrDefault(a => a.Id == teacherId);
            var grade = GetGradeByID(teacher.gradeId);
            return db.Students.Where(a => a.gradeId == grade.Id).ToList();
        }
        public Grade GetGradeByID(int gradeId)
        {
            return db.Grades.FirstOrDefault(a => a.Id == gradeId);
        }
        public GradeSubject GetSubjectByID(int subjectId)
        {
            return db.GradeSubjects.FirstOrDefault(a => a.Id == subjectId);
        }
        public OperationStatus UpdateGradeAndSubjectTeacher(int subjectId, int gradeId, int teacherId)
        {
            var grade = GetGradeByID(gradeId);
            var subject = GetSubjectByID(subjectId);
            if ((grade != null) && (subject != null))
            {
                grade.teacherId = teacherId;
                subject.teacherId = teacherId;
                db.SaveChanges();
                return OperationStatus.Updated;
            }
            return OperationStatus.Error;
        }
        public List<GradeSubject> GetGradeSubjects(int gradeId)
        {
            var grade = db.Grades.FirstOrDefault(a => a.Id == gradeId).GradeNum;
            return db.GradeSubjects.Where(a => a.GradeNumber == grade).ToList();
        }

        public List<Classroom> GetUnselectedClassroom()
        {
            return db.Classrooms.Where(a => a.gradeId == 0).ToList();
        }
        public Classroom GetClassroom(int classroomId)
        {
            return db.Classrooms.FirstOrDefault(a => a.Id == classroomId);
        }

        public List<Grade> GetGradesWithSpaces(int studentId)
        {
            var student = GetStudent(studentId);
            return db.Grades.Where(a => a.GradeNum == student.CurrentGrade && a.MaxNumOfStudsInClass > a.NumOfStudentsInClass).ToList();
        }

        public List<Student> UnAssignedStudents()
        {
            return db.Students.Where(a => a.gradeId == 0).ToList();
        }
        public List<Student> AssignedStudents()
        {
            return db.Students.Where(a => a.gradeId != 0).ToList();
        }
        public bool ClassNameExist(string className)
        {
            var classNam = db.Classrooms.FirstOrDefault(a => a.RoomName == className);
            if (classNam == null)
            {
                return false;
            }
            return (!string.IsNullOrEmpty(classNam.RoomName));
        }
    }
}