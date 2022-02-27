using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperHeroWebApi.Data;
using SuperHeroWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperHeroWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly DataContext _context;
        public SuperHeroController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("heros")]
        public async Task<ActionResult<List<SuperHero>>> Get()
        {
            return Ok(await _context.Superheroes.ToListAsync());
        }
        [HttpPost("heros")]
        public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero)
        {
            _context.Superheroes.Add(hero);
            await _context.SaveChangesAsync();
            return Ok(await _context.Superheroes.ToListAsync());
        }

        [HttpGet("hero/{id}")]
        public async Task<ActionResult<SuperHero>> GetSingleHero(int id)
        {
            var hero = await _context.Superheroes.FindAsync(id);
            if (hero == null)
                return BadRequest("Hero not found.");
            return Ok(hero);
        }

        [HttpPut("hero/{id}")]
        public async Task<ActionResult<SuperHero>> UpdateHero(SuperHero data, int id)
        {
            var dbHero = _context.Superheroes.FindAsync(id);
            if (dbHero == null)
                return BadRequest("Hero not found.");

            dbHero.Result.Name = data.Name;
            dbHero.Result.FirstName = data.FirstName;
            dbHero.Result.LastName = data.LastName;
            dbHero.Result.Place = data.Place;

           await _context.SaveChangesAsync();

            return Ok(await _context.Superheroes.ToListAsync());
        }

        [HttpDelete("hero/{id}")]
        public async Task<ActionResult<SuperHero>> DeleteHero(int id)
        {
            var _dbHero = _context.Superheroes.FindAsync(id);
            if (_dbHero == null)
                return BadRequest("Hero not found.");

            _context.Superheroes.Remove(_dbHero.Result);
            await _context.SaveChangesAsync();

            return Ok($"{_dbHero.Result.Name} is deleted from the list");
        }
    }
}
