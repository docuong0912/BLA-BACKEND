using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppData;
using Draft15.Data;
using Microsoft.VisualBasic.FileIO;

namespace Draft15.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly Draft15Context _context;

        public AssignmentsController(Draft15Context context)
        {
            _context = context;
        }

        // GET: api/Assignments
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Assignment>>> GetAllAssignment()
        {
          if (_context.Assignment == null)
          {
              return NotFound();
          }
            return await _context.Assignment.Where(a=>a.due_date>=DateTime.Now).ToListAsync();
        }

        [HttpGet("all-by-course-id")]
        public async Task<ActionResult<IEnumerable<Assignment>>> GetAssignmentByCourse(int course_id)
        {
            if (_context.Assignment == null)
            {
                return NotFound();
            }
            return await _context.Assignment.Where(a=>a.course_id == course_id && a.start_date<= DateTime.Now && DateTime.Now<=a.due_date).ToListAsync();
        }
        // GET: api/Assignments/5
        [HttpGet("details/{id}")]
        public async Task<ActionResult<Assignment>> GetAssignment(int id)
        {
          if (_context.Assignment == null)
          {
              return NotFound();
          }
            var assignment = await _context.Assignment.Include(a=>a.files).Include(a=>a.submissions).ThenInclude(s=>s.files).Where(a=>a.assignment_id==id).FirstOrDefaultAsync();

            if (assignment == null)
            {
                return NotFound();
            }

            return assignment;
        }

        // PUT: api/Assignments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update-assignment/{id}")]
        public async Task<IActionResult> PutAssignment(int id, Assignment assignment)
        {
            if (id != assignment.assignment_id)
            {
                return BadRequest();
            }

            _context.Entry(assignment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssignmentExists(id))
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

        // POST: api/Assignments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post-new-assignment")]
        public async Task<ActionResult<Assignment>> PostAssignment(Assignment assignment)
        {
          if (_context.Assignment == null)
          {
              return Problem("Entity set 'Draft15Context.Assignment'  is null.");
            }

            _context.Assignment.Add(assignment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAssignmentByCourse", new { id = assignment.assignment_id }, assignment);
        }

        // DELETE: api/Assignments/5
        [HttpDelete("delete-assignment/{id}")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            if (_context.Assignment == null)
            {
                return NotFound();
            }
            var assignment = await _context.Assignment.FindAsync(id);
            if (assignment == null)
            {
                return NotFound();
            }

            _context.Assignment.Remove(assignment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AssignmentExists(int id)
        {
            return (_context.Assignment?.Any(e => e.assignment_id == id)).GetValueOrDefault();
        }
    }
}
