using AutoMapper;
using BlossomApi.Dtos.Orders;
using BlossomApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminOrderFilterController : ControllerBase
    {
        private readonly OrderFilterPanelService _orderFilterPanelService;
        private readonly OrderQueryService _orderQueryService;
        private readonly IMapper _mapper;

        public AdminOrderFilterController(OrderFilterPanelService orderFilterPanelService, OrderQueryService orderQueryService, IMapper mapper)
        {
            _orderFilterPanelService = orderFilterPanelService;
            _orderQueryService = orderQueryService;
            _mapper = mapper;
        }

        // GET: api/AdminOrderFilterPanel
        [HttpGet("GetAdminOrderFilterPanel")]
        public async Task<ActionResult<AdminOrderFilterPanelResponseDto>> GetAdminOrderFilterPanel()
        {
            var filterPanelData = await _orderFilterPanelService.GetFilterPanelDataAsync();
            if (filterPanelData == null)
            {
                return NotFound();
            }
            return Ok(filterPanelData);
        }

        // POST: api/AdminOrderFilterPanel/GetOrdersByAdminFilter
        [HttpPost("GetOrdersByAdminFilter")]
        public async Task<ActionResult<GetOrdersByFilterResponse>> GetOrdersByAdminFilter(GetOrdersByAdminFilterRequestDto request)
        {
            var query = await _orderQueryService.ApplyFilterAndSortAsync(request);

            var totalCount = await query.CountAsync();

            var orders = await query
                .Skip(request.Start)
                .Take(request.Amount)
                .ToListAsync();

            var orderDtos = _mapper.Map<List<OrderDto>>(orders);

            var response = new GetOrdersByFilterResponse
            {
                Orders = orderDtos,
                TotalCount = totalCount
            };

            return Ok(response);
        }
    }
}