using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlossomApi.DB;
using BlossomApi.Models;
using AutoMapper;
using BlossomApi.Dtos.Characteristic;

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
            if (await _context.Characteristics.AnyAsync(c => c.Title == characteristicCreateDto.Title && c.Desc == characteristicCreateDto.Desc))
            {
                return BadRequest("A characteristic with the same title and description already exists.");
            }

            var characteristic = _mapper.Map<Characteristic>(characteristicCreateDto);
            _context.Characteristics.Add(characteristic);
            await _context.SaveChangesAsync();

            var characteristicDto = _mapper.Map<CharacteristicDto>(characteristic);
            return CreatedAtAction(nameof(GetCharacteristicById), new { id = characteristicDto.CharacteristicId }, characteristicDto);
        }

        // DELETE: api/Characteristic/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacteristicById(int id)
        {
            var characteristic = await _context.Characteristics.FindAsync(id);
            if (characteristic == null)
            {
                return NotFound("Characteristic not found with the given ID.");
            }

            _context.Characteristics.Remove(characteristic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Characteristic?title={title}&description={description}
        [HttpDelete]
        public async Task<IActionResult> DeleteCharacteristicByTitleAndDescription(string title, string description)
        {
            var characteristic = await _context.Characteristics.FirstOrDefaultAsync(c => c.Title == title && c.Desc == description);
            if (characteristic == null)
            {
                return NotFound("Characteristic not found with the given title and description.");
            }

            _context.Characteristics.Remove(characteristic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Characteristic/all?title={title}
        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAllCharacteristicsByTitle(string title)
        {
            var characteristics = await _context.Characteristics.Where(c => c.Title == title).ToListAsync();
            if (characteristics == null || !characteristics.Any())
            {
                return NotFound("No characteristics found with the given title.");
            }

            _context.Characteristics.RemoveRange(characteristics);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Characteristic/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCharacteristic(int id, CharacteristicUpdateDto characteristicUpdateDto)
        {
            if (await _context.Characteristics.AnyAsync(c => c.Title == characteristicUpdateDto.Title && c.Desc == characteristicUpdateDto.Desc && c.CharacteristicId != id))
            {
                return BadRequest("A characteristic with the same title and description already exists.");
            }

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

        // GET: api/Characteristic/tree
        [HttpGet("tree")]
        public async Task<ActionResult<IEnumerable<CharacteristicTreeDto>>> GetCharacteristicsTree()
        {
            var characteristics = await _context.Characteristics
                .GroupBy(c => c.Title)
                .Select(group => new CharacteristicTreeDto
                {
                    Title = group.Key,
                    DescriptionsWithIds = group.Select(g => new DescriptionWithId
                    {
                        Description = g.Desc,
                        Id = g.CharacteristicId
                    }).ToList()
                })
                .ToListAsync();

            return Ok(characteristics);
        }

        // GET: api/Characteristic/search/{name}
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CharacteristicDto>>> SearchCharacteristicsByName(string? name)
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
                .Select(c => _mapper.Map<CharacteristicDto>(c))
                .ToList();

            return Ok(values);
        }

        // GET: api/Characteristic/{Title}/values
        [HttpGet("{Title}/values")]
        public async Task<ActionResult<IEnumerable<DescriptionWithId>>> GetCharacteristicValues(string Title)
        {
            var characteristics = await _context.Characteristics
                .Where(c => c.Title == Title)
                .ToListAsync();

            if (characteristics == null || characteristics.Count == 0)
            {
                return Ok();
            }

            var values = characteristics
                .Select(c => new DescriptionWithId { Description = c.Desc, Id = c.CharacteristicId })
                .ToList();

            return Ok(values);
        }

        private bool CharacteristicExists(int id)
        {
            return _context.Characteristics.Any(c => c.CharacteristicId == id);
        }
    }
}

// DTO for tree structure
public class CharacteristicTreeDto
{
    public string Title { get; set; }
    public List<DescriptionWithId> DescriptionsWithIds { get; set; }
}

public class DescriptionWithId
{
    public string Description { get; set; }
    public int Id { get; set; }
}