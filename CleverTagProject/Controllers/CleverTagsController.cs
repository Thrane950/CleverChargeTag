using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CleverTagProject.DataContext;
using CleverTagProject.Models;
using Newtonsoft.Json;

namespace CleverTagProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CleverTagsController : ControllerBase
    {
        private readonly CleverTagContext _context;

        public CleverTagsController(CleverTagContext context)
        {
            _context = context;
        }

        //Get all Tags
        // GET: api/CleverTags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CleverTag>>> GetCleverTags()
        {
            return await _context.CleverTags.ToListAsync();
        }


        //Verify Tag
        // GET: api/CleverTags/{id}
        [HttpGet("Verify/{id}")]
        public async Task<ActionResult<CleverTag>> GetCleverTag(int id)
        {
            var cleverTag = await _context.CleverTags.FindAsync(id);

            if (cleverTag == null)
            {
                return NotFound();
            }

            
            if (cleverTag.Isblocked == false) 
            {
                var message = $"CleverTag with ID {id} is active and ready for use";
                return Ok(new { Message = message, CleverTag = cleverTag });

            }
            else 
            {
                var message = $"CleverTag with ID {id} is blocked";
                return Ok(new { Message = message, CleverTag = cleverTag });

            }


        }

        //Update Clever Tag
        // PUT: api/CleverTags/{id}
        [HttpPut("UpdateTagInfo/{id}")]
        public async Task<ActionResult<CleverTag>> UpdateCleverTag(int id, CleverTag cleverTag)
        {
            if (id != cleverTag.Id)
            {
                return BadRequest();
            }

            _context.Entry(cleverTag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CleverTagExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var existingCleverTag = await _context.CleverTags.FindAsync(id);

            if (existingCleverTag == null)
            {
                return NotFound();
            }

            // Retrieve the updated CleverTag from the database
            var updatedCleverTag = await _context.CleverTags.FindAsync(id);

            updatedCleverTag.Tag = cleverTag.Tag;
            updatedCleverTag.PlateNum = cleverTag.PlateNum;
            

            // Check if any properties have been modified
            var changedProperties = _context.Entry(updatedCleverTag)
                                        .Properties
                                        .Where(p => p.IsModified)
                                        .Select(p => new { Name = p.Metadata.Name, Value = p.CurrentValue })
                                        .ToList();

            // Create a message with the changed properties
            var message = $"CleverTag with ID {id} updated. Changes: {string.Join(", ", changedProperties.Select(p => $"{p.Name}: {p.Value}"))}";

            return Ok(new { Message = message, CleverTag = updatedCleverTag });
        }


        //CreateTag
        // POST: api/CreateCleverTag
        [HttpPost("CreateCleverTag")]
        public async Task<ActionResult<CleverTag>> CreateCleverTag(CleverTag cleverTag)
        {
            _context.CleverTags.Add(cleverTag);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCleverTag), new { id = cleverTag.Id }, cleverTag);
        }

        public class BlockCleverTagModel
        {
            public bool IsBlocked { get; set; }
        }


        //Block or Unblock Tag
        //Put: api/BlockCleverTag{id}
        [HttpPut("BlockCleverTag/{id}")]
        public async Task<ActionResult<CleverTag>> BlockCleverTag(int id)
        {
            var cleverTag = await _context.CleverTags.FindAsync(id);

            if (cleverTag == null)
            {
                return NotFound();
            }

            // Reverts the current bool
            cleverTag.Isblocked = !cleverTag.Isblocked;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CleverTagExists(cleverTag.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var message = cleverTag.Isblocked

            ? $"CleverTag with ID {cleverTag.Id} is now blocked."
            : $"CleverTag with ID {cleverTag.Id} is now active.";

            return Ok(new { Message = message, CleverTag = cleverTag });
        }

        //DeleteCleverTag
        // DELETE: api/CleverTags{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCleverTag(int id)
        {
            var cleverTag = await _context.CleverTags.FindAsync(id);
            if (cleverTag == null)
            {
                return NotFound();
            }

            _context.CleverTags.Remove(cleverTag);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CleverTagExists(int id)
        {
            return _context.CleverTags.Any(e => e.Id == id);
        }
    }
}
