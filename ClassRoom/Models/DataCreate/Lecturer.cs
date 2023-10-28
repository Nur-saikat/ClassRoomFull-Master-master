using ClassRoom.DataCreate;
using classroombooking.DataCreate;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xamarin.Essentials;

namespace ClassRoom.Models.DataCreate
{
    public class Lecturer
    {

        public int Id { get; set; }
        [Display(Name = "Lecturer ID")]
        public int LecturerId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;
        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";
        
        public int Number { get; set; }
        
    
        [NotMapped]
        public DateTime createdDateTime { get; set; } = DateTime.Now;

        public virtual ICollection<Booking> Bookings { get; } = new List<Booking>();
        public virtual ICollection<LecturerCourse> LecturerCourse { get; } = new List<LecturerCourse>();
       


    }
}
