# EF-Core-Example

This example shows how EF Core Domain, DAL and BLL structures are implemented and how this structure can be utilized by both ASP.NET MVC Web app and WPF app.

There is only one example entity - Address.

```
public class Address : DomainEntityId
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string PostalCode { get; set; }
}
```

In DAL AppUnitOfWork represents an entity of unit of work pattern which takes in DbContext and shares it between the repositories. Also it lazily initilizes them when they are accessed. 

```
public class AppUnitOfWork : BaseUnitOfWork<ExampleDbContext>, IAppUnitOfWork
{
    private readonly IMapper _mapper;

    private IAddressRepository _address;

    public AppUnitOfWork(ExampleDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }

    public virtual IAddressRepository Addresses =>
        _address ?? (_address = new AddressRepository(UowDbContext, new AddressMapper(_mapper)));
}
```

In BLL AppBLL represents an entity of unit of work pattern which takes in AppUnitOfWork and shares its repositories between the services. Also it lazily initilizes them when they are accessed.

```
public class AppBLL : BaseBLL<IAppUnitOfWork>, IAppBLL
{
    private readonly IMapper _mapper;

    private IAddressService _address;

    public AppBLL(IAppUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public virtual IAddressService Addresses =>
        _address ?? (_address = new AddressService(UnitOfWork.Addresses, new AddressMapper(_mapper)));
}
```

Mapping is done between all the layers using AutoMapper and AutoMapperProfile injection.

```
Services.AddAutoMapper(
    typeof(App.DAL.EF.AutoMapperProfile),
    typeof(App.BLL.AutoMapperProfile)
);
```

Both AppUnitOfWork and AppBLL are disposable because DbContext is disposable. When AppBLL is disposed it is written into the console.

WebApp has controller for entity which shows how AppBLL is utilized in ASP.NET.

```
public class AddressController : Controller
{
    private readonly IAppBLL _bll;

    public AddressController(IAppBLL bll)
    {
        _bll = bll;
    }

    // GET: Address
    public async Task<IActionResult> Index()
    {
        return View(await _bll.Addresses.AllAsync());
    }
    
    ...
}
```

WpfApp is an example of the problem with DbContext scoping in WPF. Since WPF does not have ASP.NET middleware pipeline the scope for AppBLL is not defined and AppBLL is effectively singleton. This leads to behavior when the same entity is updated twice then a EF Core "already tracking" exception is thrown.

```
// when called twice on same Address throws exception
public async Task UpdateAddressLine1()
{
    var index = Addresses.IndexOf(SelectedAddress);
    SelectedAddress = _bll.Addresses.Update(SelectedAddress);
    await _bll.SaveChangesAsync();
    Addresses[index] = SelectedAddress;
}
```

Other WpfApp projects show different approaches to solving this problem by introducing scope defining mechanisms to WPF.

WpfAppWithDbContextFactory is the most basic approach using Factory pattern and Transient lifecycle for DbContext and related data structures.

```
public async Task UpdateAddressLine1()
{
    var index = Addresses.IndexOf(SelectedAddress);
    using (var bll = _exampleAppBllFactory.Create())
    {
        SelectedAddress = bll.Addresses.Update(SelectedAddress);
        await bll.SaveChangesAsync();
    }

    Addresses[index] = SelectedAddress;
}
```

WpfAppWithDbContextScope is similar to WpfAppWithDbContextFactory but utilizes scope creation using IServiceScopeFactory instance and transient lifecycle.

```
public async Task UpdateAddressLine1()
{
    var index = Addresses.IndexOf(SelectedAddress);
    using (var scope = Ioc.Default.CreateAsyncScope())
    {
        var bll = scope.ServiceProvider.GetRequiredService<IAppBLL>();
        SelectedAddress = bll.Addresses.Update(SelectedAddress);
        await bll.SaveChangesAsync();
    }

    Addresses[index] = SelectedAddress;
}
```

WpfAppWithAbstractClass uses abstract class BLLViewModel which implements methods with lambda functions as parameter to execute lambda function inside a scope where AppBLL is resolved.

```
public class MainWindowViewModel : BLLViewModel
{
    ...

    public MainWindowViewModel(IServiceScopeFactory scopeFactory) : base(scopeFactory)
    {
        UpdateAddressLine1Command = new AsyncRelayCommand(UpdateAddressLine1);
    }
    
    ...

    public async Task UpdateAddressLine1()
    {
        var index = Addresses.IndexOf(SelectedAddress);
        await ScopeAsync(async () =>
        {
            SelectedAddress = BLL.Addresses.Update(SelectedAddress);
            await BLL.SaveChangesAsync();
        });
        Addresses[index] = SelectedAddress;
    }
}
```

WpfAppWithAttribute uses aspect oriented programming and PostSharp to create very basic class annotation which makes every "not special"(getter, setter etc) method have its own scope with AppBLL resolved in it.

```
[ScopedAspect]
public class MainWindowViewModel : ObservableObject, IBLLViewModel
{
    ...

    public MainWindowViewModel()
    {
        UpdateAddressLine1Command = new AsyncRelayCommand(UpdateAddressLine1);
    }

    ...

    public async Task UpdateAddressLine1()
    {
        var index = Addresses.IndexOf(SelectedAddress);
        SelectedAddress = BLL.Addresses.Update(SelectedAddress);
        await BLL.SaveChangesAsync();
        Addresses[index] = SelectedAddress;
    }
}
```

All of those solutions are not ideal and sadly Microsoft does not provide good documentation on how to use EF Core in WPF.

Accessing ASP.NET HTTP API from WPF is an alternative approach that requires more work to be done.
