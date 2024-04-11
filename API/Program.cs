using System.Text;
using API;
using API.AssignmentModels.Services;
using API.Authorization;
using API.Authorization.Models;
using API.CacheMemoryService;
using API.EmployeeModels;
using API.JobModels.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var services = builder.Services;

services
    .AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<MyDbContext>()
    .AddDefaultTokenProviders();

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration["Jwt:Secret"]!
            ))

    };

});

services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policyBuilder =>
        {
            policyBuilder.WithOrigins("http://localhost:5173") // Allow specific origin
                .AllowAnyHeader() // Allow all headers
                .AllowAnyMethod(); // Allow all methods
        });
});

services.AddScoped<IEmployeeService, EmployeeService>();
services.AddScoped<IOrderService, OrderService>();
services.AddScoped<IStageService, StageService>();
services.AddScoped<IAssignmentService, AssignmentService>();
services.AddScoped<IRedisCache,RedisCache>();
services.AddScoped<ICacheQueryService, CacheQueryService>();



services.AddScoped<IAccountRepository, AccountRepository>();

//database 

services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// redis

services.AddStackExchangeRedisCache(redisOption =>
{
    var connection = builder.Configuration.GetConnectionString("RedisConnection")!;
    redisOption.Configuration = connection;
});



builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
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



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var accountRepo = scope.ServiceProvider.GetRequiredService<IAccountRepository>();
    var username = builder.Configuration["AdminAccount:Username"];
    var password = builder.Configuration["AdminAccount:Password"];
    var role = builder.Configuration["AdminAccount:Role"];

    await accountRepo.EnsureRole(role!);
    if (!await accountRepo.UsernameExisted(username!))
        await accountRepo.SignUpAsync(new SignUpModel
        {
            Username = username!,
            Password = password!
        });
    await accountRepo.AddRoleToUser(username!, role!);
}




app.UseCors("AllowSpecificOrigin");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



