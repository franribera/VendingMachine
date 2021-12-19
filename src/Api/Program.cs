using Api.Configuration;
using Api.Identity.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddIdentity(builder.Configuration);
builder.Services.AddFeatures();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseHttpsRedirection();
app.UseIdentityServer();
app.UseAuthorization();
app.MapControllers();
app.Run();

//using var scope = app.Services.CreateScope();
//var context = scope.ServiceProvider.GetService<VendingMachineDbContext>();
//context?.Database.Migrate();