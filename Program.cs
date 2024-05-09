using System.Text.Json.Serialization;
using commerce_tracker_v2.Data;
using dotnet_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
{
    DotNetEnv.Env.Load();

    builder.Services.AddDbContext<DataContext>(options =>
        options.UseSqlServer(Environment.GetEnvironmentVariable("CONNECTION_STRING"))
    );

    builder.Services.AddControllers()
        .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve); //Prevents cyclical returns from many to many relationships
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 12;

    })
    .AddEntityFrameworkStores<DataContext>();
}


var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();

    app.MapControllers();

    // using (var scope = app.Services.CreateScope())
    // {
    //     var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    //     var roles = new[] { "Admin", "User" };


    // }

    app.Run();
}