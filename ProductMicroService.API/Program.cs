using eCommerce.ProductsService.DataAccessLayer;
using eCommerce.ProductsService.BuinessLogicLayer;
using eCommerce.ProductMicroService.API.Middleware;
using eCommerce.ProductMicroService.API.APIEndpoints;
using FluentValidation;
var builder = WebApplication.CreateBuilder(args);

//ADD DAL BAL
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBuinessLogicLayer();

builder.Services.AddControllers();
//FluentValidations
//builder.Services.AddValidatorsFromAssemblyContaining();
var app = builder.Build();

app.UseExceptionHandlingMidleware();    
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapProductAPIEndpoints();
app.Run();
