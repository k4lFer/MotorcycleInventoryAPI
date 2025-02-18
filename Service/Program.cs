using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Service.Helper;
using System.Text;
using CloudinaryDotNet;
using DataTransferLayer.OtherObject;
using BusinessLayer.ExternalApi;
using Microsoft.Extensions.Options;
using Resend;
using BusinessLayer.Business.User;
using Service.Generic;
using BusinessLayer.Business.Brands;
using BusinessLayer.Business.Services;
using BusinessLayer.Business.Sale;
using BusinessLayer.Business.Motorcycle;

namespace Service
    {
        public class Program
        {
            public static void Main(string[] args)
            {
                WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
                builder.Services.AddScoped<BusinessUser>();
                builder.Services.AddScoped<BusinessFactory>();
                builder.Services.AddScoped<BusinessBrand>();
                builder.Services.AddScoped<BusinessServices>();
                builder.Services.AddScoped<BusinessSales>();
                builder.Services.AddScoped<BusinessMotorcycle>();
                
                /*
                WebApplicationBuilder builder = WebApplication.CreateBuilder(new WebApplicationOptions
                {
                    EnvironmentName = Environments.Production
                });
                */
                
                #region appsettings
                AppSettings.Init();
                #endregion
                
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
                
                builder.Services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    });

                #region CORS
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowOnlyDefaults",
                        policy =>
                        {
                            policy.WithOrigins(AppSettings.GetOriginRequest().Split(','))
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowAnyOrigin()
                                .SetIsOriginAllowedToAllowWildcardSubdomains();
                        });
                });
                #endregion
                
                #region JWT
                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(jwtBearerOptions =>
                {
                    jwtBearerOptions.SaveToken = true;
                    jwtBearerOptions.RequireHttpsMetadata = true;
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = AppSettings.GetOriginIssuer(),
                        ValidAudience = AppSettings.GetOriginAudience(),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.GetAccessJwtSecret())),
                        ClockSkew = TimeSpan.Zero,
                    };
                });
                builder.Services.AddAuthorization();
                #endregion
                
                #region authentication to Swagger UI
                builder.Services.AddSwaggerGen(options =>
                {
                    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        Description = "Ingrese 'Bearer' [espacio] y luego el token en el campo de texto. Ejemplo: 'Bearer abc123'",
                    });
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "API de Control de Inventario (AppInventoryMotors)",
                        Description = "Esta API permite gestionar el control de inventario de una tienda que vende motocicletas y brinda servicios relacionados.",
                        
                        Contact = new OpenApiContact
                        {
                            Name = "Collaborators",
                            Url = new Uri("https://github.com/k4lFer/MotorcycleInventoryAPI"),
                        },
                        License = new OpenApiLicense
                        {
                            Name = "License",
                            Url = new Uri("https://github.com/k4lFer")
                        }
                    });
                    options.OperationFilter<SecurityRequirementsOperationFilter>();
                });
                #endregion

                #region Cloudinary
                var cloudinarySettings = builder.Configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();
                var account = new Account(
                    cloudinarySettings.CloudName,
                    cloudinarySettings.ApiKey,
                    cloudinarySettings.ApiSecret
                );
                var cloudinary = new Cloudinary(account)
                {
                    Api = { Secure = true }
                };
                CloudinaryService.Initialize(cloudinary);
                #endregion

               /* 
                #region Resend
                builder.Services.AddOptions();
                builder.Services.Configure<ResendClientOptions>(options =>
                {
                    options.ApiToken = builder.Configuration["Resend:Password"];
                });
                builder.Services.AddHttpClient<ResendClient>();
                #endregion
                */
                builder.Services.AddHttpClient();
                
                #region ApisNetPe
                builder.Services.AddScoped<ApisNetPe>();
                #endregion
                
                WebApplication app = builder.Build();
                
               /* #region Inicializar ResendAPI
                var resendClient = app.Services.GetRequiredService<ResendClient>();
                ResendApi.Initialize(resendClient);
                #endregion*/
                
                app.UseSwagger(options => { options.SerializeAsV2 = true; });
                app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"); });
                
                app.UseHttpsRedirection();
                app.UseCors("AllowOnlyDefaults");
                app.UseAuthentication();
                app.UseAuthorization();
                
                app.MapControllers();

                app.Run();
            }
        }
    }