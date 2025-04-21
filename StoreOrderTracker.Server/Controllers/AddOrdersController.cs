using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StoreOrderTracker.Server.Models;
using StoreOrderTracker.Server.Models.Services;

namespace StoreOrderTracker.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class AddOrdersController : ControllerBase
{
    private readonly ILogger<AddOrdersController> _logger;
    private readonly CustomerOrderRepository _customerOrderRepository;

    
    public AddOrdersController(ILogger<AddOrdersController> logger, CustomerOrderRepository customerOrderRepository)
    {
        _logger = logger;  
        _customerOrderRepository = customerOrderRepository;
    }

    
    [HttpPost]
    public IActionResult Create([FromBody] CustomerOrder customerOrder)
    {
        try
        {
            _customerOrderRepository.AddCustomerOrder(customerOrder);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while adding customer order");
            return StatusCode(500);
        }

        return Ok();
    }
}