using ServerApi.DTO;
using ServerApi.Persistance.Models;
using ServerApi.Repositories;
using ServerApi.Services;
using System.Security.Cryptography;
using System;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace ServerApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUser(int id, CancellationToken cancellationToken) => await _userRepository.GetSingleOrDefaultAsync(x => x.Id == id, x => x, cancellationToken);

        public async Task<IResult> Login(LoginRequest dto, IConfiguration configuration, CancellationToken cancellationToken)
        {
            if(!await _userRepository.AnyAsync(x => x.Email == dto.Email, cancellationToken))
            {
                return Results.BadRequest("User with given email does not exist!");
            }

            var user = await _userRepository.GetSingleAsync(x => x.Email == dto.Email,x => x, cancellationToken);

            if(!VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Results.BadRequest("Given password is invalid!");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, dto.Email)
            };

            var token = new JwtSecurityToken
            (
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(60),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Results.Ok(tokenString);
        }

        public async Task<IResult> Register(RegisterRequest dto, CancellationToken cancellationToken)
        {
            if(await _userRepository.AnyAsync(x => x.Email == dto.Email, cancellationToken))
            {
                return Results.BadRequest("The user with given email already exists");
            }

            HashPassword(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = Convert.ToBase64String(passwordHash),
                PasswordSalt = Convert.ToBase64String(passwordSalt),
                DisplayName = dto.DisplayName
            };

            await _userRepository.AddAsync(user, cancellationToken);

            return Results.Ok();
        }

        private void HashPassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPassword(string password, string passwordHash, string passwordSalt)
        {
            using (var hmac = new HMACSHA512(Convert.FromBase64String(passwordSalt)))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                var pass = Convert.ToBase64String(computedHash);
                return pass.Equals(passwordHash);
            }
        }
    }
}
