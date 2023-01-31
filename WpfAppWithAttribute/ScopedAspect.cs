using System.Threading.Tasks;
using App.Contracts.BLL;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Serialization;

namespace WpfAppWithAttribute
{
    [PSerializable]
    [MulticastAttributeUsage(MulticastTargets.Method)]
    public class ScopedAspect : MethodInterceptionAspect
    {
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            if (args.Method.IsSpecialName)
            {
                args.Proceed();
                return;
            }

            using (var scope = Ioc.Default.CreateScope())
            {
                ((IBLLViewModel)args.Instance).BLL = scope.ServiceProvider.GetRequiredService<IAppBLL>();
                args.Proceed();
            }
        }

        public override async Task OnInvokeAsync(MethodInterceptionArgs args)
        {
            if (args.Method.IsSpecialName)
            {
                args.Proceed();
                return;
            }

            using (var scope = Ioc.Default.CreateScope())
            {
                ((IBLLViewModel)args.Instance).BLL = scope.ServiceProvider.GetRequiredService<IAppBLL>();
                await args.ProceedAsync();
            }
        }
    }
}