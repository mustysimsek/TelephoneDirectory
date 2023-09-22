using System;
using MassTransit;
using Microsoft.Extensions.Options;
using TelephoneDirectory.ContactReport.Repository.Configurations;
using TelephoneDirectory.ContactReport.Service.Services.Abstracts;
using TelephoneDirectory.ContactReport.Service.Services.Concretes;
using TelephoneDirectory.Shared.Interfaces;
using TelephoneDirectory.Shared.Services;

var builder = WebApplication.CreateBuilder(args);
#region AddMassTransit
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ReportService>();
    //Default Port : 5672
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQUrl"], "/", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });
        cfg.UseConcurrencyLimit(1);
        cfg.ReceiveEndpoint("create-report-service", e =>
        {
            e.PrefetchCount = 5;
            e.UseMessageRetry(r => r.Interval(5, 100));
            e.ConfigureConsumer<ReportService>(context);
        });
    });
});
#endregion
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();


builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddSingleton<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});

builder.Services.AddScoped(typeof(IRabbitMqService), typeof(RabbitMqService));
builder.Services.AddScoped<IReportService, ReportService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();