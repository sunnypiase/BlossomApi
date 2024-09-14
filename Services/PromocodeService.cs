using BlossomApi.DB;
using BlossomApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BlossomApi.Services
{
    public class PromocodeService
    {
        private readonly BlossomContext _context;

        public PromocodeService(BlossomContext context)
        {
            _context = context;
        }

        public async Task<string> ValidateAndApplyPromocode(string? usedPromocode, Order order)
        {
            if (string.IsNullOrEmpty(usedPromocode))
            {
                return string.Empty;
            }

            var promocode = await _context.Promocodes.FirstOrDefaultAsync(p => p.Code == usedPromocode);
            if (promocode == null)
            {
                return "Промокод не знайдено.";
            }

            if (promocode.ExpirationDate < DateTime.UtcNow)
            {
                return "Промокод прострочений.";
            }

            if (promocode.UsageLeft <= 0)
            {
                return "Промокод більше не доступний.";
            }

            order.PromocodeId = promocode.PromocodeId;
            order.Promocode = promocode;
            promocode.UsageLeft--;
            _context.Promocodes.Update(promocode);

            return string.Empty;
        }
    }
}
