using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetCore.AutoRegisterDi;
using ServerApi.DTO;
using ServerApi.DTO.Comment;
using ServerApi.DTO.Pin;
using ServerApi.Extensions;
using ServerApi.Persistance;
using ServerApi.Persistance.Models;
using ServerApi.Repositories;
using ServerApi.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterAssemblyPublicNonGenericClasses(typeof(UserService).Assembly).Where(x => x.Name.EndsWith("Service")).AsPublicImplementedInterfaces();
builder.Services.RegisterAssemblyPublicNonGenericClasses(typeof(UserRepository).Assembly).Where(x => x.Name.EndsWith("Repository")).AsPublicImplementedInterfaces();

builder.Services.AddDbContext<AppDbContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("AppDb")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }}, new List<string>()}
    });
});

builder.Services.AddControllers().AddAndConfigureFluentValidation();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");//.RequireAuthorization();//.Produces<Scooter>().RequireAuthorization()//.AllowAnonymous();

app.MapGet("/pins", () => new { id = 0 }).Produces<List<Pin>>();

app.MapGet("/pin-details", () => new { id = 0 }).Produces<Pin>();

app.MapGet("/account", (int id, IUserService service, CancellationToken cancellationToken) => service.GetUser(id, cancellationToken)).Produces<User>();

app.MapPost("/pin", async (AddPinRequest pin, IPinService service, CancellationToken cancellationToken) => 
    await service.AddPinAsync(pin, cancellationToken));

app.MapPost("/login", async (LoginRequest login, IUserService service, CancellationToken cancellationToken) => 
    await service.Login(login, builder.Configuration, cancellationToken)).AllowAnonymous();

app.MapPost("/register", async (RegisterRequest register, IUserService service, CancellationToken cancellationToken) => 
    await service.Register(register, cancellationToken)).AllowAnonymous();

app.MapPost("/comment", (AddCommentRequest comment) => {});

app.MapPut("/pin", (string content) => {});

app.MapPut("/account", (string content) => {});

app.MapDelete("/pin", async (int id, IPinService service, CancellationToken cancellationToken) => 
    service.DeletePinAsync(id, cancellationToken));

app.MapDelete("/comment", (int id) => {});

app.Run();

