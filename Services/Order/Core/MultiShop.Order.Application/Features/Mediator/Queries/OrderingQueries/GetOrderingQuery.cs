using MediatR;
using MultiShop.Order.Application.Features.Mediator.Results.OrderingResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Application.Features.Mediator.Queries.OrderingQueries
{
    // Ben GetOrderingQuery çağırdığımda bana geriye List<GetOrderingQueryResult> dönücek dedik
    public class GetOrderingQuery : IRequest<List<GetOrderingQueryResult>>
    {
    }
}
