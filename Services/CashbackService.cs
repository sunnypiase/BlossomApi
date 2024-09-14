using System.Threading.Tasks;
using BlossomApi.DB;
using BlossomApi.Models;
using Microsoft.EntityFrameworkCore;
using static BlossomApi.Controllers.ShoppingCartOrderController;

namespace BlossomApi.Services
{
    public class CashbackService
    {
        private readonly BlossomContext _context;

        public CashbackService(BlossomContext context)
        {
            _context = context;
        }

        // Метод для отримання або створення кешбеку
        public async Task<Cashback> GetOrCreateCashbackAsync(SiteUser? siteUser, string phoneNumber)
        {
            Cashback? cashback = null;

            if (siteUser != null)
            {
                // Для авторизованих користувачів отримуємо кешбек за UserId
                cashback = await _context.Cashbacks.FirstOrDefaultAsync(c => c.SiteUserId == siteUser.UserId);

                if (cashback == null)
                {
                    // Якщо кешбек не знайдено, створюємо новий запис
                    cashback = new Cashback
                    {
                        PhoneNumber = siteUser.PhoneNumber,
                        Balance = 0,
                        SiteUserId = siteUser.UserId
                    };
                    _context.Cashbacks.Add(cashback);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                // Для гостей отримуємо кешбек за номером телефону
                cashback = await _context.Cashbacks.FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber);

                if (cashback == null)
                {
                    // Якщо кешбек не знайдено, створюємо новий запис
                    cashback = new Cashback
                    {
                        PhoneNumber = phoneNumber,
                        Balance = 0,
                        SiteUserId = null
                    };
                    _context.Cashbacks.Add(cashback);
                    await _context.SaveChangesAsync();
                }
            }

            return cashback;
        }

        // Метод для перевірки та застосування кешбеку
        public string ValidateAndApplyCashback(OrderBaseRequest request, Order order, Cashback cashback)
        {
            decimal cashbackToUse = 0;

            if (request is GuestOrderRequest guestRequest)
            {
                cashbackToUse = guestRequest.CashbackToUse;
            }
            else if (request is AuthenticatedOrderRequest authRequest)
            {
                cashbackToUse = authRequest.CashbackToUse;
            }

            if (cashbackToUse < 0)
            {
                return "Сума кешбеку не може бути від'ємною.";
            }

            if (cashbackToUse > cashback.Balance)
            {
                return "Недостатньо коштів на балансі кешбеку.";
            }

            order.DiscountFromCashback = cashbackToUse;
            cashback.Balance -= cashbackToUse;

            _context.Cashbacks.Update(cashback);

            return string.Empty;
        }

        // Метод для оновлення балансу кешбеку після замовлення
        public void UpdateCashbackBalance(Order order, Cashback cashback)
        {
            decimal cashbackEarned = order.TotalPriceWithDiscount * 0.03m;

            cashback.Balance += cashbackEarned;

            _context.Cashbacks.Update(cashback);

            order.CashbackEarned = cashbackEarned;

            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        // Отримання кешбеку за UserId
        public async Task<Cashback?> GetCashbackByUserIdAsync(int userId)
        {
            return await _context.Cashbacks.FirstOrDefaultAsync(c => c.SiteUserId == userId);
        }

        // Отримання кешбеку за номером телефону
        public async Task<Cashback?> GetCashbackByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Cashbacks.FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber);
        }

        // Адміністративне коригування балансу кешбеку
        public async Task<(bool Success, string? ErrorMessage)> AdjustCashbackBalanceAsync(string phoneNumber, decimal amount)
        {
            var cashback = await GetCashbackByPhoneNumberAsync(phoneNumber);
            if (cashback == null)
            {
                return (false, "Кешбек не знайдено для заданого номера телефону.");
            }

            cashback.Balance += amount;

            if (cashback.Balance < 0)
            {
                return (false, "Баланс кешбеку не може бути від'ємним.");
            }

            _context.Cashbacks.Update(cashback);
            await _context.SaveChangesAsync();

            return (true, null);
        }
    }
}
