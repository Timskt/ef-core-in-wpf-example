using App.Contracts.BLL;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace WpfAppWithDbContextFactory
{
    public class ExampleAppBLLFactory
    {
        public IAppBLL Create()
        {
            return Ioc.Default.GetRequiredService<IAppBLL>();
        }
    }
}