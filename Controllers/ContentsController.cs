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
    public class ContentsController : ControllerBase
    {
        private readonly Draft15Context _context;

        public ContentsController(Draft15Context context)
        {
            _context = context;
        }

        // GET: api/Contents
        [HttpGet("get-content-by-course-id")]
        public async Task<ActionResult<IEnumerable<Content>>> GetContentByCourse(int course_id)
        {
          if (_context.Content == null)
          {
              return NotFound();
          }
            return await _context.Content.Where(c=>c.course_id == course_id).Where(c=>c.start_date<=DateTime.Today && c.end_date>=DateTime.Today).Include(c=>c.prerequisite).Include(c=>c.files).Include(c=>c.UserContents).ToListAsync();
        }
        [HttpGet("get-content-by-user-id")]
        public async Task<ActionResult<IEnumerable<Content>>> GetContentByUser(int user_id,int course_id)
        {
            if (_context.Content == null)
            {
                return NotFound();
            }
            return await _context.Content.Where(c=>c.course_id == course_id).Where(c=>c.UserContents.Any(uc=>uc.user_id==user_id)).Where(c=>c.start_date<= DateTime.Today && DateTime.Today <= c.end_date).Include(c => c.prerequisite).Include(c => c.files).Include(c=>c.UserContents).ToListAsync();
        }
        // GET: api/Contents/5
        [HttpGet("details/{id}")]
        public async Task<ActionResult<Content>> GetContent(int id)
        {
          if (_context.Content == null)
          {
              return NotFound();
          }
            var content = await _context.Content.Include(c=>c.files).Include(c=>c.quizzes).ThenInclude(q=>q.attempts).Include(q=>q.quizzes).ThenInclude(q=>q.questions).ThenInclude(q=>q.options).Include(c=>c.UserContents).Include(c=>c.prerequisite).ThenInclude(p=>p.UserContents).Where(c=>c.content_id==id).FirstOrDefaultAsync();

            if (content == null)
            {
                return NotFound();
            }

            return content;
        }

        // PUT: api/Contents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put-content/{id}")]
        public async Task<IActionResult> PutContent(int id, Content content)
        {
            if (id != content.content_id)
            {
                return BadRequest();
            }
            var existingContent = await _context.Content.FindAsync(id);
            existingContent.UserContents = content.UserContents;
            

            try
            {
                _context.UpdateRange(existingContent);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContentExists(id))
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

        // POST: api/Contents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post-content")]
        [RequestSizeLimit(512*1024*1024)]
        public async Task<ActionResult<Content>> PostContent([FromForm]Content content,IFormFile file = null)
        {
          if (_context.Content == null)
          {
              return Problem("Entity set 'Draft15Context.Content'  is null.");
          }
            if (file != null)
            {
                foreach (var fp in content.files)
                {
                    await using var stream = new FileStream(fp.file_path, FileMode.Create);
                    await file.CopyToAsync(stream);
                }
            }
           

            
            _context.Content.Add(content);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContent", new { id = content.content_id }, content);
        }

        // DELETE: api/Contents/5
        [HttpDelete("delete-content-by-id/{id}")]
        public async Task<IActionResult> DeleteContent(int id)
        {
            if (_context.Content == null)
            {
                return NotFound();
            }
            var content = await _context.Content.FindAsync(id);
            if (content == null)
            {
                return NotFound();
            }

            _context.Content.Remove(content);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContentExists(int id)
        {
            return (_context.Content?.Any(e => e.content_id == id)).GetValueOrDefault();
        }
    }
}
