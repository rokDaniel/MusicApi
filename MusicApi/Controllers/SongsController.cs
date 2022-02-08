using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Get()
        {
            //return _dbContext.Songs;
            return Ok(_dbContext.SongsList);
        }

        // GET api/<SongsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Song song = _dbContext.SongsList.Find(id);
            if (song == null)
            {
                return NotFound("No record found against this id");
            }

            return Ok(song);
        }

        // POST api/<SongsController>
        [HttpPost]
        public IActionResult Post([FromBody] Song song)
        {
            _dbContext.SongsList.Add(song);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
          
        // PUT api/<SongsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Song songObj)
        {
            Song song = _dbContext.SongsList.Find(id);
            if (song == null)
            {
                return NotFound("No record found against this id");
            }

            song.Title = songObj.Title;
            song.Language = songObj.Language;
            _dbContext.SaveChanges();

            return Ok("Record updated successfully");
        }

        // DELETE api/<SongsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Song song =_dbContext.SongsList.Find(id);
            if (song == null)
            {
                return NotFound("No record found against this id");
            }

            _dbContext.SongsList.Remove(song);
            _dbContext.SaveChanges();

            return Ok("Record deleted");
        }
    }
}
