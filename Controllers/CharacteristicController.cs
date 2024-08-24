using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlossomApi.DB;
using BlossomApi.Dtos;
using BlossomApi.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlossomApi.Dtos.Characteristic;
using System.Reflection.PortableExecutable;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacteristicController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly IMapper _mapper;

        public CharacteristicController(BlossomContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Characteristic
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CharacteristicDto>>> GetCharacteristics()
        {
            var characteristics = await _context.Characteristics.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<CharacteristicDto>>(characteristics));
        }

        // GET: api/Characteristic/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CharacteristicDto>> GetCharacteristicById(int id)
        {
            var characteristic = await _context.Characteristics.FindAsync(id);

            if (characteristic == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CharacteristicDto>(characteristic));
        }

        // POST: api/Characteristic
        [HttpPost]
        public async Task<ActionResult<CharacteristicDto>> AddCharacteristic(CharacteristicCreateDto characteristicCreateDto)
        {
            var characteristic = _mapper.Map<Characteristic>(characteristicCreateDto);
            _context.Characteristics.Add(characteristic);
            await _context.SaveChangesAsync();

            var characteristicDto = _mapper.Map<CharacteristicDto>(characteristic);
            return CreatedAtAction(nameof(GetCharacteristicById), new { id = characteristicDto.CharacteristicId }, characteristicDto);
        }

        // PUT: api/Characteristic/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCharacteristic(int id, CharacteristicUpdateDto characteristicUpdateDto)
        {
            var characteristic = await _context.Characteristics.FindAsync(id);
            if (characteristic == null)
            {
                return NotFound();
            }

            _mapper.Map(characteristicUpdateDto, characteristic);
            _context.Entry(characteristic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!CharacteristicExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Characteristic/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacteristic(int id)
        {
            var characteristic = await _context.Characteristics.FindAsync(id);
            if (characteristic == null)
            {
                return NotFound();
            }

            _context.Characteristics.Remove(characteristic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Characteristic/search/{name}
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<string>>> SearchCharacteristicsByName(string? name)
        {
            var characteristics = name != null
                ? (await _context.Characteristics
                    .Where(c => c.Title.ToLower().Contains(name.ToLower()))
                    .ToListAsync())
                    .DistinctBy(c => c.Title)
                : (await _context.Characteristics
                    .ToListAsync())
                    .DistinctBy(c => c.Title);

            var values = characteristics
                .Select(c => c.Title )
                .ToList();

            return Ok(values);
        }

        // GET: api/Characteristic/{id}/values
        [HttpGet("{Title}/values")]
        public async Task<ActionResult<IEnumerable<string>>> GetCharacteristicValues(string Title)
        {
            var characteristic = await _context.Characteristics
                .Where(c => c.Title == Title)
                .ToListAsync();

            if (characteristic == null || characteristic.Count == 0)
            {
                return Ok();
            }

            // Assuming characteristic values are stored in Desc field for simplicity
            var values = characteristic
                .Select(c => new { Desc = c.Desc, Id = c.CharacteristicId })
                .ToList();

            return Ok(values);
        }

        private bool CharacteristicExists(int id)
        {
            return _context.Characteristics.Any(c => c.CharacteristicId == id);
        }
    }
}
