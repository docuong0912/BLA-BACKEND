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
    public class QuizOptionsController : ControllerBase
    {
        private readonly Draft15Context _context;

        public QuizOptionsController(Draft15Context context)
        {
            _context = context;
        }

        // GET: api/QuizOptions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizOption>>> GetQuizOption()
        {
          if (_context.QuizOption == null)
          {
              return NotFound();
          }
            return await _context.QuizOption.ToListAsync();
        }

        // GET: api/QuizOptions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuizOption>> GetQuizOption(int id)
        {
          if (_context.QuizOption == null)
          {
              return NotFound();
          }
            var quizOption = await _context.QuizOption.FindAsync(id);

            if (quizOption == null)
            {
                return NotFound();
            }

            return quizOption;
        }

        // PUT: api/QuizOptions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("change-option-text/{id}")]
        public async Task<ActionResult<QuizOption>> PutQuizOption(int id, string text)
        {
            var option = await _context.QuizOption.FindAsync(id);
            option.option_text = text;
            _context.Entry(option).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizOptionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(option);
        }

        // POST: api/QuizOptions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post-new-option")]
        public async Task<ActionResult<QuizOption>> PostQuizOption(QuizOption quizOption)
        {
          if (_context.QuizOption == null)
          {
              return Problem("Entity set 'Draft15Context.QuizOption'  is null.");
          }
            _context.QuizOption.Add(quizOption);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuizOption", new { id = quizOption.option_id }, quizOption);
        }

        // DELETE: api/QuizOptions/5
        [HttpDelete("delete-option/{id}")]
        public async Task<IActionResult> DeleteQuizOption(int id)
        {
            if (_context.QuizOption == null)
            {
                return NotFound();
            }
            var quizOption = await _context.QuizOption.FindAsync(id);
            if (quizOption == null)
            {
                return NotFound();
            }

            _context.QuizOption.Remove(quizOption);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuizOptionExists(int id)
        {
            return (_context.QuizOption?.Any(e => e.option_id == id)).GetValueOrDefault();
        }
    }
}
