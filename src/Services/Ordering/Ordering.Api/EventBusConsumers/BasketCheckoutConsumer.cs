using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Commands.CheckoutOrder;

namespace Ordering.Api.EventBusConsumers
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<BasketCheckoutConsumer> _logger;
        private readonly IMediator _mediator;
        public BasketCheckoutConsumer(IMapper mapper, ILogger<BasketCheckoutConsumer> logger, IMediator mediator)
        {
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            CheckoutOrderCommand checkoutOrderCommand = _mapper.Map<CheckoutOrderCommand>(context.Message);
            var id =await _mediator.Send(checkoutOrderCommand);

            _logger.LogInformation($"BasketCheckoutEvent consume success, response Id :{id}");
        }
    }
}
