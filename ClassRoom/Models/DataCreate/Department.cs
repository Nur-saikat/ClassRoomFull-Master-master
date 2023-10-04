using ClassRoom.Models.DataCreate;
using classroombooking.DataCreate;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassRoom.DataCreate
{
    public class Department
    {
        public int Id { get; set; }
        
        public string Name { get; set; }


        [NotMapped]
        public DateTime createdDateTime { get; set; } = DateTime.Now;

        public virtual ICollection<Student> Students { get; } = new List<Student>();
    }
}
