using Microsoft.Extensions.Options;
using TelephoneDirectory.PersonContact.Repository.Configurations;
using TelephoneDirectory.PersonContact.Service.Services.Abstracts;
using TelephoneDirectory.PersonContact.Service.Services.Concretes;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPersonContactInfoService, PersonContactInfoService>();

builder.Services.Configure<DatabaseSettings>(configuration.GetSection("DatabaseSettings"));
builder.Services.AddSingleton<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.Run();
