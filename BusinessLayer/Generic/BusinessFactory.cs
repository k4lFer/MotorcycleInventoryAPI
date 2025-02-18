using Microsoft.Extensions.DependencyInjection;

namespace Service.Generic
{
    public class BusinessFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public BusinessFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T CreateBusiness<T>() where T : class
        {
            return _serviceProvider.GetRequiredService<T>();
        }
    }
}