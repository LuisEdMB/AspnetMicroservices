using Dapper;
using Discount.Grpc.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly NpgsqlConnection _connection;

        public DiscountRepository(IConfiguration configuration)
        {
            _connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            var affected = await _connection.ExecuteAsync(
                "insert into Coupon(productName, description, amount) values(@ProductName, @Description, @Amount)",
                new { coupon.ProductName, coupon.Description, coupon.Amount });
            return affected == 0;
        }

        public async Task<bool> DeleteCoupon(string productName)
        {
            var affected = await _connection.ExecuteAsync(
                "delete from Coupon where productName = @productName",
                new { productName });
            return affected > 0;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            var coupon = await _connection.QueryFirstOrDefaultAsync<Coupon>(
                "select * from coupon where productName = @productName",
                new { productName });
            if (coupon == null)
                return new Coupon
                {
                    ProductName = "No Discount",
                    Amount = 0,
                    Description = "No Discount Description"
                };
            return coupon;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            var affected = await _connection.ExecuteAsync(
                "update Coupon set productName = @ProductName, description = @Description, amount = @Amount where id = @Id",
                new { coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id });
            return affected > 0;
        }
    }
}
