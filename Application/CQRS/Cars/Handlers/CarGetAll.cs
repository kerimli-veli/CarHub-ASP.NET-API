using Application.CQRS.Cars.ResponseDtos;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Cars.Handlers;

public class CarGetAll
{
    public record struct GetAllCarsQuery : IRequest<Result<List<CarGetAllDto>>> { }

    public sealed class Handler : IRequestHandler<GetAllCarsQuery, Result<List<CarGetAllDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Handler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<CarGetAllDto>>> Handle(GetAllCarsQuery request, CancellationToken cancellationToken)
        {
            var cars = _unitOfWork.CarRepository.GetAll().ToList();

            if (!cars.Any())
                return new Result<List<CarGetAllDto>>
                {
                    Data = [],
                    Errors = ["No cars found"],
                    IsSuccess = false
                };

            var response = _mapper.Map<List<CarGetAllDto>>(cars);

            return new Result<List<CarGetAllDto>>
            {
                Data = response,
                Errors = [],
                IsSuccess = true
            };
        }

    }
}
