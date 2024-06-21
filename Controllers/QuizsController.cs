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
    public class QuizsController : ControllerBase
    {
        private readonly Draft15Context _context;

        public QuizsController(Draft15Context context)
        {
            _context = context;
        }

        // GET: api/Quizs
        [HttpGet("all-quiz")]
        public async Task<ActionResult<IEnumerable<Quiz>>> GetQuiz()
        {
          if (_context.Quiz == null)
          {
              return NotFound();
          }
            return await _context.Quiz.ToListAsync();
        }

        // GET: api/Quizs/5
        [HttpGet("details/{id}")]
        public async Task<ActionResult<Quiz>> GetQuiz(int id)
        {
          if (_context.Quiz == null)
          {
              return NotFound();
          }
            var quiz = await _context.Quiz.FindAsync(id);

            if (quiz == null)
            {
                return NotFound();
            }

            return quiz;
        }

        // PUT: api/Quizs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put-quiz/{id}")]
        public async Task<IActionResult> PutQuiz(int id, Quiz quiz)
        {
            if (id != quiz.quiz_id)
            {
                return BadRequest();
            }

            _context.Entry(quiz).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizExists(id))
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

        // POST: api/Quizs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post-quiz")]
        public async Task<ActionResult<Quiz>> PostQuiz(Quiz quiz)
        {
          if (_context.Quiz == null)
          {
              return Problem("Entity set 'Draft15Context.Quiz'  is null.");
          }
            _context.Quiz.Add(quiz);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetQuiz", new { id = quiz.quiz_id }, quiz);
        }

        // DELETE: api/Quizs/5
        [HttpDelete("delete-quiz-by-id/{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            if (_context.Quiz == null)
            {
                return NotFound();
            }
            var quiz = await _context.Quiz.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            _context.Quiz.Remove(quiz);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuizExists(int id)
        {
            return (_context.Quiz?.Any(e => e.quiz_id == id)).GetValueOrDefault();
        }
    }
}
