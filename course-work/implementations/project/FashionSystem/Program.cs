using FashionSystem.Data;
using FashionSystem.Entities;
using FashionSystem.Middleware;
using FashionSystem.Repository;
using FashionSystem.Services;
using FashionSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace FashionSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Connection String
            var connectionString =
                "Server=.;Database=FashionSystemDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;";

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy
                            .WithOrigins(
                              "https://localhost:7268",
                              "http://localhost:5151"
                            )
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            // Add Controllers
            builder.Services.AddControllers();

            // JWT Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });

            // Swagger
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
            });

            // DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Repository
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IFashionItemService, FashionItemService>();
            builder.Services.AddScoped<IRentalService, RentalService>();

            var app = builder.Build();

            // Seed Data
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider
                    .GetRequiredService<AppDbContext>();

                context.Database.Migrate();

                if (!context.FashionItems.Any())
                {
                    context.FashionItems.AddRange(
                        new FashionItem
                        {
                            Name = "Brown Dress",
                            Designer = "Gucci",
                            Category = "Dress",
                            Style = "Elegant",
                            Size = "M",
                            PricePerDay = 500,
                            IsAvailable = true,
                            ImageUrl = "https://tse1.mm.bing.net/th/id/OIP.3JfIirxhvFqaDyIdALzgnAHaIb?rs=1&pid=ImgDetMain&o=7&rm=3",
                            CreatedAt = DateTime.UtcNow
                        },
                        new FashionItem
                        {
                            Name = "Black Handbag",
                            Designer = "Prada",
                            Category = "Bags",
                            Style = "Luxury",
                            Size = "One Size",
                            PricePerDay = 350,
                            IsAvailable = true,
                            ImageUrl = "https://www.prada.com/content/dam/pradabkg_products/1/1BC/1BC183/2DF0F0002/1BC183_2DF0_F0002_V_OOO_SLF.jpg/_jcr_content/renditions/cq5dam.web.hebebed.1800.1800.jpg",
                            CreatedAt = new DateTime(2026, 5, 20)
                        },

                        new FashionItem
                        {
                            Name = "White Sneakers",
                            Designer = "Dior",
                            Category = "Shoes",
                            Style = "Casual",
                            Size = "42",
                            PricePerDay = 200,
                            IsAvailable = true,
                            ImageUrl = "https://assets.christiandior.com/is/image/diorprod/3SN279ZEIH006_E02?$default_GH$&crop=282,829,1439,682&bfc=on&qlt=85",
                            CreatedAt = new DateTime(2026, 5, 20)
                        },

                       new FashionItem
                       {
                            Name = "High Heels",
                            Designer = "Gucci",
                            Category = "Shoes",
                            Style = "Elegant",
                            Size = "38",
                            PricePerDay = 100,
                            IsAvailable = true,
                            ImageUrl = "https://media.gucci.com/style/DarkGray_Center_0_0_600x314/1704821458/782628_2HK80_9763_001_090_0000_Light-Womens-GG-canvas-slingback-pump.jpg",
                            CreatedAt = new DateTime(2026, 5, 20)
                       },  

                       new FashionItem
                       {
                            Name = "Cat-eye Sunglasses",
                            Designer = "Loewe",
                            Category = "Sunglasses",
                            Style = "Casual",
                            Size = "One Size",
                            PricePerDay = 80,
                            IsAvailable = true,
                            ImageUrl = "https://www.net-a-porter.com/variants/images/1647597322468379/fr/w2000_q60.jpg",
                            CreatedAt = new DateTime(2026, 5, 20)
                        },

                       new FashionItem
                       {
                             Name = "Oversized Men's Suit",
                             Designer = "Balenciaga",
                             Category = "Suits",
                             Style = "Luxury",
                             Size = "M",
                             PricePerDay = 320,
                             IsAvailable = true,
                             ImageUrl = "https://tse2.mm.bing.net/th/id/OIP.q_K0YZaJtbAPQXFZzA3X_wHaIX?rs=1&pid=ImgDetMain&o=7&rm=3",
                             CreatedAt = new DateTime(2026, 5, 20)
                       },

                       new FashionItem
                       {
                             Name = "Hermes Birkin Etain Togo",
                             Designer = "Hermes",
                             Category = "Bags",
                             Style = "Luxury",
                             Size = "30",
                             PricePerDay = 50,
                             IsAvailable = true,
                             ImageUrl = "https://madisonavenuecouture.com/cdn/shop/products/H-B-110521-2-02.jpg?v=1651093028",
                             CreatedAt = new DateTime(2026, 5, 20)
                       },
                       new FashionItem
                       {
                             Name = "Versace Dominus Men's Watch",
                             Designer = "Versace",
                             Category = "Wathces",
                             Style = "Casual",
                             Size = "42mm",
                             PricePerDay = 80,
                             IsAvailable = true,
                             ImageUrl = "https://media.neimanmarcus.com/f_auto,q_auto:low,ar_4:5,c_fill,dpr_2.0,w_790/01/nm_4528508_100130_m",
                             CreatedAt = new DateTime(2026, 5, 20)
                       },

                       new FashionItem
                       {
                             Name = "Loro Piana Men's Loafers",
                             Designer = "Loro Piana",
                             Category = "Shoes",
                             Style = "Casual",
                             Size = "43",
                             PricePerDay = 120,
                             IsAvailable = true,
                             ImageUrl = "https://cdna.lystit.com/photos/mytheresa/4577cb1b/loro-piana-Beige-Mocassini-Summer-Charms-Walk.jpeg",
                             CreatedAt = new DateTime(2026, 5, 20)
                       },
                       new FashionItem
                       {
                             Name = "Bottega Veneta Party Dress",
                             Designer = "Bottega Veneta",
                             Category = "Dress",
                             Style = "Party",
                             Size = "L",
                             PricePerDay = 75,
                             IsAvailable = true,
                             ImageUrl = "https://tse4.mm.bing.net/th/id/OIP.s2PQfsPUKnn2LMPfWCf3JAHaIu?rs=1&pid=ImgDetMain&o=7&rm=3",
                             CreatedAt = new DateTime(2026, 5, 20)
                       },
                       new FashionItem
                       {
                             Name = "Saint Laurent Lulu Mini Dress",
                             Designer = "Saint Laurent",
                             Category = "Dress",
                             Style = "Elegant",
                             Size = "S",
                             PricePerDay = 60,
                             IsAvailable = true,
                             ImageUrl = "https://cdn.yoox.biz/34/34446746BU_13_f.jpg",
                             CreatedAt = new DateTime(2026, 5, 20)
                       }
                    );
                    context.SaveChanges();
                }
            }
            // Global Exception Middleware
            app.UseMiddleware<GlobalExceptionMiddleware>();

            // Swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // HTTPS
            app.UseHttpsRedirection();

            // CORS
            app.UseCors("AllowFrontend");

            // Static Files
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Authentication & Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // Controllers
            app.MapControllers();

            app.Run();
            Console.WriteLine(connectionString);
        }
    }
}