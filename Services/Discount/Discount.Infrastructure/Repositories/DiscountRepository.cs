using Dapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Infrastructure.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Coupon?> GetDiscount(string productName)
        {
            await using var connection = StablishConnection();
            return await connection.QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });
        }


        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            await using var connection = StablishConnection();
            var afftected = await connection.ExecuteAsync("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new {ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount});

            if (afftected == 0)
                return false;

            return true;

        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            await using var connection = StablishConnection();
            var affected = await connection.ExecuteAsync(
                "UPDATE Coupon SET ProductName=@ProductName, Description=@Description, Amount=@Amount WHERE Id=@Id",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id }
                );
            if (affected == 0) return false;

            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            await using var connection = StablishConnection();
            var affected = await connection.ExecuteAsync(
                "DELETE FROM Coupon WHERE ProductName=@ProductName",
                new { ProductName = productName}
                );
            if (affected == 0) return false;

            return true;
        }



        private NpgsqlConnection StablishConnection()
        {
            var connectionString = _configuration["DatabaseSettings:ConnectionString"];
            return new NpgsqlConnection(connectionString);
        }
    }
}
