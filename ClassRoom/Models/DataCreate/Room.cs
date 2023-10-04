using classroombooking.DataCreate;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassRoom.Models.DataCreate
{
    public class Room
    {


        public int Id { get; set; }
        public string Name { get; set; }
        //public DateTime StartTime { get; set; }
        //public TimeSpan Duration { get; set; }
        //public DateTime EndTime { get { return StartTime + Duration; } }
        public string Type { get; set; }


       [NotMapped]
        public DateTime createdDateTime { get; set; } = DateTime.Now;


        public virtual ICollection<Booking> Bookings { get; } = new List<Booking>();

    }
}
