using log4net;
using Microsoft.EntityFrameworkCore;
using Solution.DAL;
using Solution.Helper;
using Solution.Infrastructure.CommandHandler.Customer.CustomerQueryHandler;
using Solution.Infrastructure.CommandHandler.Order.Command;
using Solution.Infrastructure.CommandHandler.Order.CustomerQueryHandler;
using Solution.Infrastructure.Repository;
using Solution.Infrastructure.UOW;
using System.Reflection;

namespace Solution.Infrastructure.Service
{
    public class OrderService : IOrderRespository
    {
        private readonly IUnitOfWork _unitOfWork;
        private const string CONST_ORDERBY= "asc";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CreateOrder(CreateOrderCommand command)
        {
            try
            {
                if (!command.Items.Any())
                {
                    throw new ValidationException("No items found");
                }
                var items = command.Items.Where(a => a.Quantity > 0);
                if (items.Any())
                {
                    var order = new Order
                    {
                        CustomerId = command.CustomerId,
                        OrderDate = command.OrderDate,
                        TotalPrice = items.Sum(a => a.Product.Price * a.Quantity),
                        Items = items.Select(i => new Item
                        (
                            new Product() { Name = i.Product.Name, Price = i.Product.Price },
                            i.Quantity
                        )).ToList()
                    };

                    await _unitOfWork.Orders.AddAsync(order);
                    _unitOfWork.SaveChanges();
                    return order.Id;
                }
                else
                {
                    throw new ValidationException("No items found with qunanity > 0");
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Failed to create order", ex.Message);
                return 0;
            }
            
        }

        public async Task<List<Order>> GetOrderByDate(CustomerOrderDateCommandQuery query)
        {
            var orders = await (from order in _unitOfWork.Orders
                                where order.CustomerId == query.customerId
                                select new Order
                                {
                                    CustomerId = order.CustomerId,
                                    Id = order.Id,
                                    Items = (from item in order.Items
                                             select new Item(

                                                  item.Id,
                                                  item.OrderId,
                                                  item.ProductId,
                                                  item.Product,
                                                  item.Quantity
                                             )).ToList(),
                                    OrderDate = order.OrderDate,
                                    TotalPrice = order.TotalPrice,

                                }).ToListAsync();

            if (query.OrderBy == CONST_ORDERBY)
            {
                orders = orders.OrderBy(o => o.OrderDate).ToList();
            }
            else
            {
                orders = orders.OrderByDescending(o => o.OrderDate).ToList();
            }
            return orders;
        }

        public async Task<List<Order>> GetOrders(GetOrderQuery query)
        {
            var orders = await (from order in _unitOfWork.Orders
                                where order.CustomerId == query.customerId
                                select new Order
                                {
                                    CustomerId = order.CustomerId,
                                    Id = order.Id,
                                    Items = (from item in order.Items
                                             select new Item
                                             (
                                                 item.Id,
                                                 item.OrderId,
                                                 item.ProductId,
                                                 item.Product,
                                                 item.Quantity
                                             )).ToList(),
                                    OrderDate = order.OrderDate,
                                    TotalPrice = order.TotalPrice,

                                }).ToListAsync();
            return orders;
        }

       
    }
}
