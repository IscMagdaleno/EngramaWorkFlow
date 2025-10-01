using EngramaCoreStandar.Extensions;

using System.Reflection;

using WorkFlow.API.EngramaLevels.Dominio.Core;
using WorkFlow.API.EngramaLevels.Dominio.Interfaces;
using WorkFlow.API.EngramaLevels.Dominio.Servicios;
using WorkFlow.API.EngramaLevels.Infrastructure.Interfaces;
using WorkFlow.API.EngramaLevels.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddScoped<ITestRepository, TestRepository>();
builder.Services.AddScoped<IPlanesRepository, PlanesRepository>();
builder.Services.AddScoped<IProcesoRepository, ProcesoRepository>();
builder.Services.AddScoped<ChatMemoryService>();

builder.Services.AddScoped<IProcesoDominio, ProcesoDominio>();
builder.Services.AddScoped<ITestDominio, TestDominio>();
builder.Services.AddScoped<IPlanesDominio, PlanesDominio>();


builder.Services.AddScoped<IAzureIAService, AzureIAService>();
builder.Services.AddScoped<ILLMModuleGenerator, LLMModuleGenerator>();
// Asegúrate de que appsettings.json tenga:


// Ensure the AddEngramaDependenciesAPI method is defined in the above namespace
builder.Services.AddEngramaDependenciesAPI();



/*Swagger configuration*/
builder.Services.AddSwaggerGen(options =>
{
	// using System.Reflection;
	var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var path = Path.Combine(AppContext.BaseDirectory, xmlFilename);
	options.IncludeXmlComments(path);

});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors(x => x
					.AllowAnyMethod()
					.AllowAnyHeader()
					.SetIsOriginAllowed(origin => true) // allow any origin
					.AllowCredentials());


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
