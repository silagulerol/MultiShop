using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Application.Features.Mediator.Commands.OrderingCommands
{
    // Sadece bir id parametresi alır ve bir değer dönmez
    public class RemoveOrderingCommand : IRequest
    {
        public int OrderingId { get; set; }
        public RemoveOrderingCommand(int Id)
        {
            OrderingId = Id;
        }
    }
}
