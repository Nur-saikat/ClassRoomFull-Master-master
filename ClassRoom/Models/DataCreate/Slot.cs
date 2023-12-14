using ClassRoom.Models.Room_Booking;
using System.ComponentModel.DataAnnotations;

namespace ClassRoom.Models.DataCreate
{
    public class Slot
    {

        public int Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }

        public virtual ICollection<Booking> Bookings { get; } = new List<Booking>();
    }
}
