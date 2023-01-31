# EF-Core-Example

This example shows how EF Core Domain, DAL and BLL structures are implemented and how this structurer can be utilized by both ASP.NET MVC Web app and WPF app.

There is only one example entity - Address.

In DAL AppUnitOfWork represents an entity of unit of work pattern which takes in DbContext and shares it between the repositories. Also it lazily initilizes them when they are accessed. 

In BLL AppBLL represents an entity of unit of work pattern which takes in AppUnitOfWork and shares it between the services. Also it lazily initilizes them when they are accessed.

Mapping is done between all the layers using AutoMapper and AutoMapperProfile injection.

Both AppUnitOfWork and AppBLL are disposable because DbContext is disposable. When AppBLL is disposed it is written into the console.

WebApp has controller for entity which shows how AppBLL is utilized in ASP.NET.

WpfApp is an example of the problem with DbContext scoping in WPF. Since WPF does not have ASP.NET middleware pipeline the scope for AppBLL is not defined and AppBLL is effectively singleton. This leads to behavior when the same entity is updated twice then a EF Core "already tracking" exception is thrown.

Other WpfApp projects show different approaches to solving this problem by introducing scope defining mechanisms to WPF.

WpfAppWithDbContextFactory is the most basic approach using Factory pattern and Transient lifecycle for DbContext and related data structures.

WpfAppWithDbContextScope is similar to WpfAppWithDbContextFactory but utilizes scope creation using IServiceScopeFactory instance and transient lifecycle.

WpfAppWithAbstractClass uses abstract class BLLViewModel which implements methods with lambda functions as parameter to execute lambda function inside a scope where AppBLL is resolved.

WpfAppWithAttribute uses aspect oriented programming and PostSharp to create very basic class annotation which makes every "not special"(getter, setter etc) method have its own scope with AppBLL resolved in it.

All of those solutions are not ideal and sadly Microsoft does not provide good documentation on how to use EF Core in WPF.

Accessing ASP.NET HTTP API from WPF is an alternative approach that requires more work to be done.
