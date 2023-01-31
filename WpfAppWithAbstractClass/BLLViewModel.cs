using System;
using System.Threading.Tasks;
using App.Contracts.BLL;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace WpfAppWithAbstractClass
{
    public abstract class BLLViewModel : ObservableObject
    {
        private readonly IServiceScopeFactory _scopeFactory;

        protected BLLViewModel(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected IAppBLL BLL { get; private set; }

        protected virtual void Scope(Action action)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                BLL = scope.ServiceProvider.GetRequiredService<IAppBLL>();
                action();
            }
        }

        protected virtual T Scope<T>(Action<T> action) where T : new()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                BLL = scope.ServiceProvider.GetRequiredService<IAppBLL>();
                var result = new T();
                action(result);
                return result;
            }
        }

        protected virtual async Task ScopeAsync(Func<Task> func)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                BLL = scope.ServiceProvider.GetRequiredService<IAppBLL>();
                await func();
            }
        }

        protected virtual async Task<T> ScopeAsync<T>(Func<T, Task<T>> func) where T : new()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                BLL = scope.ServiceProvider.GetRequiredService<IAppBLL>();
                var result = new T();
                await func(result);
                return result;
            }
        }
    }
}