using KitchenDeliverySystem.Api.ServiceExtensions;
using KitchenDeliverySystem.Infra.Context;
using KitchenDeliverySystem.Infra.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSettings(builder.Configuration);
builder.Services.AddAutoMapper();
builder.Services.AddUseCases();
builder.Services.AddRepositories();
builder.Services.AddDataBase(builder.Configuration);
builder.Services.AddUnitOfWork();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
            .GetBytes(builder.Configuration.GetSection("TokenKey").Value)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

using (IServiceScope _serviceScope = app.Services.CreateScope())
{
    AppDbContext _appDbContext = _serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
    await _appDbContext.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
