using Ambev.DeveloperEvaluation.Application.Sales.Create;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Create;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class SaleMappingProfile : Profile
{
    public SaleMappingProfile()
    {
        CreateMap<CreateSaleRequest, CreateSaleCommand>();
    }
}