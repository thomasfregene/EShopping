using Basket.Application.Commands;
using Basket.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Handlers
{
    public class DeleteBasketByUsernameHandler : IRequestHandler<DeleteBasketByUsernameCommand>
    {
        private readonly IBasketRepository _basketRepository;

        public DeleteBasketByUsernameHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }
        public async Task<Unit> Handle(DeleteBasketByUsernameCommand request, CancellationToken cancellationToken)
        {
            await _basketRepository.DeleteBasket(request.Username);
            return Unit.Value;
        }
    }
}
