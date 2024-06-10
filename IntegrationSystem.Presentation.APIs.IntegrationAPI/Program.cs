using IntegrationSystem.Application.Interfaces.UseCases;
using IntegrationSystem.Application.UseCases;
using IntegrationSystem.Infrastructure.Persistence.Contexts;
using IntegrationSystem.Infrastructure.Persistence.Interfaces.Repositories;
using IntegrationSystem.Infrastructure.Persistence.Interfaces.UnitOfWorks;
using IntegrationSystem.Infrastructure.Persistence.Repositories;
using IntegrationSystem.Infrastructure.Persistence.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

// Make WebApplication
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContext, MergeUserContext>();
builder.Services.AddTransient<IMergedUserRepository, MergedUserRepository>();
builder.Services.AddTransient<IXmlUserRepository, XmlUserRepository>();
builder.Services.AddTransient<IIntegrationSystemUnitOfWork, IntegrationSystemUnitOfWork>();
builder.Services.AddTransient<IMergedUserUseCase, MergedUserUseCase>();
builder.Services.AddKeyedSingleton("XmlFileLocation", "./TestData.xml");

// Build the App
var app = builder.Build();

// Configure the HTTP request pipeline
app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
