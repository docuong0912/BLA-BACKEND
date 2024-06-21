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
    public class QuizResponsesController : ControllerBase
    {
        private readonly Draft15Context _context;

        public QuizResponsesController(Draft15Context context)
        {
            _context = context;
        }

        // GET: api/QuizResponses
        [HttpGet("get-all-responses")]
        public async Task<ActionResult<IEnumerable<QuizResponse>>> GetQuizResponse()
        {
          if (_context.QuizResponse == null)
          {
              return NotFound();
          }
            return await _context.QuizResponse.ToListAsync();
        }
        [HttpGet("get-all-responses-by-attempt")]
        public async Task<ActionResult<IEnumerable<QuizResponse>>> GetQuizResponseByAttempt(int attemp_id)
        {
            if (_context.QuizResponse == null)
            {
                return NotFound();
            }
            return await _context.QuizResponse.Where(r=>r.attempt_id == attemp_id).ToListAsync();
        }

        // GET: api/QuizResponses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuizResponse>> GetQuizResponse(int id)
        {
          if (_context.QuizResponse == null)
          {
              return NotFound();
          }
            var quizResponse = await _context.QuizResponse.FindAsync(id);

            if (quizResponse == null)
            {
                return NotFound();
            }

            return quizResponse;
        }

        //// PUT: api/QuizResponses/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutQuizResponse(int id, QuizResponse quizResponse)
        //{
        //    if (id != quizResponse.response_id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(quizResponse).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!QuizResponseExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/QuizResponses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("submit-response")]
        public async Task<ActionResult<QuizResponse>> PostQuizResponse(QuizResponse quizResponse)
        {
          if (_context.QuizResponse == null)
          {
              return Problem("Entity set 'Draft15Context.QuizResponse'  is null.");
          }
            _context.QuizResponse.Add(quizResponse);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuizResponse", new { id = quizResponse.response_id }, quizResponse);
        }

        //// DELETE: api/QuizResponses/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteQuizResponse(int id)
        //{
        //    if (_context.QuizResponse == null)
        //    {
        //        return NotFound();
        //    }
        //    var quizResponse = await _context.QuizResponse.FindAsync(id);
        //    if (quizResponse == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.QuizResponse.Remove(quizResponse);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool QuizResponseExists(int id)
        {
            return (_context.QuizResponse?.Any(e => e.response_id == id)).GetValueOrDefault();
        }
    }
}
