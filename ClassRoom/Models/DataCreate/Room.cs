using ClassRoom.Models.Room_Booking;
using classroombooking.DataCreate;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassRoom.Models.DataCreate
{
    public class Room
    {


        public int Id { get; set; }
        public string Name { get; set; }
      
        public string Type { get; set; }


       [NotMapped]
        public DateTime createdDateTime { get; set; } = DateTime.Now;
        public virtual ICollection<Booking> Bookings { get; } = new List<Booking>();

    }
    public enum Type
    {
        Class,
        Lab,
        Exam
    }
}
