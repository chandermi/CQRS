using Microsoft.AspNetCore.Mvc;
using Solution.Dipatcher;
using Solution.Infrastructure.CommandHandler.Customer.CreateCommand;
using Solution.Infrastructure.CommandHandler.Customer.CustomerQueryHandler;
using Solution.Infrastructure.CommandHandler.Customer.DeleteCommand;
using Solution.Infrastructure.CommandHandler.Customer.UpdateCommand;

namespace Solution.Controllers
{
    /// <summary>
    /// This class handles all the customer related operations such as create, update, delete, get customers
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;


        public CustomerController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            _queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));
        }
        
        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <param name="command">The command containing the data for the new customer</param>
        /// <returns>An HTTP status code indicating the result of the operation</returns>
        [HttpPost("CreateCustomer")]
        public IActionResult CreateCustomer(CreateCustomerCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid request");
            }
            try
            {
                _commandDispatcher.Dispatch(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Unable to save customer. {ex.Message}");
            }

            
        }
        /// <summary>
        /// Retrieve a customer based on the customerId
        /// </summary>
        /// <param name="customerId">The id of the customer to retrieve</param>
        /// <returns>An instance of the customer</returns>
        [HttpGet("GetCustomer")]
        public async Task<IActionResult> GetCustomer(int customerId)
        {
            if (customerId == 0)
            {
                throw new ArgumentNullException(nameof(customerId));
            }
            try
            {
                return Ok(await _queryDispatcher.HandleAsync<GetCustomerQuery, Solution.DAL.Customer>(new GetCustomerQuery() { Id = customerId }));
            }
            catch (NullReferenceException ex)
            {
                return BadRequest($"Unable to process your request. {ex.Message}");
            }
        }

        /// <summary>
        /// This method is used to get all the customers
        /// </summary>
        /// <returns>List of customers</returns>
        [HttpGet("GetAllCustomers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                return Ok(await _queryDispatcher.HandleAsync<GetAllCustomerQuery, List<Solution.DAL.Customer>>(new GetAllCustomerQuery()));
            }
            catch (NullReferenceException ex)
            {
                return BadRequest($"Unable to process your request. {ex.Message}");
            }
            
        }

        /// <summary>
        /// Update customer information 
        /// </summary>
        /// <param name="command">The command containing updated customer information</param>
        /// <returns>An HTTP status code indicating the result of the operation</returns>
        [HttpPut("UpdateCustomer")]
        public IActionResult UpdateCustomer(UpdateCustomerCommand command)
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
            catch (Exception ex ) 
            {
                return BadRequest($"Unable to save customer. {ex.Message}");
            }
            
        }

        /// <summary>
        /// Delete the customer using the provided command
        /// </summary>
        /// <param name="command">Command containing the necessary information to delete the customer</param>
        /// <returns>Ok if the customer was deleted successfully or BadRequest if there was an error</returns>
        [HttpDelete("DeleteCustomer")]
        public IActionResult DeleteCustomer(DeleteCustomerCommand command)
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
                return BadRequest($"Unable to delete customer. {ex.Message}");
            }
            
        }
    }
}
