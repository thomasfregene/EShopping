﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Infrastructure.Extensions
{
    public static class DbExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var servies = scope.ServiceProvider;
                var config = servies.GetRequiredService<IConfiguration>();
                var logger = servies.GetRequiredService<ILogger<TContext>>();
                try
                {
                    logger.LogInformation("Discount DB migration started");
                    ApplyMigration(config);
                    logger.LogInformation("Discount DB migration completed");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }

            return host;
        }

        private static void ApplyMigration(IConfiguration config)
        {
            using var connection = new NpgsqlConnection(config.GetValue<string>("DatabaseSettings:ConnectionString"));
            connection.Open();
            using var cmd = new NpgsqlCommand()
            {
                Connection = connection
            };
            cmd.CommandText = "DROP TABLE IF EXISTS Coupon";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                ProductName VARCHAR(500) NOT NULL, Description TEXT, Amount INT)";

            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Adidas Quick Force Indoor Badminton Shoes', 'Shoe Discount', 500);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Yonex VCORE Pro 100 A Tennis Racquet (270gm, Strung)', 'Racquet Discount', 700);";
            cmd.ExecuteNonQuery();
        }
    }
}
