using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class DiscountService:DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly ILogger<DiscountService> _logger;
        private readonly IDiscountRepository   _repository;

        private readonly IMapper _mapper;
        public DiscountService(ILogger<DiscountService> logger,IMapper mapper, IDiscountRepository discountRepository)
        {
            _logger = logger;
            _repository = discountRepository;
            _mapper = mapper;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon =  await _repository.GetDiscount(request.ProductName);
            if(coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with product name ={request.ProductName} was not found."));
            }
            _logger.LogInformation("Discount info is retrieved for product :{productName}, Amount :{amount}", coupon.ProductName,coupon.Amount);
            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
           bool success= await _repository.CreateDiscount(coupon);
            if (success)
            {
                _logger.LogInformation("Discount created");
                return _mapper.Map<CouponModel>(coupon);
            }

            throw new RpcException(new Status(StatusCode.Unknown, "Discount creation failed"));
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            bool success = await _repository.DeleteDiscount(request.ProductName);
            if (success)
                _logger.LogInformation("Discount deleted");
            return new DeleteDiscountResponse
            {
                Success = success
            };
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            bool success = await _repository.UpdateDiscount(coupon);
            if (success)
            {
                _logger.LogInformation("Discount updated");
                return _mapper.Map<CouponModel>(coupon);
            }

            throw new RpcException(new Status(StatusCode.Unknown, "Discount updated failed"));
        }
    }
}
