using Application.CQRS.Users.ResponseDtos;
using Application.Services;
using Common.Exceptions;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Repository.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.CQRS.Users.Handlers;

public class Login
{
    public class LoginRequest : IRequest<Result<LoginResponseDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IConfiguration configuration) : IRequestHandler<LoginRequest, Result<LoginResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IConfiguration _configuration = configuration;

        public async Task<Result<LoginResponseDto>> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            User currentUser = await _unitOfWork.UserRepository.GetUserByEmailAsync(request.Email) ?? throw new BadRequestException("User does not exist");

            var hashedPassword = PasswordHasher.ComputeStringToSha256Hash(request.Password);

            if (hashedPassword != currentUser.PasswordHash)
            {
                throw new BadRequestException("Wrong Password");
            }

            if (currentUser.Email == request.Email && currentUser.PasswordHash == hashedPassword)
            {
                List<Claim> authClaims = [
                    new Claim(ClaimTypes.NameIdentifier , currentUser.Id.ToString()),
                    new Claim(ClaimTypes.Name , currentUser.Name),
                    new Claim(ClaimTypes.Surname , currentUser.Surname),
                    new Claim(ClaimTypes.Email , currentUser.Email),
                    new Claim(ClaimTypes.Role , currentUser.UserRole.ToString())
                    ];

                JwtSecurityToken token = TokenService.CreateToken(authClaims, _configuration);
                string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                string refreshTokenString = TokenService.GenerateRefreshToken();

                RefreshToken refreshToken = new()
                {
                    Token = refreshTokenString,
                    UserId = currentUser.Id,
                    ExpirationDate = DateTime.Now.AddDays(double.Parse(_configuration.GetSection("JWT:RefreshTokenExpirationDays").Value!)),
                };

                await _unitOfWork.RefreshTokenRepository.SaveRefreshToken(refreshToken);
                await _unitOfWork.SaveChangeAsync();

                LoginResponseDto response = new()
                {
                    AccessToken = tokenString,
                    RefreshToken = refreshTokenString
                };

                return new Result<LoginResponseDto> { Data = response };
            }

            return new Result<LoginResponseDto> { Data = null, Errors = ["Something went wrong!!!"], IsSuccess = false };
        }
    }

}
