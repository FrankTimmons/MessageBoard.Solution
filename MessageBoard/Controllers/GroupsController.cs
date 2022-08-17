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
  // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [Route("api/[controller]")]
  [ApiController]
  public class GroupsController : ControllerBase
  {
    private readonly MessageBoardContext _db;

    public GroupsController(MessageBoardContext db)
    {
      _db = db;
    }

    // GET: api/Groups
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Group>>> GetGroups(string name)
    {
      var query = _db.Groups.AsQueryable();

      if (name != null)
      {
        query = query.Where(entry => entry.GroupName == name);
      }
      return await _db.Groups.ToListAsync();
    }

    // GET: api/Groups/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Group>> GetGroup(int id)
    {
      var @group = await _db.Groups.FindAsync(id);

      if (@group == null)
      {
        return NotFound();
      }

      return @group;
    }

    // PUT: api/Groups/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGroup(int id, Group @group)
    {
      if (id != @group.GroupId)
      {
        return BadRequest();
      }

      _db.Entry(@group).State = EntityState.Modified;

      try
      {
        await _db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!GroupExists(id))
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

    // POST: api/Groups
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Group>> PostGroup(Group @group)
    {
      _db.Groups.Add(@group);
      await _db.SaveChangesAsync();

      return CreatedAtAction("GetGroup", new { id = @group.GroupId }, @group);
    }

    // DELETE: api/Groups/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(int id)
    {
      var @group = await _db.Groups.FindAsync(id);
      if (@group == null)
      {
        return NotFound();
      }

      _db.Groups.Remove(@group);
      await _db.SaveChangesAsync();

      return NoContent();
    }

    private bool GroupExists(int id)
    {
      return _db.Groups.Any(e => e.GroupId == id);
    }
  }
}
