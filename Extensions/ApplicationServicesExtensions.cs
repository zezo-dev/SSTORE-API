using Microsoft.AspNetCore.Mvc;
using Store.Data.Context;
using Store.Repository.Interfaces;
using Store.Repository.Repositories;
using Store.Service.Services.ProductService.dtos;
using Store.Service.Services.ProductService;
using Store.Service.HandleResponse;
using Store.Service.Services.CacheService;
using Store.Repository.BasketRepository;
using Store.Service.Services.CustomerBasketService.Dtos;
using Store.Service.Services.CustomerBasketService;
using Store.Service.TokenService;
using Store.Service.Services.UserService;
using Store.Service.Services.PaymentService;
using Store.Service.Services.OrderService;

namespace STORE.Api.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
        {



          services.AddSingleton<ICacheService, CacheService>();


            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductService, ProductService>();
            services.AddAutoMapper(typeof(ProductProfile));
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IOrderservice, OrderService>();
            services.AddScoped<IBasketService, BasketService>();

            services.AddAutoMapper (typeof(BasketProfile));
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(model => model.Value.Errors.Count > 0)
                        .SelectMany(model => model.Value.Errors)
                        .Select(error => error.ErrorMessage)
                        .ToList();

                    var errorResponse = new ValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            return services;

        }




    }
}
