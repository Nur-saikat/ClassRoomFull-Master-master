using System.ComponentModel.DataAnnotations;

namespace ClassRoom.Models.DataCreate
{
    public class Slod
    {

        public int Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }
    }
}
