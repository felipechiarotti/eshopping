using Basket.Core.Entities;
using Basket.Core.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _cache;

        public BasketRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<ShoppingCart?> GetBasket(string userName)
        {
            var basket = await _cache.GetStringAsync(userName);
            if(string.IsNullOrEmpty(basket))
            {
                return default;
            }

            return JsonSerializer.Deserialize<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart)
        {
            var cart = JsonSerializer.Serialize(shoppingCart);
            await _cache.SetStringAsync(shoppingCart.UserName, cart);
            return await GetBasket(shoppingCart.UserName);
        }

        public async Task DeleteBasket(string userName)
        {
            await _cache.RemoveAsync(userName);
        }
    }
}
