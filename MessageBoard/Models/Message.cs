using System;
using System.ComponentModel.DataAnnotations;

namespace MessageBoard.Models
{
  public class Message
  {
    public int MessageId { get; set; }
    public int GroupId { get; set; }
    public virtual Group Group { get; set; }

    [Required]
    [StringLength(250)]
    public string Body { get; set; }

    [Required]
    [StringLength(115)]
    public string Author { get; set; }

    [Required]
    public DateTime DatePosted { get; set; }
  }
}