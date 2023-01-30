using Microsoft.AspNetCore.Mvc;
using Solution.Dipatcher;
using Solution.Infrastructure.CommandHandler.Customer.CustomerQueryHandler;
using Solution.Infrastructure.CommandHandler.Order.Command;
using Solution.Infrastructure.CommandHandler.Order.CustomerQueryHandler;


namespace Solution.Controllers
{
    /// <summary>
    /// This class handles all the customer related operations such as create, update, delete, get orders
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;


        public OrderController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            _queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));
        }

        /// <summary>
        /// Creates a new order for a customer.
        /// </summary>
        /// <param name="command">The command containing the information for the new order.</param>
        /// <returns>200 OK if the order is successfully created, 400 Bad Request if there is an error.</returns>
        [HttpPost("CreateOrder")]
        public IActionResult CreateOrder(CreateOrderCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            try
            {
                _commandDispatcher.Dispatch(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Unable to save order. {ex.Message}");
            }
        }


        /// <summary>
        /// This method is used to get all the orders
        /// </summary>
        /// <returns>List of orders</returns>
        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrder(int customerId)
        {
            if (customerId == 0)
            {
                throw new ArgumentNullException(nameof(customerId));
            }
            try
            {
                return Ok(await _queryDispatcher.HandleAsync<GetOrderQuery, List<Solution.DAL.Order>>(new GetOrderQuery() { customerId = customerId }));
            }
            catch (Exception ex)
            {
                return BadRequest($"Unable to process your request. {ex.Message}");
            }
        }

        /// <summary>
        /// This method is used to get all the orders
        /// </summary>
        /// <returns>List of orders</returns>
        [HttpGet("GetOrdersIterateByDate")]
        public async Task<IActionResult> GetOrdersIterateByDate(int customerId, string orderby = "asc")
        {
            if (customerId == 0)
            {
                throw new ArgumentNullException(nameof(customerId));
            }
            try
            {
                return Ok(await _queryDispatcher.HandleAsync<CustomerOrderDateCommandQuery, List<Solution.DAL.Order>>(new CustomerOrderDateCommandQuery() { customerId = customerId, OrderBy = orderby }));
            }
            catch (Exception ex)
            {
                return BadRequest($"Unable to process your request. {ex.Message}");
            }
        }

    }
}
