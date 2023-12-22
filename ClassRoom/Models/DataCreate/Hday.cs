using System.ComponentModel.DataAnnotations;

namespace ClassRoom.Models.DataCreate
{
    public class Hday
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime Start_Date { get; set; }

        [DataType(DataType.Date)]
        public DateTime End_Date { get; set; }
    }
}
