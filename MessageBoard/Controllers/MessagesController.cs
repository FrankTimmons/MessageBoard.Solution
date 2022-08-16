using MessageBoard.Models.DTO.Responses;
using MessageBoard.Models.DTO.Requests;
using MessageBoard.Models;
using MessageBoard.Configuration;
using System;
using System.Threading.Tasks;
using System.Text;
using System.Security.Claims;
using System.Linq;
using System.IdentityModel;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using Microsoft.IdentityModel;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace MessageBoard.Controllers
{
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [Route("api/[controller]")]
  [ApiController]
  public class MessagesController : ControllerBase
  {
    private readonly MessageBoardContext _db;

    public MessagesController(MessageBoardContext db)
    {
      _db = db;
    }

    // GET: api/Messages
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Message>>> GetMessages(string author, string body, DateTime dateRangeStart, DateTime dateRangeEnd, string groupName)
    {
      var query = _db.Messages.AsQueryable();

      if (groupName != null)
      {
        Group thisGroup = _db.Groups.FirstOrDefault(group => group.GroupName == groupName);
        query = query.Where(entry => entry.GroupId == thisGroup.GroupId);
      }

      if (author != null)
      {
        query = query.Where(entry => entry.Author == author);
      }

      if (body != null)
      {
        query = query.Where(entry => entry.Body == body);
      }

      if (dateRangeStart != new DateTime() && dateRangeEnd != new DateTime())
      {
        query = query.Where(entry => (dateRangeStart <= entry.DatePosted && entry.DatePosted < dateRangeEnd));
      }

      return await query.ToListAsync();
    }

    // GET: api/Messages/5
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<Message>> GetMessage(int id)
    {
      var message = await _db.Messages.FindAsync(id);
      // FirstOrDefault( message => message.MessageId = id)

      if (message == null)
      {
        return NotFound();
      }

      return message;
    }

    // PUT: api/Messages/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMessage(int id, string user_name, Message message)
    {

      if (id != message.MessageId || message.Author != user_name || user_name == null)
      {
        return BadRequest();
      }

      _db.Entry(message).State = EntityState.Modified;

      try
      {
        await _db.SaveChangesAsync();
      }

      catch (DbUpdateConcurrencyException)
      {
        if (!MessageExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }

    // POST: api/Messages
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Message>> PostMessage(Message message)
    {
      _db.Messages.Add(message);
      await _db.SaveChangesAsync();

      return CreatedAtAction(nameof(GetMessage), new { id = message.MessageId }, message);
    }

    // DELETE: api/Messages/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMessage(int id)
    {
      var message = await _db.Messages.FindAsync(id);
      if (message == null)
      {
        return NotFound();
      }

      _db.Messages.Remove(message);
      await _db.SaveChangesAsync();

      return NoContent();
    }

    private bool MessageExists(int id)
    {
      return _db.Messages.Any(e => e.MessageId == id);
    }
  }
}
