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
    public class FilesController : ControllerBase
    {
        private readonly Draft15Context _context;

        public FilesController(Draft15Context context)
        {
            _context = context;
        }

        // GET: api/Files
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppData.File>>> GetFile()
        {
          if (_context.File == null)
          {
              return NotFound();
          }
            return await _context.File.ToListAsync();
        }

        // GET: api/Files/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppData.File>> GetFile(int id)
        {
          if (_context.File == null)
          {
              return NotFound();
          }
            var @file = await _context.File.FindAsync(id);

            if (@file == null)
            {
                return NotFound();
            }

            return @file;
        }

        // PUT: api/Files/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put-file/{id}")]
        public async Task<IActionResult> PutFile(int id, AppData.File @file)
        {
            if (id != @file.file_id)
            {
                return BadRequest();
            }

            _context.Entry(@file).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FileExists(id))
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

        // POST: api/Files
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("upload-file")]
        public async Task<ActionResult<AppData.File>> PostFile([FromForm]AppData.File file1,IFormFile file)
        {
          if (_context.File == null)
          {
              return Problem("Entity set 'Draft15Context.File'  is null.");
          }
            var filePath = Path.Combine("C:\\Users\\Asus\\Desktop\\thesis project\\bla\\public", file1.file_type, file.FileName);
            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            file1.file_path = filePath;
            _context.File.Add(file1);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFile", new { id = file1.file_id }, file1);
        }

        // DELETE: api/Files/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            if (_context.File == null)
            {
                return NotFound();
            }
            var @file = await _context.File.FindAsync(id);
            if (@file == null)
            {
                return NotFound();
            }

            _context.File.Remove(@file);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FileExists(int id)
        {
            return (_context.File?.Any(e => e.file_id == id)).GetValueOrDefault();
        }
    }
}
