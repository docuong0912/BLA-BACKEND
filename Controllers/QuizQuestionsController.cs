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
    public class QuizQuestionsController : ControllerBase
    {
        private readonly Draft15Context _context;

        public QuizQuestionsController(Draft15Context context)
        {
            _context = context;
        }

        // GET: api/QuizQuestions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizQuestion>>> GetQuizQuestion()
        {
          if (_context.QuizQuestion == null)
          {
              return NotFound();
          }
            return await _context.QuizQuestion.ToListAsync();
        }

        // GET: api/QuizQuestions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuizQuestion>> GetQuizQuestion(int id)
        {
          if (_context.QuizQuestion == null)
          {
              return NotFound();
          }
            var quizQuestion = await _context.QuizQuestion.FindAsync(id);

            if (quizQuestion == null)
            {
                return NotFound();
            }

            return quizQuestion;
        }

        // PUT: api/QuizQuestions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("change-question-content/{id}")]
        public async Task<ActionResult<QuizQuestion>> PutQuizQuestion(int id, string content)
        {
            var question = await _context.QuizQuestion.FindAsync(id);
            question.quesion_content = content;

            _context.Entry(question).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizQuestionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(question);
        }

        // POST: api/QuizQuestions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post-new-question")]
        public async Task<ActionResult<QuizQuestion>> PostQuizQuestion(QuizQuestion quizQuestion)
        {
          if (_context.QuizQuestion == null)
          {
              return Problem("Entity set 'Draft15Context.QuizQuestion'  is null.");
          }
            _context.QuizQuestion.Add(quizQuestion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuizQuestion", new { id = quizQuestion.question_id }, quizQuestion);
        }

        // DELETE: api/QuizQuestions/5
        [HttpDelete("delete-question/{id}")]
        public async Task<IActionResult> DeleteQuizQuestion(int id)
        {
            if (_context.QuizQuestion == null)
            {
                return NotFound();
            }
            var quizQuestion = await _context.QuizQuestion.FindAsync(id);
            if (quizQuestion == null)
            {
                return NotFound();
            }

            _context.QuizQuestion.Remove(quizQuestion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuizQuestionExists(int id)
        {
            return (_context.QuizQuestion?.Any(e => e.question_id == id)).GetValueOrDefault();
        }
    }
}
