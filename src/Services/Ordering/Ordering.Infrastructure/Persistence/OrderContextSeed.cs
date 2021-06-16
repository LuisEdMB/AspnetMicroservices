﻿using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext context, ILogger<OrderContextSeed> logger)
        {
            if (!context.Orders.Any())
            {
                context.Orders.AddRange(GetPreconfiguredOrders());
                await context.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order {
                    UserName = "LuisEdMB",
                    FirstName = "Luis Eduardo",
                    LastName = "Mamani Bedregal",
                    EmailAddress = "bedregale@gmail.com",
                    AddressLine = "Tacna",
                    Country = "Peru",
                    TotalPrice = 350
                }
            };
        }
    }
}
