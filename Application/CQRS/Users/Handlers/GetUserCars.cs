using Application.CQRS.Cars.ResponseDtos;
using Application.CQRS.Users.ResponseDtos;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Repository.Common;

namespace Application.CQRS.Users.Handlers;

public class GetUserCars
{
    public class GetUserCarsQuery : IRequest<Result<List<GetUserCarsDto>>> 
    {
        public int UserId { get; set; }
    }

    public sealed class Handler : IRequestHandler<GetUserCarsQuery, Result<List<GetUserCarsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Handler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetUserCarsDto>>> Handle(GetUserCarsQuery request, CancellationToken cancellationToken)
        {
            var cars = await _unitOfWork.UserRepository.GetUserCarAsync(request.UserId); 

            if (cars == null || !cars.Any())
            {
                return new Result<List<GetUserCarsDto>>
                {
                    Data = [],
                    Errors = ["No cars found"],
                    IsSuccess = false
                };
            }

            var response = _mapper.Map<List<GetUserCarsDto>>(cars); 

            return new Result<List<GetUserCarsDto>>
            {
                Data = response,
                Errors = [],
                IsSuccess = true
            };
        }
    }
}
