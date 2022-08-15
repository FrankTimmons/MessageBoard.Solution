using Microsoft.EntityFrameworkCore;

namespace MessageBoard.Models
{
  public class MessageBoardContext : DbContext
  {
    public MessageBoardContext(DbContextOptions<MessageBoardContext> options)
      : base(options)
    {
      
    }
  protected override void OnModelCreating(ModelBuilder builder)
  {
    builder.Entity<Message>()
      .HasData(
        new Message { MessageId = 1, Body = "Matilda", Author = "Woolly Mammoth", DatePosted = System.DateTime.Now },
        new Message { MessageId = 2, Body = "Rexie", Author = "Dinosaur", DatePosted = System.DateTime.Now },
        new Message { MessageId = 3, Body = "Matilda", Author = "Dinosaur", DatePosted = System.DateTime.Now },
        new Message { MessageId = 4, Body = "Pip", Author = "Shark", DatePosted = System.DateTime.Now },
        new Message { MessageId = 5, Body = "Bartholomew", Author = "Dinosaur", DatePosted = new System.DateTime(2015, 12, 1, 9, 38, 58) }
      );
  }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Group> Groups { get; set; }
  }
}