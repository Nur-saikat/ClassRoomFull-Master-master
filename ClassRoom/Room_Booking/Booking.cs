﻿//using ClassRoom.DataCreate;

using ClassRoom.Models.DataCreate;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using Room = ClassRoom.Models.Room;

namespace classroombooking.DataCreate
{
    public class Booking
    {
        public int Id { get; set; }

        //[DataType(DataType.DateTime)]
        //public DateTime StartDate { get; set; }

        //[DataType(DataType.DateTime)]
        //public DateTime EndDate { get; set; }

        [Display(Name = "Class Time")]
        public int? SlodId { get; set; }
        public virtual Slod? Slods { get; set; }


        //[DataType(DataType.DateTime)]
        //public DateTime Finish { get; set; }

        [Display(Name = "Session")]
        public int? SessionId { get; set; }
        public virtual Session? Sessions { get; set; }

        [Display(Name = "Lecturer")]
        public int? LecturerId { get; set; }
        public virtual Lecturer? Lecturers { get; set; }
        [Display(Name = "Room")]
        public int? RoomId { get; set; }
        public virtual Room? Rooms { get; set; }
        [Display(Name = "Course")]
        public int? CourseId { get; set; }
        public virtual Course? Course { get; set; }

     
    }
}
