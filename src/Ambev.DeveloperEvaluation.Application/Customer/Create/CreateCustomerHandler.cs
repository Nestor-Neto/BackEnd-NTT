using Ambev.DeveloperEvaluation.Domain.Interface.IServices;
using Ambev.DeveloperEvaluation.Domain.Entities.DataTransferObjects;

namespace Ambev.DeveloperEvaluation.Application.Customer.Create;

public class CreateCustomerHandler
{
    private readonly ICustomerService _customerService;

    public CreateCustomerHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<Domain.Entities.Customer> HandleAsync(CreateCustomerCommand command)
    {
        var customerDto = new CreateCustomerDto
        {
            CustomerName = command.CustomerName,
            Email = command.Email,
            Phone = command.Phone,
            Branches = command.Branches.Select(branch => new CreateBranchDto
            {
                BranchName = branch.BranchName,
                Email = branch.Email,
                Phone = branch.Phone,
                IsBranch = branch.IsBranch
            }).ToList()
        };

        return await _customerService.CreateCustomerAsync(customerDto);
    }
}
