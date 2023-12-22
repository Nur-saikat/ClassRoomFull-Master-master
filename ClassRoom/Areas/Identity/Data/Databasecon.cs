using ClassRoom.DataCreate;
using ClassRoom.Models.DataCreate;
using ClassRoom.Models.Room_Booking;
using classroombooking.DataCreate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClassRoom.Areas.Identity.Data;

public class Databasecon : IdentityDbContext<IdentityUser>
{
    public Databasecon(DbContextOptions<Databasecon> options)
        : base(options)
    {
    }
    public DbSet<Booking> Bookings { get; set; }

    public DbSet<Routine> Routines { get; set; }

    public DbSet<Room> Rooms { get; set; }

    public DbSet<Lecturer> Lecturers { get; set; }

    public DbSet<Course> Courses { get; set; }

    public DbSet<Department> Departments { get; set; }

    public DbSet<LecturerCourse> LecturerCourses { get; set; }

    public DbSet<StudentCourse> StudentCourse { get; set; }

    public DbSet<Slot> Slots { get; set; }

    public DbSet<Hday> Hdays { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }


    public DbSet<classroombooking.DataCreate.Student> Student { get; set; } = default!;


    public DbSet<ClassRoom.Models.DataCreate.Session> Session { get; set; } = default!;
}
