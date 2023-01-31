using System.Windows;
using App.BLL;
using App.Contracts.BLL;
using App.Contracts.DAL;
using App.DAL.EF;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BLLAutoMapperProfile = App.BLL.AutoMapperProfile;
using DALAutoMapperProfile = App.DAL.EF.AutoMapperProfile;

namespace WpfAppWithAttribute
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureServices();
        }

        private static void ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ExampleDbContext>(
                options => { options.UseSqlite(@"Data Source=..\..\..\WebApp\ExampleDatabase.db"); });

            services.AddScoped<IAppUnitOfWork, AppUnitOfWork>();

            services.AddScoped<IAppBLL, AppBLL>();

            services.AddAutoMapper(
                typeof(DALAutoMapperProfile),
                typeof(BLLAutoMapperProfile)
            );

            services.AddTransient<MainWindowViewModel>();

            Ioc.Default.ConfigureServices(services.BuildServiceProvider());
        }
    }
}