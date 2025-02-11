using AutoMapper;
using DistributedSystem.Contract.Abstractions.Shared;
using DistributedSystem.Domain.Entities;

namespace DistributedSystem.Application.Mapper;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        CreateMap<Product, DistributedSystem.Contract.Services.V1.Product.Response.ProductResponse>().ReverseMap();
        CreateMap<PageResult<Product>, PageResult<DistributedSystem.Contract.Services.V1.Product.Response.ProductResponse>>().ReverseMap();
    }
}
