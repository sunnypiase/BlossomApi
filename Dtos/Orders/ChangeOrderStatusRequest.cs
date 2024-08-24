using System.ComponentModel.DataAnnotations;
using BlossomApi.Models;

namespace BlossomApi.Dtos.Orders
{
    public class ChangeOrderStatusRequest
    {
        [Required] public OrderStatus Status { get; set; }
    }

}