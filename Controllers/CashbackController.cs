using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BlossomApi.DB;
using BlossomApi.Models;
using BlossomApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlossomApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CashbackController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly CashbackService _cashbackService;

        public CashbackController(
            BlossomContext context,
            UserManager<IdentityUser> userManager,
            CashbackService cashbackService)
        {
            _context = context;
            _userManager = userManager;
            _cashbackService = cashbackService;
        }

        // 1. Отримання балансу кешбеку для поточного авторизованого користувача
        [Authorize]
        [HttpGet("GetBalance")]
        public async Task<IActionResult> GetBalance()
        {
            var siteUser = await GetCurrentUserAsync();
            if (siteUser == null)
            {
                return Unauthorized();
            }

            var cashback = await _cashbackService.GetCashbackByUserIdAsync(siteUser.UserId);
            if (cashback == null)
            {
                return NotFound("Кешбек не знайдено.");
            }

            return Ok(new { Balance = cashback.Balance });
        }

        // 2. Отримання балансу кешбеку за номером телефону (для гостей)
        [HttpGet("GetBalanceByPhone")]
        public async Task<IActionResult> GetBalanceByPhone([FromQuery] string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return BadRequest("Номер телефону є обов'язковим.");
            }

            var cashback = await _cashbackService.GetCashbackByPhoneNumberAsync(phoneNumber);
            if (cashback == null)
            {
                return NotFound("Кешбек не знайдено.");
            }

            return Ok(new { Balance = cashback.Balance });
        }

        // 3. Адміністративне коригування балансу кешбеку
        [Authorize(Roles = "Admin")]
        [HttpPost("AdjustBalance")]
        public async Task<IActionResult> AdjustBalance([FromBody] AdjustCashbackRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _cashbackService.AdjustCashbackBalanceAsync(request.PhoneNumber, request.Amount);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(new { Message = "Баланс кешбеку успішно оновлено." });
        }

        // 4. Перегляд історії кешбеку (опціонально, потребує додаткової реалізації)
        // ...

        // Допоміжний метод для отримання поточного користувача
        private async Task<SiteUser?> GetCurrentUserAsync()
        {
            var identityUserId = _userManager.GetUserId(User);
            return await _context.SiteUsers.FirstOrDefaultAsync(u => u.IdentityUserId == identityUserId);
        }
    }

    // Модель запиту для коригування кешбеку
    public class AdjustCashbackRequest
    {
        [Required(ErrorMessage = "Номер телефону є обов'язковим.")]
        [Phone(ErrorMessage = "Неправильний формат номера телефону.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Сума є обов'язковою.")]
        public decimal Amount { get; set; } // Позитивне або негативне значення для збільшення або зменшення балансу
    }
}
