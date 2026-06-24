using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application.Exceptions;
using Tabsareh.Infrastructure.Config;
using Tabsareh.Infrastructure.Persistance;
using Tabsareh.Infrastructure.Persistance.Seed;
using System.Net;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var pepper = builder.Configuration["Security:Pepper"];

// ================= JWT =================
var signingKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SigningKey"]!));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.UseSecurityTokenValidators = true;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30),
            NameClaimType = System.Security.Claims.ClaimTypes.NameIdentifier
        };
    });

// ================= Authorization =================
builder.Services.AddAuthorization(options =>
{
    var permissions = new[]
    {
        "view_dashboard", "manage_admins", "manage_roles",
        "manage_teachers", "manage_content_owners"
    };
    foreach (var perm in permissions)
        options.AddPolicy(perm, policy => policy.RequireClaim("permission", perm));
});

// ================= Controllers =================
builder.Services.AddControllers(a =>
{
    a.Conventions.Add(new CqrsModelConvention());
});

// ================= Autofac =================
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    containerBuilder.RegisterModule(new AutofacModule(builder.Configuration)));

// ================= SQL Server (EF Core) =================
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Missing ConnectionStrings:Default");

builder.Services.AddDbContext<TabsarehDbContext>(options =>
    options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure()));

// ================= Seeder =================
builder.Services.AddHostedService<DataSeeder>();

// ================= Swagger =================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Tabsareh API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your token}"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
    });
});

// ================= Misc =================
builder.Services.AddHttpContextAccessor();
builder.Services.AddHealthChecks();
builder.Services.AddCors(options =>
{
    options.AddPolicy("OpenCors", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// ================= Exception Handling =================
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        var errorCode = "ERR_500";
        var errorMessage = "There is a UnKnow Error. Please Call Support Team.";
        var statusCode = HttpStatusCode.InternalServerError;

        if (exception is NotFoundException)
        {
            errorCode = "ERR_404";
            errorMessage = exception.Message;
            statusCode = HttpStatusCode.NotFound;
        }
        else if (exception is UserAccessException)
        {
            errorCode = "ERR_403";
            errorMessage = exception.Message;
            statusCode = HttpStatusCode.Forbidden;
        }
        else if (exception is ValidationException validationException)
        {
            errorCode = "ERR_400";
            errorMessage = string.Join(", ", validationException.Errors);
            statusCode = HttpStatusCode.BadRequest;
        }

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            ErrorCode = errorCode,
            ErrorMessage = errorMessage
        }));
    });
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tabsareh API");
    c.RoutePrefix = "swagger";
});

if (string.IsNullOrWhiteSpace(pepper))
    throw new InvalidOperationException("Missing Security:Pepper");

// ================= Static Files (uploaded images) =================
Directory.CreateDirectory(Path.Combine(app.Environment.ContentRootPath, "wwwroot", "uploads"));
app.UseStaticFiles();

app.UseCors("OpenCors");
app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();
