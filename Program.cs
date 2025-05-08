    using System.Text;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using SampleApi.Data;
    using SampleApi.Entity;
    using SampleApi.Repositories;
    using SampleApi.Service;

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddSwaggerGen();
    builder.Services.AddControllers();
    builder.Services.AddScoped<JWTService>();
    builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    builder.Services.AddScoped<IEmployeeService, EmployeeService>();

    var connectionString = builder.Configuration.GetConnectionString("db_employee_net");

    builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
    {
        options.UseNpgsql(connectionString);
    }, ServiceLifetime.Scoped, ServiceLifetime.Singleton);
        
    
    builder.Services.AddSingleton<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
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
            });

    
    //setting bearer otomatis di swagger
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ahmad Raihan training .net Api", Version = "V1" });

        c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Masukkan token JWT Anda"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    }
                },
                Array.Empty<string>()
            }
        });
    });
    
    var app = builder.Build();
    
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
    
    
    // builder.Services.AddSwaggerGen(options =>
    // {
    //     options.SwaggerDoc("v1", new OpenApiInfo { Title = "Ahmad Raihan training .net Api", Version = "V1" });
    //
    //     options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    //     {
    //         Name = "Authorization",
    //         In = ParameterLocation.Header,
    //         Type = SecuritySchemeType.ApiKey,
    //         Scheme = JwtBearerDefaults.AuthenticationScheme,
    //         Description = "JWT Authorization header menggunakan Bearer scheme. Contoh: Authorization: Bearer {token}"
    //     });
    //
    //     options.AddSecurityRequirement(new OpenApiSecurityRequirement
    //     {
    //         {
    //             new OpenApiSecurityScheme
    //             {
    //                 Reference = new OpenApiReference
    //                 {
    //                     Type = ReferenceType.SecurityScheme,
    //                     Id = JwtBearerDefaults.AuthenticationScheme
    //                 },
    //                 Scheme = JwtBearerDefaults.AuthenticationScheme
    //             },
    //             Array.Empty<string>()
    //         }
    //     });
    // });



