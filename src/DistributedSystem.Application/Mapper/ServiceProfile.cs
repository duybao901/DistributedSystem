using AutoMapper;
using DistributedSystem.Contract.Abstractions.Shared;
using DistributedSystem.Contract.Services.V1.Product;
using DistributedSystem.Domain.Entities;

namespace DistributedSystem.Application.Mapper;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        // V1
        CreateMap<Product, Response.ProductResponse>().ReverseMap();
        CreateMap<PageResult<Product>, PageResult<Response.ProductResponse>>().ReverseMap();

        //// V2
        //CreateMap<Product, Contract.Services.V2.Product.Response.ProductResponse>().ReverseMap();
    }
}