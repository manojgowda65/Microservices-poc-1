using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistance;
using Ordering.Application.Models;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _mailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper
            , IEmailService emailService, ILogger<CheckoutOrderCommandHandler> logger)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _mailService = emailService;
            _logger = logger;
        }
        public async Task<int> Handle(CheckoutOrderCommand request
            , CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request);
            Order newOrder = await _orderRepository.AddAsync(orderEntity);

            _logger.LogInformation($"Order {newOrder.Id} is created");

            await SendMail(newOrder);
            return newOrder.Id;
        }

        private async Task SendMail(Order newOrder)
        {
            try
            {
                Email mail = new Email()
                {
                    To = "manojgowdames@gmail.com",
                    Subject = $"Order is created",
                    Body = $"Order id {newOrder.Id}"
                };
                await _mailService.SendMail(mail);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Order with id:{newOrder.Id} failed due to error at the email service. Failed to send order email. Exception details : {ex.Message}");
            }
        }
    }
}
