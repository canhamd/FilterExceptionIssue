using FilterExceptionIssue.WebApi.Common.Extensions;
using FilterExceptionIssue.WebApi.Common.Filters;
using FilterExceptionIssue.WebApi.GeochronologyFeature.Commands.SaveGeochronology;
using MassTransit;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddFluentValidation(Assembly.GetExecutingAssembly());

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<SaveGeochronologyConsumer>();

    config.UsingInMemory((ctx, cfg) =>
    {
        cfg.UseConsumeFilter(typeof(RequestValidationScopedFilter<>), ctx);

        cfg.ConfigureEndpoints(ctx);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
