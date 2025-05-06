namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Create;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Cancel;
using Ambev.DeveloperEvaluation.Domain.Interface.IServices;
using Ambev.DeveloperEvaluation.Application.Sales.Cancel;
using Ambev.DeveloperEvaluation.Application.Sales.Create;
using Ambev.DeveloperEvaluation.Domain.Entities.DataTransferObjects;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SalesController : ControllerBase
{
    private readonly ISaleService _saleService;

    public SalesController(ISaleService saleService)
    {
        _saleService = saleService;
    }

    /// <summary>
    /// Creates a new sale
    /// </summary>
    /// <param name="request">Sale data to be created</param>
    /// <param name="mapper">AutoMapper for object mapping</param>
    /// <returns>Created sale</returns>
    /// <response code="201">Sale created successfully</response>
    /// <response code="400">Invalid request</response>
    /// <response code="409">Conflict - Business rules violated</response>
    /// <response code="500">Internal server error</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<Sale>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] CreateSaleRequest request,
        [FromServices] IMapper mapper)
    {
        try
        {
            var command = mapper.Map<CreateSaleCommand>(request);

            var saleCreateDto = new SaleCreateDto
            {
                CustomerId = command.CustomerId,    
                CustomerName = command.CustomerName,
                BranchId = command.BranchId,
                BranchName = command.BranchName,
                Items = command.Items.Select(item => new SaleItemCreateDto
                {
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };
            var sale = await _saleService.CreateSaleAsync(saleCreateDto);
            return CreatedAtAction(nameof(GetById), new { id = sale.Id }, sale);
        }
        catch (BusinessRuleException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An internal error occurred while processing your request." });
        }
    }

    /// <summary>
    /// Gets a sale by ID
    /// </summary>
    /// <param name="id">Sale ID</param>
    /// <returns>Found sale</returns>
    /// <response code="200">Sale found</response>
    /// <response code="404">Sale not found</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<Sale>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _saleService.GetSaleAsync(id);
        if (result == null)
            return NotFound(new { message = $"Sale with ID {id} not found." });

        return Ok(result);
    }

    /// <summary>
    /// Gets all sales with pagination
    /// </summary>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="size">Page size (default: 10)</param>
    /// <returns>List of sales</returns>
    /// <response code="200">List of sales returned successfully</response>
    /// <response code="400">Invalid pagination parameters</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<IEnumerable<Sale>>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        if (page < 1 || size < 1)
            return BadRequest(new { message = "Pagination parameters must be greater than zero." });

        var sales = await _saleService.GetSalesAsync(page, size);
        var totalCount = await _saleService.GetTotalSalesCountAsync();

        var paginatedList = new PaginatedList<Sale>(sales.ToList(), totalCount, page, size);

        var response = new PaginatedResponse<Sale>
        {
            Data = paginatedList,          
            CurrentPage = paginatedList.CurrentPage,
            TotalPages = paginatedList.TotalPages,
            TotalCount = paginatedList.TotalCount,                
            Message = "Sales retrieved successfully."
        };

        return Ok(response);
    }

    /// <summary>
    /// Cancels a sale
    /// </summary>
    /// <param name="id">ID of the sale to be cancelled</param>
    /// <param name="mapper">AutoMapper for object mapping</param>
    /// <returns>Cancelled sale</returns>
    /// <response code="200">Sale cancelled successfully</response>
    /// <response code="404">Sale not found</response>
    /// <response code="409">Sale is already cancelled</response>
    [HttpPut("{id:guid}/cancel")]
    [ProducesResponseType(typeof(ApiResponseWithData<Sale>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Cancel(
        [FromRoute] Guid id,
        [FromServices] IMapper mapper)
    {
        try
        {
            var request = new CancelSaleRequest { SaleId = id };
            var command = mapper.Map<CancelSaleCommand>(request);

            var result = await _saleService.CancelSaleAsync(command.SaleId);
            return Ok(new { message = "Sale cancelled successfully.", data = result });
        }
        catch (BusinessRuleException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An internal error occurred while processing your request." });
        }
    }
}
