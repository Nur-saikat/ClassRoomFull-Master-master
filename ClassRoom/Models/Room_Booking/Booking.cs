using ClassRoom.Models.DataCreate;
using classroombooking.DataCreate;
using System.ComponentModel.DataAnnotations;

namespace ClassRoom.Models.Room_Booking
{
    public class Booking
    {
            public int Id { get; set; }

            [Display(Name = "Class Time")]
            public int? SlotId { get; set; }
            public virtual Slot? Slots { get; set; }

            [DataType(DataType.Date)]
            public DateTime? Class_Date { get; set; } 

            [Display(Name = "Room")]
            public int? RoomId { get; set; }
            public virtual Room? Rooms { get; set; }

            [Display(Name = "Routine")]
            public int? RoutineId { get; set; }
            public virtual Routine? Routines { get; set; }

        
    }


}
