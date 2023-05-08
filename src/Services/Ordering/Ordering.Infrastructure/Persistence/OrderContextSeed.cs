using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public  class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext context,ILogger<OrderContextSeed> logger)
        {
            if (!context.Orders.Any())
            {
                context.Orders.AddRange(GetPreconfiguredOrders());
                await context.SaveChangesAsync();
                logger.LogInformation("seed database with one order");
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order() {UserName = "mg",
                    FirstName = "Manoj", 
                    LastName = "Gowda",
                    EmailAddress = "manojgowdames@gmail.com",
                    AddressLine = "Bengaluru", 
                    Country = "India", 
                    State="Karnataka",
                    ZipCode="12344",
                    PaymentMethod=1,
                    TotalPrice = 350,
                    CVV="123",
                    CardName="Manoj",
                    CardNumber="1234 1234 1234",
                    Expiration="",
                    LastModifiedBy="manoj",
                    LastModifiedDate=DateTime.Now,
                    CreatedBy="manoj",
                    CreatedDate=DateTime.Now
                }
            };
        }
    }
}
