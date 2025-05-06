using Ambev.DeveloperEvaluation.Application.Sales.Cancel;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Cancel;
using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.Create;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Create;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class CancelSaleRequestProfile : Profile
{
    public CancelSaleRequestProfile()
    {
            CreateMap<CancelSaleRequest, CancelSaleCommand>();
            CreateMap<CreateSaleRequest, CreateSaleCommand>();            
            CreateMap<CreateSaleItemRequest, CreateSaleItemDto>();
        }
    }
