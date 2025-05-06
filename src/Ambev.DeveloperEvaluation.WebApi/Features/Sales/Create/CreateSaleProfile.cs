using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.Create;
using Ambev.DeveloperEvaluation.Domain.Entities.DataTransferObjects;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Create;

public class CreateSaleProfile : Profile
{
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleRequest, CreateSaleCommand>();
        CreateMap<CreateSaleItemRequest, SaleItemCreateDto>();
    }
} 