using Application;
using Identity;
using Identity.Models;
using Identity.Seeds;
using Microsoft.AspNetCore.Identity;
using Persistence;
using Shared;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationLayer();
builder.Services.AddSharedInfraestructure(builder.Configuration);
builder.Services.AddPersistenceInfraestructure(builder.Configuration);
builder.Services.AddIdentityInfraestructure(builder.Configuration);
builder.Services.AddApiVersioningExtension();

var app = builder.Build();

Main(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseErrorHandlerMiddleware();

app.MapControllers();

app.Run();

async static void Main(WebApplication app)
{
    try
    {
        using (var scope = app.Services.CreateScope())
        {

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await DefaultRoles.SeedAsync(userManager, roleManager);
            await DefaultAdminUser.SeedAsync(userManager, roleManager);
            await DefaultBasicUser.SeedAsync(userManager, roleManager);
        }
    }
    catch
    {
        throw;
    }
}