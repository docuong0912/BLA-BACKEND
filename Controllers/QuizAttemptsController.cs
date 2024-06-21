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
    public class QuizAttemptsController : ControllerBase
    {
        private readonly Draft15Context _context;

        public QuizAttemptsController(Draft15Context context)
        {
            _context = context;
        }

        // GET: api/QuizAttempts
        [HttpGet("all-attempt")]
        public async Task<ActionResult<IEnumerable<QuizAttempt>>> GetQuizAttempt()
        {
          if (_context.QuizAttempt == null)
          {
              return NotFound();
          }
            return await _context.QuizAttempt.Include(q=>q.responses).ThenInclude(r=>r.choosen).ToListAsync();
        }
        [HttpGet("all-attempt-by-quiz-id")]
        public async Task<ActionResult<IEnumerable<QuizAttempt>>> GetQuizAttemptByQuizId(int quiz_id)
        {
            if (_context.QuizAttempt == null)
            {
                return NotFound();
            }
            return await _context.QuizAttempt.Where(q=>q.quiz_id == quiz_id).Include(q => q.responses).ThenInclude(r => r.choosen).ToListAsync();
        }
        // GET: api/QuizAttempts/5
        [HttpGet("details/{id}")]
        public async Task<ActionResult<QuizAttempt>> GetQuizAttempt(int id)
        {
          if (_context.QuizAttempt == null)
          {
              return NotFound();
          }
            var quizAttempt = await _context.QuizAttempt.FindAsync(id);

            if (quizAttempt == null)
            {
                return NotFound();
            }

            return quizAttempt;
        }

        // PUT: api/QuizAttempts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update-score/{id}")]
        public async Task<IActionResult> PutQuizAttempt(int id, int score)
        {
            var quizAttempt = await _context.QuizAttempt.FindAsync(id);
            if (quizAttempt == null) return BadRequest();
            quizAttempt.score = score;
            _context.Entry(quizAttempt).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizAttemptExists(id))
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

        // POST: api/QuizAttempts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("submit-quiz-answer")]
        public async Task<ActionResult<QuizAttempt>> PostQuizAttempt(QuizAttempt quizAttempt)
        {
          if (_context.QuizAttempt == null)
          {
              return Problem("Entity set 'Draft15Context.QuizAttempt'  is null.");
          }
            _context.QuizAttempt.Add(quizAttempt);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuizAttempt", new { id = quizAttempt.attempt_id }, quizAttempt);
        }

        // DELETE: api/QuizAttempts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuizAttempt(int id)
        {
            if (_context.QuizAttempt == null)
            {
                return NotFound();
            }
            var quizAttempt = await _context.QuizAttempt.FindAsync(id);
            if (quizAttempt == null)
            {
                return NotFound();
            }

            _context.QuizAttempt.Remove(quizAttempt);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuizAttemptExists(int id)
        {
            return (_context.QuizAttempt?.Any(e => e.attempt_id == id)).GetValueOrDefault();
        }
    }
}
