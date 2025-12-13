using eCommerce.ProductMicroService.API.APIEndpoints;
using eCommerce.ProductMicroService.API.Middleware;
using eCommerce.ProductsService.BuinessLogicLayer;
using eCommerce.ProductsService.DataAccessLayer;
using FluentValidation;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);

//ADD DAL BAL
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBuinessLogicLayer();

builder.Services.AddControllers();
//FluentValidations
//builder.Services.AddValidatorsFromAssemblyContaining();
//add model binder to read vlues from json to enum
builder.Services.ConfigureHttpJsonOptions(Options => {
    Options.SerializerOptions.Converters.Add(new
    JsonStringEnumConverter());
});

// add swager
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandlingMidleware();    
app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();


app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapProductAPIEndpoints();
app.Run();
