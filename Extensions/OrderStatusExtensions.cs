using BlossomApi.Models;

public static class OrderStatusExtensions
{
    public static string ToUkrainianName(this OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Created => "Створено",
            OrderStatus.Accepted => "Прийнято",
            OrderStatus.Declined => "Відхилено",
            OrderStatus.NeedToShip => "Потрібно відправити",
            OrderStatus.Shipped => "Відправлено",
            OrderStatus.Completed => "Завершено",
            OrderStatus.Refund => "Повернення",
            OrderStatus.Offline => "Офлайн",
            _ => "Невідомо"
        };
    }
}
