using MediatR;
using MultiShop.Order.Application.Features.Mediator.Results.OrderingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Application.Features.Mediator.Queries.OrderingQueries
{
    // GetOrderingByIdQuery objesi 1 parametre alır ve geriye GetOrderingByIdQueryResult döner.
    public class GetOrderingByIdQuery : IRequest<GetOrderingByIdQueryResult>
    {
        public int OrderingId { get; set; }
        public GetOrderingByIdQuery(int id)
        {
            OrderingId = id;
        }
    }
}
