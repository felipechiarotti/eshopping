using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Mappers;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers
{
    public class CreateShoppingCartCommandHandler :
        IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly DiscountService _discountService;

        public CreateShoppingCartCommandHandler(IBasketRepository basketRepository, DiscountService discountService)
        {
            _basketRepository = basketRepository;
            _discountService = discountService;
        }

        public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
        {
            foreach(var item in  request.Items)
            {
                var coupon = await _discountService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            };

            var shoppingCart = new ShoppingCart(request.UserName, request.Items);
            var shoppingCartResponse =  await _basketRepository.UpdateBasket(shoppingCart);
            return BasketMapper.Mapper.Map<ShoppingCartResponse>(shoppingCartResponse);
        }
    }
}
