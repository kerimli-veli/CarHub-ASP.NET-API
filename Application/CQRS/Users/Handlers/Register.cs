using Application.CQRS.Users.ResponseDtos;
using Common.GlobalResponses.Generics;
using MediatR;
using AutoMapper;
using Repository.Common;
using Common.Exceptions;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Application.Services;

namespace Application.CQRS.Users.Handlers;

public class Register
{
    public class RegisterCommand : IRequest<Result<RegisterDto>>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public IFormFile UserImage { get; set; }
    }

    public sealed class Handler : IRequestHandler<RegisterCommand, Result<RegisterDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<RegisterDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _unitOfWork.UserRepository.GetUserByEmailAsync(request.Email);
            if (currentUser != null)
                throw new BadRequestException("User is already exist with provided mail");

            var user = _mapper.Map<User>(request);
            var hashPassword = PasswordHasher.ComputeStringToSha256Hash(request.Password);
            user.PasswordHash = hashPassword;
            user.CreatedBy = user.Id;

            if (request.UserImage != null)
            {
                var userImagePath = await ImageService.SaveImageAsync(request.UserImage, "uploads");
                user.UserImagePath = userImagePath;
            }

            await _unitOfWork.UserRepository.RegisterAsync(user);

            var response = _mapper.Map<RegisterDto>(user);

            return new Result<RegisterDto>
            {
                Data = response,
                Errors = new List<string>(),
                IsSuccess = true
            };
        }
    }

}
