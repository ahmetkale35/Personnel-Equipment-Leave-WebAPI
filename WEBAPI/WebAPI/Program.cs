using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Services.Contracts;
using WebAPI.Extensions;
using WebAPI.Utilities.AutoMapper;

var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.

builder.Services.AddControllers(config =>
{
    config.ReturnHttpNotAcceptable = true; // Return 406 Not Acceptable if the client does not accept the response format
    config.RespectBrowserAcceptHeader = true; // Use the Accept header to determine the response format
    
})
   // .AddXmlDataContractSerializerFormatters()
    .AddApplicationPart(typeof(Presentation.AssemblyRefence).Assembly)
    .AddNewtonsoftJson();


builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    // ModelStateInvalidFilter is set to true by default, which means that if the model state is invalid, it will return a 422 Unprocessable Entity response.
    options.SuppressModelStateInvalidFilter = true;
    
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureLoggerService();


// Configure CORS to allow requests from the React app running on localhost:5173
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});
// Registering IEquipmentService directly, assuming it is implemented in the ServiceManager


// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(Program));


// Configure Identity
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);



var app = builder.Build();
var logger = app.Services.GetRequiredService<ILoggerService>();
app.ConfigureExceptionHandler(logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
