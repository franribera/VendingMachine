using Api.Configuration;
using Api.Identity.Configuration;
using Api.Infrastructure.MiddleWares;
using Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddIdentity(builder.Configuration);
builder.Services.AddValidators();
builder.Services.AddMediatr();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseHttpsRedirection();
app.UseIdentityServer();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<ExceptionHandler>();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetService<VendingMachineDbContext>();
    context?.Database.Migrate();
}

app.Run();