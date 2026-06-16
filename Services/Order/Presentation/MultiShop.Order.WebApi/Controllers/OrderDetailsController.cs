using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Order.Application.Features.CQRS.Commands.OrderDetailCommands;
using MultiShop.Order.Application.Features.CQRS.Handlers.OrderDetailHandlers;
using MultiShop.Order.Application.Features.CQRS.Queries.OrderDetailQueries;

namespace MultiShop.Order.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly CreateOrderDetailCommandHandler _createOrderDetailCommandHandler;
        private readonly UpdateOrderDetailCommandHandler _updateOrderDetailCommandHandler;
        private readonly RemoveOrderDetailCommandHandler _removeOrderDetailCommandHandler;
        private readonly GetOrderDetailByIdQueryHandler _getOrderDetailByIdQueryHandler;
        private readonly GetOrderDetailQueryHandler _getOrderDetailQueryHandler;
        private readonly GetOrderDetailsByVendorIdQueryHandler _getOrderDetailsByVendorIdQueryHandler;
        private readonly GetOrderDetailsByOrderingIdQueryHandler _getOrderDetailsByOrderingIdQueryHandler;
        public OrderDetailsController(CreateOrderDetailCommandHandler createOrderDetailCommandHandler,
            UpdateOrderDetailCommandHandler updateOrderDetailCommandHandler,
            RemoveOrderDetailCommandHandler removeOrderDetailCommandHandler,
             GetOrderDetailByIdQueryHandler getOrderDetailByIdQueryHandler,
             GetOrderDetailQueryHandler getOrderDetailQueryHandler,
              GetOrderDetailsByVendorIdQueryHandler getOrderDetailsByVendorIdQueryHandler,
              GetOrderDetailsByOrderingIdQueryHandler getOrderDetailsByOrderingIdQueryHandler)
        {
            _createOrderDetailCommandHandler = createOrderDetailCommandHandler;
            _updateOrderDetailCommandHandler = updateOrderDetailCommandHandler;
            _removeOrderDetailCommandHandler = removeOrderDetailCommandHandler;
            _getOrderDetailByIdQueryHandler = getOrderDetailByIdQueryHandler;
            _getOrderDetailQueryHandler = getOrderDetailQueryHandler;
            _getOrderDetailsByVendorIdQueryHandler = getOrderDetailsByVendorIdQueryHandler;
            _getOrderDetailsByOrderingIdQueryHandler = getOrderDetailsByOrderingIdQueryHandler;
        }

        [HttpGet] 
        public async Task<IActionResult> OrderDetailList()
        {
            var results = await _getOrderDetailQueryHandler.Handle();
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetailById(int id)
        {
            var result = await _getOrderDetailByIdQueryHandler.Handle(new GetOrderDetailByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderDetail(CreateOrderDetailCommand createOrderDetailCommand)
        {
            await _createOrderDetailCommandHandler.Handle(createOrderDetailCommand);
            return Ok("adding is successfull");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveOrderDetail(int id)
        {
            await _removeOrderDetailCommandHandler.Handle(new RemoveOrderDetailCommand(id));
            return Ok("removing is successfull");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrderDetail(UpdateOrderDetailCommand updateOrderDetailCommand)
        {
            await _updateOrderDetailCommandHandler.Handle(updateOrderDetailCommand);
            return Ok("updating is successfull");
        }

        [HttpGet("GetByVendorId/{vendorId}")]
        public async Task<IActionResult> GetByVendorId(string vendorId)
        {
            var values = await _getOrderDetailsByVendorIdQueryHandler
                .Handle(new GetOrderDetailsByVendorIdQuery(vendorId));

            return Ok(values);
        }

        [HttpGet("GetByOrderingId/{orderingId}")]
        public async Task<IActionResult> GetByOrderingId(int orderingId)
        {
            var values = await _getOrderDetailsByOrderingIdQueryHandler
                .Handle(new GetOrderDetailsByOrderingIdQuery(orderingId));

            return Ok(values);
        }
    }
}
