using Service;
using Model.Http;
using Model.DB;
using CachingAop.Interceptors;
using CachingAop.DI;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration Configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddSingleton(Configuration);
        SetDiRegistration(services, Configuration);
    }

    private static void SetDiRegistration(IServiceCollection services, IConfiguration Configuration)
    {
        // Register services in service collection
        services.SetSharpCachingAopRegistration(Configuration, true, true);


        services.AddSingleton<IExampleService, ExampleService>();
        services.AddSingleton<IHttpManager, HttpManager>();

        // Inject cacheable services and interceptors
        services.AddInterceptedSingleton<IDbManager, DbManager, AsyncCachingInterceptor>();
    }
}

