using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace MessageBoard.Models
{
  public class MessageBoardContext : IdentityDbContext
  {
    public MessageBoardContext(DbContextOptions<MessageBoardContext> options)
      : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
      builder.Entity<Message>()
        .HasData(
          new Message { MessageId = 1, GroupId = 1, Body = "Matilda", Author = "Woolly Mammoth", DatePosted = System.DateTime.Now },
          new Message { MessageId = 2, GroupId = 1, Body = "Rexie", Author = "Dinosaur", DatePosted = System.DateTime.Now },
          new Message { MessageId = 3, GroupId = 1, Body = "Matilda", Author = "Dinosaur", DatePosted = System.DateTime.Now },
          new Message { MessageId = 4, GroupId = 1, Body = "Pip", Author = "Shark", DatePosted = System.DateTime.Now },
          new Message { MessageId = 5, GroupId = 1, Body = "Bartholomew", Author = "Dinosaur", DatePosted = new System.DateTime(2015, 12, 1, 9, 38, 58) }
        );

      builder.Entity<Group>()
      .HasData(
        new Group { GroupId = 1, GroupName = "Animals" }
      );
    }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Group> Groups { get; set; }
  }
}