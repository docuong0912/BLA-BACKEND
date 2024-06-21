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
    public class SubmissionsController : ControllerBase
    {
        private readonly Draft15Context _context;

        public SubmissionsController(Draft15Context context)
        {
            _context = context;
        }

        // GET: api/Submissions
        [HttpGet]
        public async Task<ActionResult<Submission>> GetSubmission(int user_id,int assignment_id)
        {
          if (_context.Submission == null)
          {
              return NotFound();
          }
            var submission =  await _context.Submission.Include(s=>s.files).Where(s=>s.user_id == user_id).Where(s=>s.assignment_id == assignment_id).OrderBy(s=>s.submission_id).FirstOrDefaultAsync();
            if(submission == null)
            {
                return NotFound();
            }
            
            return submission;
        }
        
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Submission>>> FindAll()
        {
            if (_context.Submission == null)
            {
                return NotFound();
            }
            var submission = await _context.Submission.ToListAsync();
            if (submission == null)
            {
                return NotFound();
            }
            return submission;
        }

        // GET: api/Submissions/5
        [HttpGet("details/{id}")]
        public async Task<ActionResult<Submission>> GetSubmission(int id)
        {
          if (_context.Submission == null)
          {
              return NotFound();
          }
            var submission = await _context.Submission.FindAsync(id);

            if (submission == null)
            {
                return NotFound();
            }

            return submission;
        }

        // PUT: api/Submissions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update-submission/{id}")]
        public async Task<IActionResult> PutSubmission(int id, Submission submission)
        {
            if (id != submission.submission_id)
            {
                return BadRequest();
            }

            _context.Entry(submission).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubmissionExists(id))
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
        [HttpPut("grading/{id}")]
        public async Task<IActionResult> Grading(int id, int grade)
        {
            var submission = await _context.Submission.FindAsync(id);
            submission.grade = grade;

            _context.Entry(submission).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubmissionExists(id))
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
        // POST: api/Submissions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post-submission")]
        public async Task<ActionResult<Submission>> PostSubmission(Submission submission)
        {
          if (_context.Submission == null)
          {
              return Problem("Entity set 'Draft15Context.Submission'  is null.");
          }
            
            _context.Submission.Add(submission);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubmission", new { id = submission.submission_id }, submission);
        }

        // DELETE: api/Submissions/5
        [HttpDelete("delete-submission/{id}")]
        public async Task<IActionResult> DeleteSubmission(int id)
        {
            if (_context.Submission == null)
            {
                return NotFound();
            }
            var submission = await _context.Submission.FindAsync(id);
            if (submission == null)
            {
                return NotFound();
            }

            _context.Submission.Remove(submission);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubmissionExists(int id)
        {
            return (_context.Submission?.Any(e => e.submission_id == id)).GetValueOrDefault();
        }
    }
}
