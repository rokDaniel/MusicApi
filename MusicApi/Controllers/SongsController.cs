using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private ApiDbContext _dbContext;

        public SongsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<SongsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //return _dbContext.Songs;
            return Ok(await _dbContext.SongsList.ToListAsync());
        }

        // GET api/<SongsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Song song = await _dbContext.SongsList.FindAsync(id);
            if (song == null)
            {
                return NotFound("No record found against this id");
            }

            return Ok(song);
        }

        // POST api/<SongsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Song song)
        {
            await _dbContext.SongsList.AddAsync(song);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
          
        // PUT api/<SongsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Song songObj)
        {
            Song song = await _dbContext.SongsList.FindAsync(id);
            if (song == null)
            {
                return NotFound("No record found against this id");
            }

            song.Title = songObj.Title;
            song.Language = songObj.Language;
            song.Duration = songObj.Duration;
            await _dbContext.SaveChangesAsync();

            return Ok("Record updated successfully");
        }

        // DELETE api/<SongsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Song song = await _dbContext.SongsList.FindAsync(id);
            if (song == null)
            {
                return NotFound("No record found against this id");
            }

            _dbContext.SongsList.Remove(song);
            await _dbContext.SaveChangesAsync();

            return Ok("Record deleted");
        }
    }
}
