using MediatR;
using MultiShop.Order.Application.Features.Mediator.Commands.OrderingCommands;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Application.Features.Mediator.Handlers.OrderingHandlers
{
    public class CreateOrderingCommandHandler : IRequestHandler<CreateOrderingCommand, int>
    {

        private readonly IRepository<Ordering> _repository;
        public CreateOrderingCommandHandler(IRepository<Ordering> repository)
        {
            _repository = repository;
        }


        public async Task<int> Handle(CreateOrderingCommand request, CancellationToken cancellationToken)
        {
            var entity = new Ordering
            {
                UserId = request.UserId,
                OrderDate = request.OrderDate,
                TotalPrice = request.TotalPrice
            };

            await _repository.CreateAsync(entity);

            return entity.OrderingId; 
        }
    }
}
