using Application.CQRS.Users.ResponseDtos;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponses.Generics;
using MediatR;
using Microsoft.AspNetCore.Http;
using Repository.Common;

namespace Application.CQRS.Users.Handlers;

public class Update
{
    public class Command : IRequest<Result<UpdateDto>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public IFormFile? UserImage { get; set; } 

        //public string PasswordHash { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<Command, Result<UpdateDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<UpdateDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            var currentUser = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);
            if (currentUser == null) throw new BadRequestException($"User does not exist with id {request.Id}");

            // Şəkil yükləmə
            if (request.UserImage != null && request.UserImage.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.UserImage.FileName)}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.UserImage.CopyToAsync(stream);
                }

                currentUser.UserImagePath = $"uploads/{fileName}";
            }

            currentUser.Name = request.Name;
            currentUser.Surname = request.Surname;
            currentUser.Email = request.Email;
            currentUser.Phone = request.Phone;
            currentUser.UpdatedBy = currentUser.Id;

            _unitOfWork.UserRepository.Update(currentUser);

            var response = _mapper.Map<UpdateDto>(currentUser);

            return new Result<UpdateDto>()
            {
                Data = response,
                Errors = [],
                IsSuccess = true
            };
        }
    }

}
