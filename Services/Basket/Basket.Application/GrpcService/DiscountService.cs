using Discount.Grpc.Protos;


namespace Basket.Application.GrpcService
{
    public class DiscountService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _grpcClient;

        public DiscountService(DiscountProtoService.DiscountProtoServiceClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var discountRequest = new GetDiscountRequest
            {
                ProductName = productName
            };
            return await _grpcClient.GetDiscountAsync(discountRequest);
        }

    }
}
