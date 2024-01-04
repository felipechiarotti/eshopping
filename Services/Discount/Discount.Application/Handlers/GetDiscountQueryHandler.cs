using Discount.Application.Mappers;
using Discount.Application.Queries;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.Application.Handlers
{
    internal class GetDiscountQueryHandler :
        IRequestHandler<GetDiscountQuery, CouponModel>
    {
        private readonly IDiscountRepository _discountRepository;

        public GetDiscountQueryHandler(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public async Task<CouponModel> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
        {
            var coupon = await _discountRepository.GetDiscount(request.ProductName);
            if(coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount for {request.ProductName} not found"));
            }

            return DiscountMapper.Mapper.Map<CouponModel>(coupon);
        }
    }
}
