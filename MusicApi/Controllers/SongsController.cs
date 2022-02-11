﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Helpers;
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

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Song song)
        {
            var imageUrl = await FileHelper.UploadImage(song.Image);
            song.ImageUrl = imageUrl;
            var AudioUrl = await FileHelper.UploadFile(song.AudioFile);
            song.AudioUrl = AudioUrl;
            song.UploadDate = DateTime.Now;
            await _dbContext.AddAsync(song);
            await _dbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSongs(int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 5;
            var songs = await (from song in _dbContext.SongsList
                                select new
                                {
                                    SongId = song.Id,
                                    SongTitle = song.Title,
                                    ImageUrl = song.ImageUrl,
                                    AudioUrl = song.AudioUrl
                                }).ToListAsync();

            return Ok(songs.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> FeaturedSongs()
        {
            var songs = await (from song in _dbContext.SongsList
                               where song.IsFeatured == true
                               select new
                               {
                                   SongId = song.Id,
                                   SongTitle = song.Title,
                                   ImageUrl = song.ImageUrl,
                                   AudioUrl = song.AudioUrl
                               }).ToListAsync();

            return Ok(songs);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> NewSongs()
        {
            var songs = await (from song in _dbContext.SongsList
                               orderby song.UploadDate descending
                               select new
                               {
                                   SongId = song.Id,
                                   SongTitle = song.Title,
                                   ImageUrl = song.ImageUrl,
                                   AudioUrl = song.AudioUrl
                               }).Take(5).ToListAsync();

            return Ok(songs);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> SearchSongs(string query)
        {
            var songs = await (from song in _dbContext.SongsList
                               where song.Title.StartsWith(query)
                               select new
                               {
                                   SongId = song.Id,
                                   SongTitle = song.Title,
                                   ImageUrl = song.ImageUrl,
                                   AudioUrl = song.AudioUrl
                               }).ToListAsync();

            return Ok(songs);
        }
    }
}
