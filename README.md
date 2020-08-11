# Generic Repository Pattern for C# .Net Core

Easy to use generic Repository and Unit of Work pattern for Entity Framework .NET Core.

## Installation

The easiest way to install EF.Core.Generic.Data into your solution/project is to use NuGet.:

```bash
    nuget Install-Package EF.Core.Generic.Data
```

Or via the DotNet Cli

```bash
    dotnet add package EF.Core.Generic.Data
```

Check out [Nuget package page](https://www.nuget.org/packages/EF.Core.Generic.Data/) for more details.

## Bugs & Feature requests

If you want to raise bugs or Request a feature please do so via a [Github issue](https://github.com/sidesoftware/EF.Core.Generic.Data/issues) and we will attempt to address it as soon as resource is available to do so.

## Documentation 

Once you have added the Nuget Package to your project, you can edit your `Startup.cs`  and import `using EF.Core.Generic.Data.DependencyInjection;`

```c#
public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Use the EF.Core.Generic.Data Dependency Injection to set up the Unit of Work
            services.AddDbContext<SampleContext>(options => 
            options.UseSqlServer(Configuration.GetConnectionString("NAME OF CONNECTION")))
                .AddUnitOfWork<SampleContext>();

            services.AddMvc();
        }
```
### Example
Below is an example of a simple select. 

```c#

 public class PersonService : IService<Person>
    {
        private readonly IUnitOfWork _db;
        public PersonService(IUnitOfWork db)
        {
            _db = db;
        }
        
        public Person GetAddressint id)
        {
           return _db.Repository<Person>().Get(id);
        }
    }

```