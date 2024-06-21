using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppData;
using Draft15.Data;

namespace Draft15.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserContentsController : ControllerBase
    {
        private readonly Draft15Context _context;

        public UserContentsController(Draft15Context context)
        {
            _context = context;
        }

        // GET: api/UserContents
        [HttpGet("get-all-users")]
        public async Task<ActionResult<IEnumerable<UserContent>>> GetUserContent()
        {
          if (_context.UserContent == null)
          {
              return NotFound();
          }
            return await _context.UserContent.ToListAsync();
        }

        // GET: api/UserContents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserContent>> GetUserContent(int id)
        {
          if (_context.UserContent == null)
          {
              return NotFound();
          }
            var userContent = await _context.UserContent.FindAsync(id);

            if (userContent == null)
            {
                return NotFound();
            }

            return userContent;
        }

        // PUT: api/UserContents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("change-user-content-status/{id}")]
        public async Task<IActionResult> PutUserContent(int id, UserContent userContent)
        {
            if (id != userContent.Id)
            {
                return BadRequest();
            }

            _context.Entry(userContent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserContentExists(id))
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

        // POST: api/UserContents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserContent>> PostUserContent(UserContent userContent)
        {
          if (_context.UserContent == null)
          {
              return Problem("Entity set 'Draft15Context.UserContent'  is null.");
          }
            _context.UserContent.Add(userContent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserContent", new { id = userContent.Id }, userContent);
        }

        // DELETE: api/UserContents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserContent(int id)
        {
            if (_context.UserContent == null)
            {
                return NotFound();
            }
            var userContent = await _context.UserContent.FindAsync(id);
            if (userContent == null)
            {
                return NotFound();
            }

            _context.UserContent.Remove(userContent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserContentExists(int id)
        {
            return (_context.UserContent?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
