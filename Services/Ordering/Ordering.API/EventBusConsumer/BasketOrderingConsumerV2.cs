﻿using AutoMapper;
using EventBus.Messages.Events;
using EventBus.Messges.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Commands;

namespace Ordering.API.EventBusConsumer
{
    public class BasketOrderingConsumerV2 : IConsumer<BasketCheckoutEventV2>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketOrderingConsumerV2> _logger;

        public BasketOrderingConsumerV2(IMediator mediator, IMapper mapper, ILogger<BasketOrderingConsumerV2> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEventV2> context)
        {
            using var scope = _logger.BeginScope("Consuming Basket Checkout Event for {correlationId}", context.Message.CorrelationId);
            var command = _mapper.Map<CheckoutOrderCommand>(context.Message);
            PopulateAddress(command);
            var result = await _mediator.Send(command);
            _logger.LogInformation("Basket checkout event completed!!!");
        }

        private static void PopulateAddress(CheckoutOrderCommand command)
        {
            command.FirstName = "Fregzy";
            command.LastName = "Tom";
            command.EmailAddress = "fregzy@eshop.net";
            command.AddressLine = "Lagos";
            command.Country = "NG";
            command.State = "LG";
            command.ZipCode = "560001";
            command.PaymentMethod = 1;
            command.CardName = "Visa";
            command.CardNumber = "1234567890123456";
            command.Expiration = "12/25";
            command.Cvv = "123";
        }
    }
}
