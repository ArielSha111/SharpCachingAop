using Service;
using Model.Http;
using Model.DB;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddSingleton(configuration);
        SetDiRegistration(services);
    }

    private static void SetDiRegistration(IServiceCollection services)
    {
        services.AddSingleton<IExampleService, ExampleService>();
        services.AddSingleton<IHttpManager, HttpManager>();
        services.AddSingleton<IDbManager, DbManager>();
    }
}

