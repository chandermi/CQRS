using Solution.Infrastructure.CommandHandler.Customer.CreateCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Service.Repository
{
    public interface ICustomerRespository
    {
        public void CreateCustomer(CreateCustomerCommand cmd);
        
    }
}
