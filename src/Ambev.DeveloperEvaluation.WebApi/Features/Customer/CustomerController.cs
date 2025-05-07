using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities.DataTransferObjects;
using Ambev.DeveloperEvaluation.Domain.Interface.IServices;
using Ambev.DeveloperEvaluation.WebApi.Features.Register.Create;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Application.Customer.Create;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
namespace Ambev.DeveloperEvaluation.WebApi.Features.Register;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    /// <summary>
    /// Creates a new customer
    /// </summary>
    /// <param name="request">Customer data to be created</param>
    /// <param name="mapper">AutoMapper for object mapping</param>
    /// <returns>Created customer</returns>
    /// <response code="201">Customer created successfully</response>
    /// <response code="400">Invalid request</response>
    /// <response code="409">Conflict - Customer or branch already exists</response>
    /// <response code="500">Internal server error</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<Customer>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCustomerRequest request,
        [FromServices] IMapper mapper)
    {
        try
        {
            var command = mapper.Map<CreateCustomerCommand>(request);

            var customerCreateDto = new CreateCustomerDto
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

            var customer = await _customerService.CreateCustomerAsync(customerCreateDto);
            return CreatedAtAction(nameof(GetById), new { id = customer.CustomerId }, customer);
        }
        catch (BusinessRuleException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An internal error occurred while processing your request." });
        }
    }

    /// <summary>
    /// Gets all customers with pagination
    /// </summary>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="size">Page size (default: 10)</param>
    /// <returns>List of customers</returns>
    /// <response code="200">List of customers returned successfully</response>
    /// <response code="400">Invalid pagination parameters</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<Customer>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        if (page < 1 || size < 1)
            return BadRequest(new { message = "Pagination parameters must be greater than zero." });

        var customers = await _customerService.GetAllAsync(page, size);
        var totalCount = await _customerService.GetTotalCustomersCountAsync();
        return Ok(new { data = customers, total = totalCount });
    }

    /// <summary>
    /// Gets a customer by ID
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <returns>Found customer</returns>
    /// <response code="200">Customer found</response>
    /// <response code="404">Customer not found</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<Customer>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
            return NotFound(new { message = $"Customer with ID {id} not found." });

        return Ok(customer);
    }
}

