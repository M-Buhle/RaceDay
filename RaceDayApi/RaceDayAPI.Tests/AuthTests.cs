using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;
using Xunit;
namespace RaceDayAPI.Tests
{
   public class AuthTests
   {
       private RaceDayDbContext GetInMemoryContext()
       {
           var options = new DbContextOptionsBuilder<RaceDayDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
           return new RaceDayDbContext(options);
       }
       [Fact]
       public async Task Register_NewUser_Success()
       {
           // Arrange
           using var context = GetInMemoryContext();
           var user = new User
           {
               Email = "test@test.com",
               PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
               FirstName = "Test",
               LastName = "User",
               IDNumber = "1234567890123",
               Role = "Participant"
           };
           // Act
           context.Users.Add(user);
           await context.SaveChangesAsync();
           // Assert
           var savedUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "test@test.com");
           Assert.NotNull(savedUser);
           Assert.Equal("Participant", savedUser.Role);
           Assert.True(BCrypt.Net.BCrypt.Verify("Password123!", savedUser.PasswordHash));
       }
       [Fact]
       public void Password_IsHashed()
       {
           // Arrange
           var password = "MyPassword123";
           // Act
           var hash = BCrypt.Net.BCrypt.HashPassword(password);
           // Assert
           Assert.NotEqual(password, hash);
           Assert.True(BCrypt.Net.BCrypt.Verify(password, hash));
       }
       [Fact]
       public async Task Event_OrganiserCanCreate_Success()
       {
           // Arrange
           using var context = GetInMemoryContext();
           var organiser = new User
           {
               Email = "org@test.com",
               PasswordHash = "hash",
               FirstName = "Org",
               LastName = "User",
               IDNumber = "1111111111111",
               Role = "Organiser"
           };
           context.Users.Add(organiser);
           await context.SaveChangesAsync();
           var ev = new Event
           {
               OrganiserID = organiser.UserID,
               EventName = "Test Run",
               EventType = "Running",
               EventDate = DateTime.UtcNow.AddDays(30)
           };
           // Act
           context.Events.Add(ev);
           await context.SaveChangesAsync();
           // Assert
           Assert.Equal(1, await context.Events.CountAsync());
       }
       [Fact]
       public async Task Enrolment_ParticipantCanEnrol_Success()
       {
           // Arrange
           using var context = GetInMemoryContext();
           var participant = new User { Email = "p@test.com", PasswordHash = "h", FirstName = "P", LastName = "T", IDNumber = "2222222222222", Role = "Participant" };
           var organiser = new User { Email = "o@test.com", PasswordHash = "h", FirstName = "O", LastName = "T", IDNumber = "3333333333333", Role = "Organiser" };
           context.Users.AddRange(participant, organiser);
           await context.SaveChangesAsync();
           var ev = new Event { OrganiserID = organiser.UserID, EventName = "Run", EventType = "Running", EventDate = DateTime.UtcNow.AddDays(30) };
           context.Events.Add(ev);
           await context.SaveChangesAsync();
           var cat = new Category { EventID = ev.EventID, CategoryName = "10km", Distance = 10 };
           context.Categories.Add(cat);
           await context.SaveChangesAsync();
           var enrolment = new EventEnrolment
           {
               EventID = ev.EventID,
               ParticipantID = participant.UserID,
               CategoryID = cat.CategoryID,
               RegistrationNumber = "REG-001"
           };
           // Act
           context.EventEnrolments.Add(enrolment);
           await context.SaveChangesAsync();
           // Assert
           Assert.Equal(1, await context.EventEnrolments.CountAsync());
           Assert.NotNull(await context.EventEnrolments.FirstOrDefaultAsync(e => e.ParticipantID == participant.UserID));
       }
   }
}