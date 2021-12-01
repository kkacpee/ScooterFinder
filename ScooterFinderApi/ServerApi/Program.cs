using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ServerApi.DTO;
using ServerApi.DTO.Comment;
using ServerApi.DTO.Pin;
using ServerApi.Extensions;
using ServerApi.Persistance;
using ServerApi.Persistance.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("AppDb")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {new OpenApiSecurityScheme{Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }}, new List<string>()}
    });
});

builder.Services.AddControllers().AddAndConfigureFluentValidation();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
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

app.MapGet("/", () => "Hello World!");//.Produces<Scooter>().RequireAuthorization()//.AllowAnonymous();

app.MapGet("/pins", () => new { id = 0 }).Produces<List<Pin>>();

app.MapGet("/pin-details", () => new { id = 0 }).Produces<Pin>();

app.MapGet("/account", () => new { id = 0 }).Produces<User>();

app.MapPost("/pin", (AddPinRequest pin) => {});

app.MapPost("/login", (LoginRequest login) => {});

app.MapPost("/register", (LoginRequest register) => {});

app.MapPost("/comment", (AddCommentRequest comment) => {});

app.MapPut("/pin", (string content) => {});

app.MapPut("/account", (string content) => {});

app.MapDelete("/pin", (int id) => {});

app.MapDelete("/comment", (int id) => {});


app.Run();

