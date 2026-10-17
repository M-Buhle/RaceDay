using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Models;
namespace RaceDayAPI.Data
{
   public class RaceDayDbContext : DbContext
   {
       public RaceDayDbContext(DbContextOptions<RaceDayDbContext> options) : base(options) { }
       public DbSet<User> Users { get; set; }
       public DbSet<Event> Events { get; set; }
       public DbSet<Category> Categories { get; set; }
       public DbSet<EventEnrolment> EventEnrolments { get; set; }
       public DbSet<Result> Results { get; set; }
       public DbSet<WeatherLog> WeatherLogs { get; set; }
       protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
           base.OnModelCreating(modelBuilder);
           // Unique constraints
           modelBuilder.Entity<User>()
               .HasIndex(u => u.Email)
               .IsUnique();
           modelBuilder.Entity<User>()
               .HasIndex(u => u.IDNumber)
               .IsUnique();
           modelBuilder.Entity<EventEnrolment>()
               .HasIndex(e => new { e.EventID, e.ParticipantID })
               .IsUnique();
           modelBuilder.Entity<Result>()
               .HasIndex(r => r.EnrolmentID)
               .IsUnique();
           // Seed data
           modelBuilder.Entity<User>().HasData(
               new User { UserID = 1, Email = "john.durban@gmail.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"), FirstName = "John", LastName = "Durban", IDNumber = "8001015800087", PhoneNumber = "0821234567", DateOfBirth = new DateTime(1980, 1, 1), Gender = "Male", Role = "Organiser", IsActive = true, CreatedAt = DateTime.UtcNow },
               new User { UserID = 2, Email = "sarah.capetown@gmail.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"), FirstName = "Sarah", LastName = "Capetown", IDNumber = "8505155800088", PhoneNumber = "0837654321", DateOfBirth = new DateTime(1985, 5, 15), Gender = "Female", Role = "Organiser", IsActive = true, CreatedAt = DateTime.UtcNow },
               new User { UserID = 3, Email = "mike.soweto@gmail.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"), FirstName = "Mike", LastName = "Soweto", IDNumber = "9003035800089", PhoneNumber = "0829876543", DateOfBirth = new DateTime(1990, 3, 3), Gender = "Male", Role = "Participant", IsActive = true, CreatedAt = DateTime.UtcNow },
               new User { UserID = 4, Email = "lisa.pmb@gmail.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"), FirstName = "Lisa", LastName = "Pietermaritzburg", IDNumber = "9507115800090", PhoneNumber = "0845678901", DateOfBirth = new DateTime(1995, 7, 11), Gender = "Female", Role = "Participant", IsActive = true, CreatedAt = DateTime.UtcNow }
           );
       }
   }
}