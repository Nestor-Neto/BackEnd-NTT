using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities.DataTransferObjects;
using Ambev.DeveloperEvaluation.Application.Customer.Create;
using Ambev.DeveloperEvaluation.WebApi.Features.Register.Create;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class CreateCustomerProfile : Profile
{
    public CreateCustomerProfile()
    {
        CreateMap<CreateCustomerRequest, CreateCustomerCommand>();
        CreateMap<CreateBranchRequest, CreateBranchDto>();
    }
}
