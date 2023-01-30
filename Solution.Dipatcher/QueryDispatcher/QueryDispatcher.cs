using Solution.Helper;
using Solution.Infrastructure;

namespace Solution.Dipatcher
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public async Task<TResult> HandleAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            var service = this._serviceProvider.GetService(typeof(IQueryHandler<TQuery, TResult>)) as IQueryHandler<TQuery, TResult>;
            if (service == null)
            {
                throw new NotFoundException($"Command doesn't have any handler {query.GetType().Name}");
            }
            return await service.HandleAsync(query);
        }
    }
}
