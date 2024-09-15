using System.ComponentModel.DataAnnotations;
using BlossomApi.DB;
using BlossomApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlossomApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromocodesController : ControllerBase
    {
        private readonly BlossomContext _context;

        public PromocodesController(BlossomContext context)
        {
            _context = context;
        }

        // GET: api/promocodes/check/{code}
        [HttpGet("check/{code}")]
        public async Task<IActionResult> CheckPromocode(string code)
        {
            var promocode = await _context.Promocodes.FirstOrDefaultAsync(p => p.Code == code);
            if (promocode is not { UsageLeft: > 0 } || promocode.ExpirationDate < DateTime.UtcNow)
            {
                return NotFound();
            }

            return Ok(promocode.Discount);
        }

        // GET: api/promocodes
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetPromocodes()
        {
            var promocodes = await _context.Promocodes.ToListAsync();
            return Ok(promocodes.Select(p => new PromocodeDto
            {
                PromocodeId = p.PromocodeId,
                Code = p.Code,
                Discount = p.Discount,
                UsageLeft = p.UsageLeft,
                ExpirationDate = p.ExpirationDate
            }));
        }

        // GET: api/promocodes/{id}
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPromocode(int id)
        {
            var promocode = await _context.Promocodes.FindAsync(id);
            if (promocode == null)
            {
                return NotFound();
            }

            var promocodeDto = new PromocodeDto
            {
                PromocodeId = promocode.PromocodeId,
                Code = promocode.Code,
                Discount = promocode.Discount,
                UsageLeft = promocode.UsageLeft,
                ExpirationDate = promocode.ExpirationDate
            };

            return Ok(promocodeDto);
        }

        // POST: api/promocodes
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreatePromocode([FromBody] CreatePromocodeDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var promocode = new Promocode
            {
                Code = createDto.Code,
                Discount = createDto.Discount,
                UsageLeft = createDto.UsageLeft,
                ExpirationDate = createDto.ExpirationDate
            };

            _context.Promocodes.Add(promocode);
            await _context.SaveChangesAsync();

            var promocodeDto = new PromocodeDto
            {
                PromocodeId = promocode.PromocodeId,
                Code = promocode.Code,
                Discount = promocode.Discount,
                UsageLeft = promocode.UsageLeft,
                ExpirationDate = promocode.ExpirationDate
            };

            return CreatedAtAction(nameof(GetPromocode), new { id = promocode.PromocodeId }, promocodeDto);
        }

        // PUT: api/promocodes/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePromocode(int id, [FromBody] UpdatePromocodeDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var promocode = await _context.Promocodes.FindAsync(id);
            if (promocode == null)
            {
                return NotFound();
            }

            // Update only the fields that are provided (not null)
            if (updateDto.Code != null)
            {
                promocode.Code = updateDto.Code;
            }

            if (updateDto.Discount.HasValue)
            {
                promocode.Discount = updateDto.Discount.Value;
            }

            if (updateDto.UsageLeft.HasValue)
            {
                promocode.UsageLeft = updateDto.UsageLeft.Value;
            }

            if (updateDto.ExpirationDate.HasValue)
            {
                promocode.ExpirationDate = updateDto.ExpirationDate.Value;
            }

            _context.Promocodes.Update(promocode);
            await _context.SaveChangesAsync();

            var promocodeDto = new PromocodeDto
            {
                PromocodeId = promocode.PromocodeId,
                Code = promocode.Code,
                Discount = promocode.Discount,
                UsageLeft = promocode.UsageLeft,
                ExpirationDate = promocode.ExpirationDate
            };

            return Ok(promocodeDto);
        }


        // DELETE: api/promocodes/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePromocode(int id)
        {
            var promocode = await _context.Promocodes.FindAsync(id);
            if (promocode == null)
            {
                return NotFound();
            }

            _context.Promocodes.Remove(promocode);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
    public class PromocodeDto
    {
        public int PromocodeId { get; set; }
        public string Code { get; set; }
        public decimal Discount { get; set; }
        public int UsageLeft { get; set; }
        public DateTime ExpirationDate { get; set; }
    }

    public class CreatePromocodeDto
    {
        [Required]
        public string Code { get; set; }
        [Required]
        [Range(0, 100)]
        public decimal Discount { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int UsageLeft { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
    }

    public class UpdatePromocodeDto
    {
        public string? Code { get; set; }
        [Range(0, 100)]
        public decimal? Discount { get; set; }
        [Range(0, int.MaxValue)]
        public int? UsageLeft { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }

}
