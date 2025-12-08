using eCommerce.ProductsService.DataAccessLayer;
using eCommerce.ProductsService.BuinessLogicLayer;
using eCommerce.ProductMicroService.API.Middleware;
var builder = WebApplication.CreateBuilder(args);

//ADD DAL BAL
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBuinessLogicLayer();

builder.Services.AddControllers();  

var app = builder.Build();

app.UseExceptionHandlingMidleware();    
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
