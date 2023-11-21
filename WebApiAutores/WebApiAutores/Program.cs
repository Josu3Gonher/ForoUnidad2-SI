using Microsoft.AspNetCore.Identity;
using WebApiAutores;
using WebApiAutores.Entities;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

var servicioLogger = (ILogger<Startup>)app.Services.GetService(typeof(ILogger<Startup>));

startup.Configure(app, app.Environment, servicioLogger);

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();

	try
	{
		var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
		var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

		await ApplicationDbContextData.LoadDataAsync(userManager, roleManager, loggerFactory);
	}
	catch (Exception e)
	{
		var logger = loggerFactory.CreateLogger<Program>();
		logger.LogError(e, "Error al inicializar datos");
	}
}

app.Run();
