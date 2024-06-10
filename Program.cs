using System.Text.Json.Serialization;
using commerce_tracker_v2.Data;
using commerce_tracker_v2.Interfaces;
using commerce_tracker_v2.Services;
using dotnet_backend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    DotNetEnv.Env.Load();

    // var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
    // var dbName = Environment.GetEnvironmentVariable("DB_NAME");
    // var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
    // var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword};TrustServerCertificate=True;";
    // Console.WriteLine(connectionString);


    // builder.Services.AddDbContext<DataContext>(options =>
    // options.UseSqlServer(Environment.GetEnvironmentVariable("CONNECTION_STRING"))
    // );

    var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
    var serverVersion = new MySqlServerVersion(new Version(8, 0));
    Console.WriteLine(serverVersion);
    builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(connectionString, serverVersion)
    );

    builder.Services.AddControllers()
        .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve); //Prevents cyclical returns from many to many relationships
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddSwaggerGen(option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "Commerce API", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });

        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });
    });

    builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 12;

    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<DataContext>();

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultForbidScheme =
        options.DefaultScheme =
        options.DefaultSignInScheme =
        options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
            // ValidIssuer = "http://localhost:5246",
            ValidateAudience = true,
            ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
            // ValidAudience = "http://localhost:5246",
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                // System.Text.Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SIGNING_KEY"))
                System.Text.Encoding.UTF8.GetBytes("ko3ruirwr9wur9hwr89w47uhr94rh4w38rhgeg55e4ge5g5e4ge454etete4wrwrwrw3rw3r9rtwfs44sfhrtw")
            )
        };
    });

    builder.Services.AddScoped<ITokenService, TokenService>();



}


var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    // app.UseHttpsRedirection();

    app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .WithOrigins("https://commerce-api-dotnet.azurewebsites.net/") //For Deployment
        .SetIsOriginAllowed(origin => true));

    app.UseAuthentication();
    app.UseAuthorization();



    app.MapControllers();

    // using (var scope = app.Services.CreateScope())
    // {
    //     var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    //     var roles = new[] { "Admin", "User" };


    // }

    app.Run();
}