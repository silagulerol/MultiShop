using MediatR;
using MultiShop.Order.Application.Features.Mediator.Queries.OrderingQueries;
using MultiShop.Order.Application.Features.Mediator.Results.OrderingResults;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Application.Features.Mediator.Handlers.OrderingHandlers
{
    /* Generic eşleşme:
        Query:  IRequest<ResultType>
	
	    Result: IRequestHandler<QueryType, ResultType>

     *  IRequestHandler'dan miras alacak bu sınıf.Peki kim için?
     *  İstek yapılan yer neresi? İstsek yapan yer bizim IRequest sınıfımızı tutan yer olacak.
     *  Peki bu istek nereden karşılanacak
    */
    public class GetOrderingQueryHandler : IRequestHandler<GetOrderingQuery, List<GetOrderingQueryResult>>
    {
        private readonly IRepository<Ordering> _repository;
        public GetOrderingQueryHandler(IRepository<Ordering> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetOrderingQueryResult>> Handle(GetOrderingQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetAllAsync();
            return values.Select( x => new GetOrderingQueryResult
            {
                OrderingId = x.OrderingId,
                UserId  = x.UserId,
                OrderDate = x.OrderDate,
                TotalPrice  = x.TotalPrice
            }).ToList();
        }
    }
}
