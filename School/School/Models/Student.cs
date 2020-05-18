using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        #region Personal information

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }


        [Display(Name = "Middle Name")]
        public string MidName { get; set; }

        [Required]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Display(Name = "ID Number")]
        [Required]
        [MaxLength(13)]
        [MinLength(13)]
        [Range(13, Int64.MaxValue, ErrorMessage = "ID Number should not contain charecters and must be 13 digits")]
        public string idNum { get; set; }


        [Required]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(10)]
        [MinLength(10)]
        [Range(10, Int64.MaxValue, ErrorMessage = "Phone number should not contain charecters and must be 10 digits")]
        [Display(Name = "Phone Number")]
        public string phone { get; set; }

        [Display(Name = "Previous School Report")]
        public byte[] PrevSchoolReport { get; set; }

        #endregion

        #region Address

        [Display(Name = "Country")]
        public string country { get; set; }

        [Display(Name = "Street Number")]
        public string street_number { get; set; }


        [Display(Name = "Street Name")]
        public string route { get; set; }

        [Display(Name = "Province")]
        public string administrative_area_level_1 { get; set; }


        [Display(Name = "City")]
        public string locality { get; set; }

        [Display(Name = "Code")]
        public string postal_code { get; set; }
        public string adress { get; set; }

        #endregion

        #region School Information

        [Display(Name = "Current Grade")]
        public GradeEnum CurrentGrade { get; set; }


        [Display(Name = "Grade Id")]
        public int gradeId { get; set; }

        [Display(Name = "Current Grade")]
        public string gradeName { get; set; }

        #endregion

        public string dateRegistered { get; set; }

        public string GetAddress()
        {
            string ad = (country + " " + street_number + " " + route + " " + administrative_area_level_1 + " " + locality + " " + postal_code);
            return ad;
        }

    }

}