using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetCore.AutoRegisterDi;
using ServerApi.DTO;
using ServerApi.DTO.Comment;
using ServerApi.DTO.Pin;
using ServerApi.DTO.User;
using ServerApi.Persistance;
using ServerApi.Persistance.Models;
using ServerApi.Repositories;
using ServerApi.Services;
using ServerApi.Validation;
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

builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterRequestValidator>());

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

app.MapGet("/", () => "Hello World!");

app.MapGet("/pins", async (IPinService service, CancellationToken cancellationToken) => 
    await service.GetPinsAsync(cancellationToken)).Produces<List<Pin>>();

app.MapGet("/pin-details", async (int id, IPinService service, CancellationToken cancellationToken) => 
    await service.GetPinAsync(id, cancellationToken)).Produces<PinDetailsResponse>();

app.MapGet("/account", async (int id, IUserService service, CancellationToken cancellationToken) => 
    await service.GetUser(id, cancellationToken)).Produces<User>();

app.MapPost("/pin", async (AddPinRequest pin, IPinService service, IValidator<AddPinRequest> validator, CancellationToken cancellationToken) =>
    {
        var validationResult = validator.Validate(pin);
        if (!validationResult.IsValid)
        {
            var errors = new { errors = validationResult.Errors.Select(x => x.ErrorMessage) };
            return Results.BadRequest(errors);
        }
        return await service.AddPinAsync(pin, cancellationToken);
    });

app.MapPost("/login", async (LoginRequest login, IUserService service, IValidator<LoginRequest> validator, CancellationToken cancellationToken) =>
    {
        var validationResult = validator.Validate(login);
        if (!validationResult.IsValid)
        {
            var errors = new { errors = validationResult.Errors.Select(x => x.ErrorMessage) };
            return Results.BadRequest(errors);
        }
        return await service.Login(login, builder.Configuration, cancellationToken);
    }).AllowAnonymous();

app.MapPost("/register", async (RegisterRequest register, IUserService service, IValidator<RegisterRequest> validator, CancellationToken cancellationToken) =>
{
    var validationResult = validator.Validate(register);
    if (!validationResult.IsValid)
    {
        var errors = new { errors = validationResult.Errors.Select(x => x.ErrorMessage) };
        return Results.BadRequest(errors);
    }
    return await service.Register(register, cancellationToken);
}).AllowAnonymous();

app.MapPost("/comment", async (AddCommentRequest comment, ICommentService service, IValidator<AddCommentRequest> validator, CancellationToken cancellationToken) =>
    {
        var validationResult = validator.Validate(comment);
        if (!validationResult.IsValid)
        {
            var errors = new { errors = validationResult.Errors.Select(x => x.ErrorMessage) };
            return Results.BadRequest(errors);
        }
        return await service.AddCommentAsync(comment, cancellationToken);
    });

app.MapPut("/pin", async (EditPinRequest dto, IPinService service, IValidator<EditPinRequest> validator, CancellationToken cancellationToken) =>
    {
        var validationResult = validator.Validate(dto);
        if (!validationResult.IsValid)
        {
            var errors = new { errors = validationResult.Errors.Select(x => x.ErrorMessage) };
            return Results.BadRequest(errors);
        }
        return await service.UpdatePinAsync(dto, cancellationToken);
    });

app.MapPut("/account", async (EditUserRequest dto, IUserService service, IValidator<EditUserRequest> validator, CancellationToken cancellationToken) => 
    {
        var validationResult = validator.Validate(dto);
        if (!validationResult.IsValid)
        {
            var errors = new { errors = validationResult.Errors.Select(x => x.ErrorMessage) };
            return Results.BadRequest(errors);
        }
        return await service.EditUserAsync(dto, cancellationToken);
    });

app.MapDelete("/pin", async (int id, IPinService service, CancellationToken cancellationToken) => 
    await service.DeletePinAsync(id, cancellationToken));

app.MapDelete("/comment", async (int id, ICommentService service, CancellationToken cancellationToken) => 
    await service.DeleteCommentAsync(id, cancellationToken));

app.Run();

