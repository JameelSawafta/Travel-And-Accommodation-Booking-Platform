using System.Text;
using Asp.Versioning;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using PasswordHashing;
using PaymentGateway;
using PayPal.Api;
using TokenGenerator;
using TravelAndAccommodationBookingPlatform.API.Controllers;
using TravelAndAccommodationBookingPlatform.API.Middlewares;
using TravelAndAccommodationBookingPlatform.Db.DbContext;
using TravelAndAccommodationBookingPlatform.Db.DbServices;
using TravelAndAccommodationBookingPlatform.Db.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Services;

namespace TravelAndAccommodationBookingPlatform.CompositionRoot;

public class Program
{
    public static void Main(string[] args)
    {
        Env.Load();
        
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Authentication:Issuer"], 
                    ValidAudience = builder.Configuration["Authentication:Audience"], 
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
                };
            });
        
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireClaim("Role", UserRole.Admin.ToString()));
        
            options.AddPolicy("UserOrAdmin", policy =>
                policy.RequireAssertion(context =>
                    context.User.HasClaim(c => 
                        c.Type == "Role" && 
                        (c.Value == UserRole.Admin.ToString() || c.Value == UserRole.User.ToString()))));
        });

        builder.Services.AddControllers().AddApplicationPart(typeof(AuthController).Assembly);
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var xmlFile = $"{assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            }
            
            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your token in the text input below.<br> Example: 'Bearer 12345abcdef'"
            });
            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });

        builder.Services.AddDbContext<TravelAndAccommodationBookingPlatformDbContext>(
            dbContext => dbContext.UseNpgsql(builder.Configuration["ConnectionStrings:constr"])
            );
        
        builder.Services.AddApiVersioning(
            setupAction =>
            {
                setupAction.ReportApiVersions = true;
                setupAction.AssumeDefaultVersionWhenUnspecified = true;
                setupAction.DefaultApiVersion =
                    ApiVersion.Default; 
                setupAction.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            }
        );
        
        builder.Services.AddHttpContextAccessor();
        
        builder.Services.AddSingleton<APIContext>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var config = new Dictionary<string, string>
            {
                { "clientId", Environment.GetEnvironmentVariable("PAYPAL_CLIENT_ID") },
                { "clientSecret", Environment.GetEnvironmentVariable("PAYPAL_CLIENT_SECRET") },
                { "mode", configuration["paypal:Mode"] }
            };
        
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            return new APIContext(accessToken) { Config = config };
        });
        
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddTransient<ITokenGeneratorService, JwtGeneratorService>();
        builder.Services.AddTransient<IPasswordService, Argon2PasswordService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IHotelRepository, HotelRepository>();
        builder.Services.AddScoped<IHotelService, HotelService>();
        builder.Services.AddScoped<IBookingRepository, BookingRepository>();
        builder.Services.AddScoped<IBookingService, BookingService>();
        builder.Services.AddScoped<ICityRepository, CityRepository>();
        builder.Services.AddScoped<ICityService, CityService>();
        builder.Services.AddScoped<IRoomRepository, RoomRepository>();
        builder.Services.AddScoped<IRoomService, RoomService>();
        builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
        builder.Services.AddScoped<ICartRepository, CartRepository>();
        builder.Services.AddScoped<ICartService, CartService>();
        
        builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
        builder.Services.AddScoped<IPaymentGatewayService, PayPalGatewayService>();
        builder.Services.AddScoped<IPaymentService, PaymentService>();
        
        builder.Services.AddScoped<IPaginationService, PaginationService>();
        
        
        
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        
        var app = builder.Build();
        
        app.UseMiddleware<CustomExceptionHandlingMiddleware>();
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        
        app.UseRouting();
        
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        app.Run();
    }
}