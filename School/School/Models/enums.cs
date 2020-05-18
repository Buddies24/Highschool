using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School.Models
{
    public enum SubjectStatusEnum
    {
        NewToSchoolAndGrade = 0,
        Enrolled = 1,
        Failed = 2,
        Passed = 3
    }
    public enum GradeEnum
    {
        //None = Error = 0 (invalid grade)
        None = 0,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Eleven = 11,
        Twelve = 12
    }
    public enum GradesSeparatorEnum
    {
        A,
        B,
        C
    }
    public enum OperationStatus
    {
        Error = 0,
        Initializing = 1,
        Created = 2,
        Updated = 3,
        Deleted = 4
    }
    public enum ResponseStatus
    {
        None = 0,
        Error = 1,
        Success = 2

    }
}