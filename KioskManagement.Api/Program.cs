using Autofac;
using Autofac.Extensions.DependencyInjection;
using KioskManagement.Common.Loggings;
using KioskManagement.Data;
using KioskManagement.Data.Repositories;
using KioskManagement.Service;
using KioskManagement.WebApi.Infrastructure.Extentsions;
using KioskManagement.WebApi.Services;


var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new ContainerModule());
    builder.RegisterInstance(AutoMapperConfig.Initialize()).SingleInstance();
    //builder.RegisterType<MongoContext>().AsSelf().As(x => x.GetInterfaces().FirstOrDefault(t => t.Name.EndsWith("Context")));
    // builder.RegisterType<MongoContext>().As<IMongoContext>().WithAttributeFiltering();

    builder.RegisterAssemblyTypes(typeof(ApplicationGroupRepository).Assembly)
          .Where(t => t.Name.EndsWith("Repository"))
           .As(serviceMapping: x => x.GetInterfaces().FirstOrDefault(t => t.Name.EndsWith("Repository")));
    builder.RegisterAssemblyTypes(typeof(ApplicationGroupService).Assembly)
       .Where(t => t.Name.EndsWith("Service"))
       .As(x => x.GetInterfaces().FirstOrDefault(t => t.Name.EndsWith("Service")));
}).ConfigureServices(services =>
{
    services.AddAutofac();
});

RoundTheCodeFileLoggerOption loggerOption = new RoundTheCodeFileLoggerOption();
builder.Services.Configure<RoundTheCodeFileLoggerOption>(builder.Configuration.GetSection("Logging").GetSection("RoundTheCodeFile").GetSection("Options"));
//builder.Configuration.GetSection("Logging").GetSection("RoundTheCodeFile").GetSection("Options").Bind(loggerOption);
builder.Services.AddSingleton<ILoggerProvider, RoundTheCodeFileLoggerProvider>();
// add worker services
builder.Services.AddHostedService<Worker>();
// Call ConfigureContainer on the Host sub property

//builder.Logging.AddConsole();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore).AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddIdentity();
builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddJwtAuthentication(builder.Services.GetApplicationSettings(builder.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiDemo v1");
        c.RoutePrefix = "swagger";
    });
}
app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader())
                .UseAuthentication();

app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseStaticFiles();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiDemo v1");
    c.RoutePrefix = "swagger";
});
app.UseStaticFiles();
app.MapControllers();
app.UseStaticFiles();
app.Run();




