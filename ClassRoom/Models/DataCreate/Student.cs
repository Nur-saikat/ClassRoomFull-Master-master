using ClassRoom.DataCreate;
using ClassRoom.Models.DataCreate;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace classroombooking.DataCreate
{
    public class Student
    {
        
        public int Id { get; set; }
        [Key]
        [Display(Name = "Student Id")]
        public int StudentId { get; set; }

        public string FirstName { get; set; } = null!;
        
        public string LastName { get; set; } = null!;

        public string FullName => $"{FirstName} {LastName}";
       
        public string Address { get; set; } = null!;
       

        public int Number { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; } = null!;
        [NotMapped]
        public DateTime createdDateTime { get; set; } = DateTime.Now;

        public int? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }

        public virtual ICollection<StudentCourse> StudentCourses { get; } = new List<StudentCourse>();


    }
}
